namespace JsonDotNet.CustomContractResolvers.Samples.Website.Controllers
{
    using System.Web.Mvc;

    using JsonDotNet.CustomContractResolvers;
    using JsonDotNet.CustomContractResolvers.Samples.Website.Models;
    using JsonDotNet.CustomContractResolvers.Samples.Website.ViewModels.Home;

    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var movie = CreateMovieToSerialize();
            var movieAsJson = JsonConvert.SerializeObject(movie);
            var model = new IndexViewModel { Json = movieAsJson };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Create a PropertiesContractResolver instance
                var propertiesContractResolver = new PropertiesContractResolver();

                // Set the properties, exclude properties and property match mode to the values specified by the user
                propertiesContractResolver.Properties.Add(model.Properties ?? string.Empty);
                propertiesContractResolver.ExcludeProperties.Add(model.ExcludeProperties ?? string.Empty);
                propertiesContractResolver.PropertyMatchMode = model.PropertyMatchMode;

                // Create a JsonSerializerSettings instance that uses our PropertiesContractResolver instance
                // to resolve contracts (thereby influencing the generated JSON)
                var serializerSettings = new JsonSerializerSettings { ContractResolver = propertiesContractResolver };

                // Create the object to serialize
                var movieToSerialize = CreateMovieToSerialize();

                // Convert the object using our PropertiesContractResolver instance 
                model.JsonSerializationResult = JsonConvert.SerializeObject(movieToSerialize, serializerSettings);
            }

            return this.View(model);
        }

        private static Movie CreateMovieToSerialize()
        {
            return new Movie
            {
                Id = 12,
                Title = "Inception",
                Director = CreateDirectorToSerialize()
            };
        }

        private static Director CreateDirectorToSerialize()
        {
            return new Director
            {
                Id = 12,
                Name = "Christopher Nolan",
                Age = 48,
            };
        }
    }
}