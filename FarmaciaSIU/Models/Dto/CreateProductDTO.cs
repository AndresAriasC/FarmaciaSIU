using System.ComponentModel.DataAnnotations;

namespace FarmaciaSIU.Models.Dto
{
    public class CreateProductDTO
    {
        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }

    }
}
