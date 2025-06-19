using HackathonAPI.DTOs.Request;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackathonAPI.Controllers
{
    [Route("/api/ocorrencias")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly IOcorrenciaService _ocorrenciaService;

        public OcorrenciaController(IOcorrenciaService ocorrenciaService) =>
            _ocorrenciaService = ocorrenciaService;

        [HttpGet]
        public ActionResult<List<Ocorrencia>> ObterOcorrenciaes(DateTime? dataOcorrencia)
        {
            var ocorrencias = _ocorrenciaService.ObterOcorrencias(dataOcorrencia);
            if (ocorrencias == null || ocorrencias.Count == 0)
                return NoContent();

            return Ok(ocorrencias);
        }

        [HttpGet("{id}")]
        public ActionResult<Ocorrencia> ObterOcorrenciaPorId(int id)
        {
            var ocorrencia = _ocorrenciaService.ObterPorId(id);
            if (ocorrencia == null)
                return NotFound();

            return Ok(ocorrencia);
        }

        [HttpGet("porcolaborador/{idColaborador}")]
        public ActionResult<Ocorrencia> ObterOcorrenciaPorColaborador(int idColaborador, DateTime? dataOcorrencia)
        {
            var ocorrencia = _ocorrenciaService.ObterPorFuncionario(idColaborador, dataOcorrencia);
            if (ocorrencia == null)
                return NotFound();

            return Ok(ocorrencia);
        }

        [HttpPost]
        public ActionResult CriarOcorrencia(OcorrenciaRequest ocorrencia)
        {
            var ocorrenciaEntidade = ocorrencia.ConverterParaEntidade();
            var id = _ocorrenciaService.CriarOcorrencia(ocorrenciaEntidade);
            return CreatedAtAction(nameof(ObterOcorrenciaPorId), new { Id = id }, id);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarOcorrencia(int id, OcorrenciaRequest ocorrencia)
        {
            var ocorrenciaOriginal = _ocorrenciaService.ObterPorId(id);
            if (ocorrenciaOriginal == null)
                return NotFound();

            var ocorrenciaAlteracoes = ocorrencia.ConverterParaEntidade();
            _ocorrenciaService.AtualizarOcorrencia(ocorrenciaOriginal, ocorrenciaAlteracoes);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult ExcluirOcorrencia(int id)
        {
            try
            {
                _ocorrenciaService.RemoverOcorrencia(id);
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
