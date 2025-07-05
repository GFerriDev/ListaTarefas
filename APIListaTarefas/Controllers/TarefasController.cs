using AppLista.Server.Models;
using AppLista.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppLista.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefasController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> Get()
        {
            var tarefas = await _tarefaService.GetAllAsync();
            return Ok(tarefas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Tarefa tarefa)
        {
            await _tarefaService.AddAsync(tarefa);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Tarefa tarefa)
        {
            var sucesso = await _tarefaService.UpdateAsync(id, tarefa);
            return sucesso ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tarefaService.RemoveAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpPut("{id}/concluir")]
        public async Task<ActionResult> Concluir(int id)
        {
            var sucesso = await _tarefaService.ConcluirAsync(id);
            return sucesso ? Ok() : NotFound();
        }
    }
}