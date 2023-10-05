using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ecommerce_iniciativatena.Service;
using ecommerce_iniciativatena.Model;
using Microsoft.AspNetCore.Authorization;

namespace ecommerce_iniciativatena.Controllers
{
    [Route("~/produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IValidator<Produto> _produtoValidator;

        public ProdutoController(
           IProdutoService produtoService,
           IValidator<Produto> produtoValidator)
        {
            _produtoService = produtoService;
            _produtoValidator = produtoValidator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _produtoService.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _produtoService.GetById(id);

            if (Resposta is null)
            {
                return NotFound();
            }

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult> GetByNome(string nome)
        {
            return Ok(await _produtoService.GetByNome(nome));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Produto produto)
        {
            var validarProduto = await _produtoValidator.ValidateAsync(produto);

            if (!validarProduto.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarProduto);

            var Resposta = await _produtoService.Create(produto);

            if (Resposta is null)
            {
                return BadRequest("Produto não encontrado!");
            }
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Produto produto)
        {
            if (produto.Id == 0)
                return BadRequest("Id da Produto é inválido!");

            var validarProduto = await _produtoValidator.ValidateAsync(produto);

            if (!validarProduto.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarProduto);

            var Resposta = await _produtoService.Update(produto);

            if (Resposta is null)
                return NotFound("Produto e/ou Categoria não foi encontrada");

            return Ok(Resposta);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaProduto = await _produtoService.GetById(id);

            if (BuscaProduto is null)
                return NotFound("Produto não foi encontrado");

            await _produtoService.Delete(BuscaProduto);

            return NoContent();
        }
    }
}
