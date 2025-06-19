using HackathonAPI.DTOs.Request;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackathonAPI.Controllers
{
    [ApiController]
    [Route("/api/colaboradores")]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorService _colaboradorService;

        public ColaboradorController(IColaboradorService colaboradorService) =>
            _colaboradorService = colaboradorService;

        [HttpGet]
        public ActionResult<List<Colaborador>> ObterColaboradores(string? nome)
        {
            var colaboradores = _colaboradorService.ObterColaboradores(nome);
            if (colaboradores == null || colaboradores.Count == 0)
                return NoContent();

            return Ok(colaboradores);
        }

        [HttpGet("{id}")]
        public ActionResult<Colaborador> ObterColaboradorPorId(int id)
        {
            var colaborador = _colaboradorService.ObterPorId(id);
            if (colaborador == null)
                return NotFound();

            return Ok(colaborador);
        }

        [HttpPost]
        public ActionResult CriarColaborador(ColaboradorRequest colaborador)
        {
            var colaboradorEntidade = colaborador.ConverterParaEntidade();
            var id = _colaboradorService.CriarColaborador(colaboradorEntidade);
            return CreatedAtAction(nameof(ObterColaboradorPorId), new { Id = id }, id);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarColaborador(int id, ColaboradorRequest colaborador)
        {
            var colaboradorOriginal = _colaboradorService.ObterPorId(id);
            if (colaboradorOriginal == null)
                return NotFound();

            var colaboradorAlteracoes = colaborador.ConverterParaEntidade();
            _colaboradorService.AtualizarColaborador(colaboradorOriginal, colaboradorAlteracoes);
            return NoContent();
        }      


        [HttpDelete("{id}")]
        public ActionResult ExcluirColaborador(int id)
        {
            try
            {
                _colaboradorService.RemoverColaborador(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName.Equals("id"))
                    return NotFound();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensagem = ex.Message });
            }
        }
    }
}