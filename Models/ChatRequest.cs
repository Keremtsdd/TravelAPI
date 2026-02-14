using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class ChatRequest
    {
        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = null!;
    }
}
