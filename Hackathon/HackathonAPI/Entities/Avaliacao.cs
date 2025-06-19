namespace HackathonAPI.Entities
{
    public class Avaliacao : Entity
    {
        public int ColaboradorId { get; set; }
        public DateTime DataAvaliacao { get; private set; }
        public double Nota { get; set; }
        public Colaborador? Colaborador { get; private set; }

        public Avaliacao(int colaboradorId, DateTime dataAvaliacao, double nota)
        {
            ColaboradorId = colaboradorId;
            DataAvaliacao = dataAvaliacao;
            Nota = nota;
        }

        public void AlterarDados(double nota)
        {
            Nota = nota;
        }
    }
}
