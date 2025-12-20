using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Notification : BaseModel
    {
        public string? Title { get; set; }
        public string? Body { get; set; }

        [ForeignKey("User")]
        public string? UserId{ get; set; }
        public User? User { get; set; }


        public bool IsRead { get; set; }
        public string? Type { get; set; }
    }
}
