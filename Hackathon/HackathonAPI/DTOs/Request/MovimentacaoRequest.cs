using HackathonAPI.Entities;

namespace HackathonAPI.DTOs.Request
{
    public class MovimentacaoRequest
    {
        public int ColaboradorId { get; set; }
        public DateTime DataMovimentacao { get; private set; }
        public decimal SalarioAnterior { get; set; }
        public decimal SalarioAtual { get; set; }

        public Movimentacao ConverterParaEntidade()
        {
            return new Movimentacao(ColaboradorId, DataMovimentacao, SalarioAnterior, SalarioAtual);
        }
    }
}
