using HackathonAPI.DTOs.Request;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackathonAPI.Controllers
{    
    [ApiController]
    [Route("/api/movimentacoes")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacaoService _movimentacaoService;

        public MovimentacaoController(IMovimentacaoService movimentacaoService) =>
            _movimentacaoService = movimentacaoService;

        [HttpGet]
        public ActionResult<List<Movimentacao>> ObterMovimentacaoes(DateTime? dataMovimentacao)
        {
            var movimentacaoes = _movimentacaoService.ObterMovimentacoes(dataMovimentacao);
            if (movimentacaoes == null || movimentacaoes.Count == 0)
                return NoContent();

            return Ok(movimentacaoes);
        }

        [HttpGet("{id}")]
        public ActionResult<Movimentacao> ObterMovimentacaoPorId(int id)
        {
            var movimentacao = _movimentacaoService.ObterPorId(id);
            if (movimentacao == null)
                return NotFound();

            return Ok(movimentacao);
        }

        [HttpGet("porcolaborador/{id}")]
        public ActionResult<Movimentacao> ObterMovimentacaoPorColaborador(int idColaborador, DateTime? dataMovimentacao)
        {
            var movimentacao = _movimentacaoService.ObterPorFuncionario(idColaborador, dataMovimentacao);
            if (movimentacao == null)
                return NotFound();

            return Ok(movimentacao);
        }

        [HttpPost]
        public ActionResult CriarMovimentacao(MovimentacaoRequest movimentacao)
        {
            var movimentacaoEntidade = movimentacao.ConverterParaEntidade();
            var id = _movimentacaoService.CriarMovimentacao(movimentacaoEntidade);
            return CreatedAtAction(nameof(ObterMovimentacaoPorId), new { Id = id }, id);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarMovimentacao(int id, MovimentacaoRequest movimentacao)
        {
            var movimentacaoOriginal = _movimentacaoService.ObterPorId(id);
            if (movimentacaoOriginal == null)
                return NotFound();

            var movimentacaoAlteracoes = movimentacao.ConverterParaEntidade();
            _movimentacaoService.AtualizarMovimentacao(movimentacaoOriginal, movimentacaoAlteracoes);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult ExcluirMovimentacao(int id)
        {
            try
            {
                _movimentacaoService.RemoverMovimentacao(id);
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
