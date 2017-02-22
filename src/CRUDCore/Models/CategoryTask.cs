using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDCore.Models
{
    public class CategoryTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        [MinLength(5)]
        public string Title { get; set; }

        [MinLength(10)]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public bool IsActived { get; set; }

        public ICollection<Tasks> Tasks { get; set; }
    }
}