using HackathonAPI.Data.Context;
using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly DataContext _context;

        public ColaboradorService(DataContext context)
        {
            _context = context;
        }

        public List<Colaborador> ObterColaboradors(string nome)
        {
            return _context.Colaboradores       
                .Where(p => string.IsNullOrWhiteSpace(nome) || p.Nome.Contains(nome))
                .ToList();
        }   

        public Colaborador? ObterPorId(int id)
        {
            return _context.Colaboradores
                .FirstOrDefault(p => p.Id == id);
        }

        public int CriarColaborador(Colaborador colaborador)
        {
            _context.Colaboradores.Add(colaborador);
            _context.SaveChanges();
            return colaborador.Id;
        }

        public void AtualizarColaborador(Colaborador colaboradorOriginal, Colaborador colaboradorAlteracoes)
        {
            colaboradorOriginal.AlterarDados(colaboradorAlteracoes.Nome,
                colaboradorAlteracoes.Salario);
            _context.SaveChanges();
        }

        public void RemoverColaborador(int id)
        {
            var colaborador = ObterPorId(id);
            if (colaborador == null)
                throw new ArgumentException("O colaborador com o identificador informado não existe", "id");

            _context.Colaboradores.Remove(colaborador);
            _context.SaveChanges();
        }
    }
}