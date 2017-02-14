using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDCore.Models
{
    public class CategoryTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Status")]
        public bool IsActived { get; set; }

        public ICollection<Tasks> Tasks { get; set; }
    }
}