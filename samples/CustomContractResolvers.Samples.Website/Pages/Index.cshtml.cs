using System.ComponentModel.DataAnnotations;
using JsonDotNet.CustomContractResolvers.Samples.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace JsonDotNet.CustomContractResolvers.Samples.Website.Pages
{
    public class Index : PageModel
    {
        [BindProperty, Display(Name = "JSON"), Required]
        public string Json { get; set; }
        
        [BindProperty, Display(Name = "Include properties")]
        public string IncludeProperties { get; set; }

        [BindProperty, Display(Name = "Exclude properties")]
        public string ExcludeProperties { get; set; }

        [BindProperty, Display(Name = "Property match mode")]
        public PropertyMatchMode PropertyMatchMode { get; set; }

        [Display(Name = "JSON (serialized)")]
        public string JsonSerialized { get; set; }
        
        public void OnGet()
        {
            Json = GetInitialJson();
        }
        
        public void OnPost()
        {
            if (!ModelState.IsValid)
                return;

            JsonSerialized = GetSerializedJson();
        }

        private static string GetInitialJson() =>
            JsonConvert.SerializeObject(CreateMovie());

        private string GetSerializedJson()
        {
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add(IncludeProperties ?? string.Empty);
            propertiesContractResolver.ExcludeProperties.Add(ExcludeProperties ?? string.Empty);
            propertiesContractResolver.PropertyMatchMode = PropertyMatchMode;

            var serializerSettings = new JsonSerializerSettings { ContractResolver = propertiesContractResolver };
            return JsonConvert.SerializeObject(CreateMovie(), serializerSettings);
        }

        private static Movie CreateMovie() =>
            new Movie
            {
                Id = 12,
                Title = "Inception",
                Director = CreateDirector()
            };

        private static Director CreateDirector() =>
            new Director
            {
                Id = 12,
                Name = "Christopher Nolan",
                Age = 48,
            };
    }
}