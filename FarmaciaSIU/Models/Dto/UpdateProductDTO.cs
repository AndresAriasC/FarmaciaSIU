using System.ComponentModel.DataAnnotations;

namespace FarmaciaSIU.Models.Dto
{
    public class UpdateProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }
    }
}
