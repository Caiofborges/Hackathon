using HackathonAPI.Entities;

namespace HackathonAPI.Interfaces
{
    public interface IMovimentacaoService
    {
        List<Movimentacao> ObterMovimentacoes(DateTime? dataMovimentacao);
        Movimentacao ObterPorId(int id);
        List<Movimentacao>? ObterPorFuncionario(int idColaborador, DateTime? dataAvalicao);
        int CriarMovimentacao(Movimentacao Movimentacao);
        void AtualizarMovimentacao(Movimentacao MovimentacaoOriginal, Movimentacao MovimentacaoAlteracoes);
        void RemoverMovimentacao(int id);
    }
}
