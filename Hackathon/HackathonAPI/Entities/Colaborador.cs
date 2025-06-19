namespace HackathonAPI.Entities
{
    public class Colaborador : Entity
    {
        public string Nome { get; private set; }
        public DateTime DataAdmissao { get; private set; }
        public decimal Salario { get; private set; }

        public Colaborador(string nome, DateTime dataAdmissao , decimal salario)
        {
            Nome = nome;
            DataAdmissao = dataAdmissao;
            Salario = salario;
        }

        public void AlterarDados(string nome, decimal salario)
        {
            Nome = nome;
            Salario = salario;
        }
    }
}