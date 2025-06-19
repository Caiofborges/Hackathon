using HackathonAPI.Data.Context;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Services
{
    public class OcorrenciaService : IOcorrenciaService
    {
        private readonly DataContext _context;

        public OcorrenciaService(DataContext context)
        {
            _context = context;
        }

        public List<Ocorrencia> ObterOcorrencias(DateTime? dataOcorrencia)
        {
            return _context.Ocorrencias
                .Include(x => x.Colaborador)
                .Where(p => dataOcorrencia != null ? int.Parse(p.DataOcorrencia.ToString("AAAAmmdd")) == int.Parse(dataOcorrencia.Value.ToString("AAAAmmdd")) : true)
                .ToList();
        }

        public Ocorrencia? ObterPorId(int id)
        {
            return _context.Ocorrencias
                .Include(x => x.Colaborador)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Ocorrencia>? ObterPorFuncionario(int idColaborador, DateTime? dataOcorrencia)
        {
            return _context.Ocorrencias
                .Include(x => x.Colaborador)
                .Where(p =>
                p.ColaboradorId == idColaborador &&
                dataOcorrencia != null ? p.DataOcorrencia == dataOcorrencia : true)
                .ToList();
        }

        public int CriarOcorrencia(Ocorrencia colaborador)
        {
            _context.Ocorrencias.Add(colaborador);
            _context.SaveChanges();
            return colaborador.Id;
        }

        public void AtualizarOcorrencia(Ocorrencia colaboradorOriginal, Ocorrencia colaboradorAlteracoes)
        {
            colaboradorOriginal.AlterarDados(colaboradorAlteracoes.Nivel, colaboradorAlteracoes.Motivo);
            _context.SaveChanges();
        }

        public void RemoverOcorrencia(int id)
        {
            var colaborador = ObterPorId(id);
            if (colaborador == null)
                throw new ArgumentException("A ocorrência com o identificador informado não existe", "id");

            _context.Ocorrencias.Remove(colaborador);
            _context.SaveChanges();
        }
    }
}