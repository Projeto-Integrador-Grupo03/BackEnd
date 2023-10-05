using ecommerce_iniciativatena.Model;
using ecommerce_iniciativatena.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_iniciativatena.Controllers
{
    [Authorize]
    [Route("~/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IValidator<Categoria> _categoriaValidator;

        public CategoriaController(
            ICategoriaService categoriaService,
            IValidator<Categoria> categoriaValidator
            )
        {
            _categoriaService = categoriaService;
            _categoriaValidator = categoriaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _categoriaService.GetAll());
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult> GetByNome(string nome)
        {
            return Ok(await _categoriaService.GetByNome(nome));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _categoriaService.GetById(id);

            if (Resposta is null)
                return NotFound();

            return Ok(Resposta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Categoria categoria)
        {

            var validarCategoria = await _categoriaValidator.ValidateAsync(categoria);

            if (!validarCategoria.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarCategoria);
            }

            await _categoriaService.Create(categoria);

            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Categoria categoria)
        {
            if (categoria.Id == 0)
                return BadRequest("Id da categoria é inválido!");

            var validarCategoria = await _categoriaValidator.ValidateAsync(categoria);

            if (!validarCategoria.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarCategoria);
            }

            var Resposta = await _categoriaService.Update(categoria);

            if (Resposta is null)
                return NotFound("Categoria não encontrada!");

            return Ok(Resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaTema = await _categoriaService.GetById(id);

            if (BuscaTema is null)
                return NotFound("Tema não foi encontrada!");

            await _categoriaService.Delete(BuscaTema);

            return NoContent();
        }



    }
}
