namespace JsonDotNet.CustomContractResolvers.Samples.Website.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    using JsonDotNet.CustomContractResolvers;

    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets the JSON string representing the object to be parsed.
        /// </summary>
        /// <value>
        /// The JSON string.
        /// </value>
        public string Json { get; set; }

        /// <summary>
        /// Gets or sets the properties to serialize.
        /// </summary>
        /// <value>
        /// The properties to serialize.
        /// </value>
        public string Properties { get; set; }

        /// <summary>
        /// Gets or sets the properties to exclude from serialization.
        /// </summary>
        /// <value>
        /// The properties to exclude.
        /// </value>
        [Display(Name = "Exclude properties")]
        public string ExcludeProperties { get; set; }

        /// <summary>
        /// Gets or sets the property match mode.
        /// </summary>
        /// <value>
        /// The property match mode.
        /// </value>
        [Display(Name = "Property match mode")]
        public PropertyMatchMode PropertyMatchMode { get; set; }
        
        /// <summary>
        /// Gets or sets the resulting serialized JSON.
        /// </summary>
        /// <value>
        /// The serialized json.
        /// </value>
        public string JsonSerializationResult { get; set; }
    }
}