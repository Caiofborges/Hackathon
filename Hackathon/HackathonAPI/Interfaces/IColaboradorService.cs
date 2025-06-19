using HackathonAPI.Entities;

namespace HackathonAPI.Interfaces
{
    public interface IColaboradorService
    {
        List<Colaborador> ObterColaboradors(string nome);      
        Colaborador? ObterPorId(int id);
        int CriarColaborador(Colaborador Colaborador);
        void AtualizarColaborador(Colaborador ColaboradorOriginal, Colaborador ColaboradorAlteracoes);
        void RemoverColaborador(int id);
    }
}