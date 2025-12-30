using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevHive.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        [Required]
        [StringLength(200)]
        public string Heading { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string PageTitle { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        [Url]
        public string FeaturedImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string UrlHandle { get; set; } = string.Empty;

        // Set in controller to DateTime.UtcNow (no UI editing)
        public DateTime PublishedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        public bool Visible { get; set; }

        // Display tags
        public IEnumerable<SelectListItem> Tags { get; set; } = Enumerable.Empty<SelectListItem>();

        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
