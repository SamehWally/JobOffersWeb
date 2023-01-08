using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RoleViewModel
    {
        public string? Id { get; set; }
        [Display(Name ="إلاسم")]
        public string? Name { get; set; }
    }
}
