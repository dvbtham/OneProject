using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDCore.Models
{
    public class Tasks
    {
        public int ID { get; set; }

        [Display(Name = "In Category Task")]
        public int IdCategoryTask { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "Deadline Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeadlineDate { get; set; }

        public string Description { get; set; }

        [Required]
        public float UnitPer { get; set; }

        [Display(Name = "Status")]
        public bool IsActived { get; set; }

        [ForeignKey("IdCategoryTask")]
        public virtual CategoryTask CategoryTask { get; set; }

        public SelectList CategoryTasks { get; set; }
    }
}