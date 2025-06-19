using HackathonAPI.Entities;

namespace HackathonAPI.Interfaces
{
    public interface IOcorrenciaService
    {
        List<Ocorrencia> ObterOcorrencias(DateTime? dataOcorrencia);
        Ocorrencia ObterPorId(int id);
        List<Ocorrencia>? ObterPorFuncionario(int id, DateTime? dataOcorrencia);
        int CriarOcorrencia(Ocorrencia Ocorrencia);
        void AtualizarOcorrencia(Ocorrencia OcorrenciaOriginal, Ocorrencia OcorrenciaAlteracoes);
        void RemoverOcorrencia(int id);
    }
}
