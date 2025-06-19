using HackathonAPI.Data.Context;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Services
{
    public class MovimentacaoService : IMovimentacaoService
    {
        private readonly DataContext _context;

        public MovimentacaoService(DataContext context)
        {
            _context = context;
        }

        public List<Movimentacao> ObterMovimentacoes(DateTime? dataMovimentacao)
        {
            return _context.Movimentacoes
                .Include(x => x.Colaborador)
                .Where(p => dataMovimentacao != null ? int.Parse(p.DataMovimentacao.ToString("AAAAmmdd")) == int.Parse(dataMovimentacao.Value.ToString("AAAAmmdd")) : true)
                .ToList();
        }

        public Movimentacao? ObterPorId(int id)
        {
            return _context.Movimentacoes
                .Include(x => x.Colaborador)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Movimentacao>? ObterPorFuncionario(int idColaborador, DateTime? dataMovimentacao)
        {
            return _context.Movimentacoes
                .Include(x => x.Colaborador)
                .Where(p =>
                p.ColaboradorId == idColaborador &&
               dataMovimentacao != null ? int.Parse(p.DataMovimentacao.ToString("AAAAmmdd")) == int.Parse(dataMovimentacao.Value.ToString("AAAAmmdd")) : true)
                .ToList();
        }

        public int CriarMovimentacao(Movimentacao colaborador)
        {
            _context.Movimentacoes.Add(colaborador);
            _context.SaveChanges();
            return colaborador.Id;
        }

        public void AtualizarMovimentacao(Movimentacao colaboradorOriginal, Movimentacao colaboradorAlteracoes)
        {
            colaboradorOriginal.AlterarDados(colaboradorAlteracoes.SalarioAnterior, colaboradorAlteracoes.SalarioAtual);
            _context.SaveChanges();
        }

        public void RemoverMovimentacao(int id)
        {
            var colaborador = ObterPorId(id);
            if (colaborador == null)
                throw new ArgumentException("A movimentação com o identificador informado não existe", "id");

            _context.Movimentacoes.Remove(colaborador);
            _context.SaveChanges();
        }
    }
}