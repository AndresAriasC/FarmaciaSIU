using System.ComponentModel.DataAnnotations;

namespace FarmaciaSIU.Models.Dto
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        [Required]
        public string ? ProductName { get; set; }

    }
}
