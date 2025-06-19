using HackathonAPI.Entities;

namespace HackathonAPI.DTOs.Request
{
    public class ColaboradorRequest
    {
        public string Nome { get; set; }  
        public decimal Salario { get; set; }

        public Colaborador ConverterParaEntidade()
        {
            return new Colaborador(Nome, Salario);
        }
    }
}