using DevHive.Web.Models;
using DevHive.Web.Models.ViewModels;
using DevHive.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevHive.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository
            )
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? tag, int pageNumber = 1, int pageSize = 1 )
        {
            if (pageSize < 1) pageSize = 5;
            if(pageSize > 50) pageSize = 50;
           

            // 1) Get tags
            var allTags = await tagRepository.GetAllAsync();

            // 2) Count posts 
            var totalPosts = await blogPostRepository.CountAsync(tag);

            // 3) Total pages
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            if (totalPages < 1) totalPages = 1;

            // 4) so it never points to an empty page
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages) pageNumber = totalPages;

            // 5) Get posts for this page 
            var posts = await blogPostRepository.GetAllAsync(
                searchQuery: null,
                sortBy: null,
                sortDirection: null,
                pageSize: pageSize,
                pageNumber: pageNumber,
                tagName: tag
            );

            var vm = new HomeViewModel
            {
                Tags = allTags,
                BlogPosts = posts,
                  PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,

                SelectedTag = tag
            };

            return View(vm);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
