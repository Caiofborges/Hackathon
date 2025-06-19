using HackathonAPI.Data.Context;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly DataContext _context;

        public AvaliacaoService(DataContext context)
        {
            _context = context;
        }

        public List<Avaliacao> ObterAvaliacoes(DateTime? dataAvalicao)
        {
            return _context.Avaliacoes
                .Include(x => x.Colaborador)
                .Where(p => dataAvalicao != null ? int.Parse(p.DataAvaliacao.ToString("AAAAmmdd")) == int.Parse(dataAvalicao.Value.ToString("AAAAmmdd")) : true)
                .ToList();
        }

        public Avaliacao? ObterPorId(int id)
        {
            return _context.Avaliacoes
                .Include(x => x.Colaborador)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Avaliacao>? ObterPorFuncionario(int idColaborador, DateTime? dataAvalicao)
        {
            return _context.Avaliacoes
                .Include(x => x.Colaborador)
                .Where(p => 
                p.ColaboradorId == idColaborador &&
                dataAvalicao != null ? int.Parse(p.DataAvaliacao.ToString("AAAAmmdd")) == int.Parse(dataAvalicao.Value.ToString("AAAAmmdd")) : true)
                .ToList();
        }

        public int CriarAvaliacao(Avaliacao colaborador)
        {
            _context.Avaliacoes.Add(colaborador);
            _context.SaveChanges();
            return colaborador.Id;
        }

        public void AtualizarAvaliacao(Avaliacao colaboradorOriginal, Avaliacao colaboradorAlteracoes)
        {
            colaboradorOriginal.AlterarDados(colaboradorAlteracoes.Nota);
            _context.SaveChanges();
        }

        public void RemoverAvaliacao(int id)
        {
            var colaborador = ObterPorId(id);
            if (colaborador == null)
                throw new ArgumentException("A avaliação com o identificador informado não existe", "id");

            _context.Avaliacoes.Remove(colaborador);
            _context.SaveChanges();
        }
    }
}