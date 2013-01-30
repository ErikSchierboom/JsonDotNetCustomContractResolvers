namespace JsonDotNet.CustomContractResolvers.Samples.Website.Controllers
{
    using System.Web.Mvc;

    using JsonDotNet.CustomContractResolvers;
    using JsonDotNet.CustomContractResolvers.Samples.Website.ViewModels.Home;

    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            // Use a default JSON string
            model.Json = "{\"Id\":12,\"Title\":\"Inception\",\"Director\":\"Christopher Nolan\"}";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Create a PropertiesContractResolver instance using the specified properties
                // and exclude properties values
                var propertiesContractResolver = new PropertiesContractResolver();
                propertiesContractResolver.Properties.Add(model.Properties);
                propertiesContractResolver.ExcludeProperties.Add(model.ExcludeProperties);

                // Create a JsonSerializerSettings instance that uses our PropertiesContractResolver instance
                // to resolve contracts (thereby influencing the generated JSON)
                var serializerSettings = new JsonSerializerSettings { ContractResolver = propertiesContractResolver };

                // Create an object from the specified JSON
                var deserializeObject = JsonConvert.DeserializeObject(model.Json);

                // Convert the object using our PropertiesContractResolver instance 
                model.JsonSerializationResult = JsonConvert.SerializeObject(deserializeObject, serializerSettings);
            }

            return this.View(model);
        }
    }
}