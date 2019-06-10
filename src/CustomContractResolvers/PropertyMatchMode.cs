using System.ComponentModel.DataAnnotations;

namespace JsonDotNet.CustomContractResolvers
{
    public enum PropertyMatchMode
    {
        [Display(Name = "Name")]
        Name,
        
        [Display(Name = "Name and type")]
        NameAndType
    }
}