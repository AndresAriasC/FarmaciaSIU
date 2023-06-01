using FarmaciaSIU.Data;
using FarmaciaSIU.Models;
using FarmaciaSIU.Models.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FarmaciaSIU.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FarmaciaController : ControllerBase
    {
        private readonly FarmaciaContext _db;
        private readonly ILogger<FarmaciaController> _logger;

        public FarmaciaController(ILogger<FarmaciaController> logger, FarmaciaContext db) {

            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts() {
            _logger.LogInformation("Obtener Los Producto");
            return Ok(await _db.Productos.ToListAsync());
        }


        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductDTO>> GetProduct(int id) {
            if (id == 0)
            {
                _logger.LogError($"Error al traer el Producto con ID {id}");
                return BadRequest();
            }
            var product = await _db.Productos.FirstOrDefaultAsync(s => s.ProductId == id);

            if (product == null)
            {
                _logger.LogError($"Error al traer el producto con id {id}");
                return NotFound();
            }

            return Ok(product);
        }

        /*Metodo que se utiliza para agregar un registro nuevo*/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDTO>> AddStudent([FromBody] CreateProductDTO studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //si esto se cumple el producto no se aggrega pero se muestra un mensaje de producto ya existente
            if (await _db.Productos.FirstOrDefaultAsync(s => s.ProductName.ToLower() == studentDto.ProductName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Producto con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (studentDto == null)
            {
                return BadRequest(studentDto);
            }

            Producto modelo = new()
            {
                ProductName = studentDto.ProductName
            };

            await _db.Productos.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetProducts", new { id = modelo.ProductId }, modelo);

        }
        /*Metodo encargado de eliminar un registro*/
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var producto = await _db.Productos.FirstOrDefaultAsync(s => s.ProductId == id);

            if (id == null)
            {
                return NotFound();
            }

            _db.Productos.Remove(producto);
            await _db.SaveChangesAsync(true);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO UpdateDTO)
        {
            if (UpdateDTO == null || id != UpdateDTO.ProductId)
            {
                return BadRequest();
            }

            Producto modelo = new()
            {
                ProductId = UpdateDTO.ProductId,
                ProductName = UpdateDTO.ProductName
            };

            _db.Productos.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialStudent(int id, JsonPatchDocument<UpdateProductDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var product = await _db.Productos.AsNoTracking().FirstOrDefaultAsync(s => s.ProductId == id);

            UpdateProductDTO productDto = new()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName
            };
            if (product == null) return BadRequest();

            patchDto.ApplyTo(productDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Producto modelo = new()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName
            };

            _db.Productos.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
