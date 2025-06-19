using System.Text.Json.Serialization;

namespace HackathonAPI.Entities
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }

        [JsonIgnore]
        public ICollection<Livro> Livros { get; private set; }

        public Categoria(string nome)
        {
            Nome = nome;
        }

        public Categoria(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}