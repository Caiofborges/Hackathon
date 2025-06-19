namespace HackathonAPI.Entities
{
    public class Ocorrencia : Entity
    {
        public int ColaboradorId { get; set; }
        public DateTime DataOcorrencia { get; private set; }
        public int Nivel { get; set; }
        public string Motivo { get; set; }
        public Colaborador? Colaborador { get; private set; }

        public Ocorrencia(int colaboradorId, DateTime dataOcorrencia, int nivel, string motivo)
        {
            ColaboradorId = colaboradorId;
            DataOcorrencia = dataOcorrencia;
            Nivel = nivel;
            Motivo = motivo;
        }

        public void AlterarDados(int nivel, string motivo)
        {
            Nivel = nivel;
            Motivo = motivo;
        }
    }
}
