using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDCore.Models
{
    public class Tasks
    {
        public int ID { get; set; }

        [Display(Name = "Id Category Task")]
        public int IdCategoryTask { get; set; }

        public string Title { get; set; }

        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Deadline Date")]
        public DateTime DeadlineDate { get; set; }

        public string Description { get; set; }

        public float UnitPer { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [ForeignKey("IdCategoryTask")]
        public virtual CategoryTask CategoryTask { get; set; }
    }
}