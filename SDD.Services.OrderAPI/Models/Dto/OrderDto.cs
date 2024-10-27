using System.ComponentModel.DataAnnotations;

namespace SDD.Services.OrderAPI.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
