namespace HackathonAPI.Entities
{
    public class Movimentacao : Entity
    {
        public int ColaboradorId { get; set; }
        public DateTime DataMovimentacao { get; private set; }
        public decimal SalarioAnterior { get; set; }
        public decimal SalarioAtual { get; set; }
        public Colaborador? Colaborador { get; private set; }

        public Movimentacao(int colaboradorId, DateTime dataMovimentacao ,  decimal salarioAnterior, decimal salarioAtual)
        {
            ColaboradorId = colaboradorId;
            DataMovimentacao = dataMovimentacao;
            SalarioAnterior = salarioAnterior;
            SalarioAtual = salarioAtual;
        }

        public void AlterarDados(decimal salarioAnterior, decimal salarioAtual)
        {
            SalarioAnterior = salarioAnterior;
            SalarioAtual = salarioAtual;
        }
    }
}
