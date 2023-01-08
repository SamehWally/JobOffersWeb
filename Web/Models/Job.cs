using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Display(Name ="إسم الوظيفة")]
        public string? JobTitle { get; set; }
        [Display(Name="وصف الوظيفة")]
        public string? JobContent { get; set; }
        [Display(Name ="صورة الوظيفة")]
        public string? JobImage { get; set; }
        [Display(Name = "نوع الوظيفة")]
        public int CategoryId { get; set; }
        [Display(Name = "نوع الوظيفة")]
        public virtual Category? Category { get; set; }
        public string UserId { get; set; } = "";
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
