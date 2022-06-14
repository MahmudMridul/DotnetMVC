using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
