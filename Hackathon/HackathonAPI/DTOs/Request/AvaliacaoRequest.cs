using HackathonAPI.Entities;

namespace HackathonAPI.DTOs.Request
{
    public class AvaliacaoRequest
    {
        public int ColaboradorId { get; set; }
        public DateTime DataAvaliacao { get; private set; }
        public double Nota { get; set; }

        public Avaliacao ConverterParaEntidade()
        {
            return new Avaliacao(ColaboradorId, DataAvaliacao, Nota);
        }
    }
}
