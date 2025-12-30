using DevHive.Web.Models.Domain;
using DevHive.Web.Models.ViewModels;
using DevHive.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;


namespace DevHive.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();

            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    // Get like for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }


                }

                // Get comments for blog post
                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    var identityUser = await userManager.FindByIdAsync(blogComment.UserId.ToString());
                    var name = identityUser?.UserName ?? "Unknown";

                    // אם השם הוא אימייל – תציג רק את החלק לפני @
                    if (name.Contains("@"))
                    {
                        name = name.Split('@')[0];
                    }

                    blogCommentsForView.Add(new BlogComment
                    {
                        Id = blogComment.Id,
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = name,
                        UserId = blogComment.UserId
                    });
                }


                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags?.ToList() ?? new List<Tag>(),
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };

            }

            return View(blogDetailsViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var blogPost = await blogPostRepository.GetAsync(blogDetailsViewModel.Id);

            if (blogPost == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Basic validation: don't allow empty comments
            if (string.IsNullOrWhiteSpace(blogDetailsViewModel.CommentDescription))
            {
                // Reload same page, keep user at the comments section
                var backUrl = Url.Action("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
                return Redirect($"{backUrl}#comments");
            }

            // Get logged in user's id
            var userId = userManager.GetUserId(User);

            var blogComment = new BlogPostComment
            {
                BlogPostId = blogPost.Id,
                Description = blogDetailsViewModel.CommentDescription.Trim(),
                DateAdded = DateTime.UtcNow,
                UserId = Guid.Parse(userId!)
            };

            var createdComment = await blogPostCommentRepository.AddAsync(blogComment);

            // Redirect back to the exact comment (prevents jumping to top)
            var redirectUrl = Url.Action("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            return Redirect($"{redirectUrl}#comment-{createdComment.Id}");
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(Guid commentId, string urlHandle)
        {
            var comment = await blogPostCommentRepository.GetAsync(commentId);

            // If comment doesn't exist, just go back to the post
            if (comment == null)
            {
                var notFoundRedirect = Url.Action("Index", "Blogs", new { urlHandle }) + "#comments";
                return Redirect(notFoundRedirect);
            }

            var currentUserId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");

            // Regular users can delete ONLY their own comments
            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(currentUserId)
                    || !Guid.TryParse(currentUserId, out var currentUserGuid)
                    || comment.UserId != currentUserGuid)
                {
                    return Forbid();
                }
            }

            await blogPostCommentRepository.DeleteAsync(commentId);

            var redirectUrl = Url.Action("Index", "Blogs", new { urlHandle }) + "#comments";
            return Redirect(redirectUrl);
        }

    }
}
