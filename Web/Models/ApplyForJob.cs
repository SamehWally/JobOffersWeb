using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Web.ViewModels;

namespace Web.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; } = 0;
        public virtual Job? Job { get; set; }
        public string UserId { get; set; } = "";
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
