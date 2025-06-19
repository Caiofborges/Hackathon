using HackathonAPI.Entities;

namespace HackathonAPI.Interfaces
{
    public interface IAvaliacaoService
    {
        List<Avaliacao> ObterAvaliacoes(DateTime? dataAvalicao);
        Avaliacao ObterPorId(int id);
        List<Avaliacao>? ObterPorFuncionario(int idColaborador, DateTime? dataAvalicao);
        int CriarAvaliacao(Avaliacao Avaliacao);
        void AtualizarAvaliacao(Avaliacao AvaliacaoOriginal, Avaliacao AvaliacaoAlteracoes);
        void RemoverAvaliacao(int id);
    }
}
