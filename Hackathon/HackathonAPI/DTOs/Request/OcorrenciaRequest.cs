using HackathonAPI.Entities;

namespace HackathonAPI.DTOs.Request
{
    public class OcorrenciaRequest
    {
        public int ColaboradorId { get; set; }
        public DateTime DataOcorrencia { get; private set; }
        public int Nivel { get; set; }
        public string Motivo { get; set; }

        public Ocorrencia ConverterParaEntidade()
        {
            return new Ocorrencia(ColaboradorId, DataOcorrencia, Nivel, Motivo);
        }
    }
}
