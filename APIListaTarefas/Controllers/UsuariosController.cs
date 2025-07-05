using AppLista.Server.Models;
using AppLista.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppLista.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }


        [HttpPost]
        public async Task<ActionResult<Usuario>> Create([FromBody] Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return BadRequest("Nome é obrigatório.");

            var novoUsuario = await _usuarioService.AddAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = novoUsuario.Id }, novoUsuario);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            var sucesso = await _usuarioService.UpdateAsync(id, usuario);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _usuarioService.DeleteAsync(id);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}
