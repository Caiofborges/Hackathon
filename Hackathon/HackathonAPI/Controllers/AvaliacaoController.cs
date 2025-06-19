using HackathonAPI.DTOs.Request;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using HackathonAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackathonAPI.Controllers
{
    [ApiController]
    [Route("/api/avaliacoes")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        public AvaliacaoController(IAvaliacaoService avaliacaoService) =>
            _avaliacaoService = avaliacaoService;

        [HttpGet]
        public ActionResult<List<Avaliacao>> ObterAvaliacaoes(DateTime? dataAvalicao)
        {
            var avaliacaoes = _avaliacaoService.ObterAvaliacoes(dataAvalicao);
            if (avaliacaoes == null || avaliacaoes.Count == 0)
                return NoContent();

            return Ok(avaliacaoes);
        }

        [HttpGet("{id}")]
        public ActionResult<Avaliacao> ObterAvaliacaoPorId(int id)
        {
            var avaliacao = _avaliacaoService.ObterPorId(id);
            if (avaliacao == null)
                return NotFound();

            return Ok(avaliacao);
        }

        [HttpGet("porcolaborador/{idColaborador}")]
        public ActionResult<Avaliacao> ObterAvaliacaoPorColaborador(int idColaborador, DateTime? dataAvalicao)
        {
            var avaliacao = _avaliacaoService.ObterPorFuncionario(idColaborador, dataAvalicao);
            if (avaliacao == null)
                return NotFound();

            return Ok(avaliacao);
        }

        [HttpPost]
        public ActionResult CriarAvaliacao(AvaliacaoRequest avaliacao)
        {
            var avaliacaoEntidade = avaliacao.ConverterParaEntidade();
            var id = _avaliacaoService.CriarAvaliacao(avaliacaoEntidade);
            return CreatedAtAction(nameof(ObterAvaliacaoPorId), new { Id = id }, id);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarAvaliacao(int id, AvaliacaoRequest avaliacao)
        {
            var avaliacaoOriginal = _avaliacaoService.ObterPorId(id);
            if (avaliacaoOriginal == null)
                return NotFound();

            var avaliacaoAlteracoes = avaliacao.ConverterParaEntidade();
            _avaliacaoService.AtualizarAvaliacao(avaliacaoOriginal, avaliacaoAlteracoes);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult ExcluirAvaliacao(int id)
        {
            try
            {
                _avaliacaoService.RemoverAvaliacao(id);
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

