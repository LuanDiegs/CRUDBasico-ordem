using Infra.Exceptions;
using Infra.Repositorio.Entidades.Personagens.Entidade.Enums;
using Infra.Repositorio.Entidades.Personagens.Entidade.Validator;

namespace Infra.Repositorio.Entidades.Personagens.Entidade
{
    public class Personagem
    {
        [Obsolete("Obsoleto", true)]
        public Personagem()
        {
        }

        public Personagem(
            string nome,
            int idade,
            Classe classe,
            Trilha trilha,
            int nivel,
            int nex)
        {
            Nome = nome;
            Idade = idade;
            Classe = classe;
            Trilha = trilha;
            Nivel = nivel;
            NEX = nex;
        }

        public Personagem(
            int id,
            string nome,
            int idade,
            Classe classe,
            Trilha trilha,
            int nivel,
            int nex)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
            Classe = classe;
            Trilha = trilha;
            Nivel = nivel;
            NEX = nex;
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public int Idade { get; private set; }

        public Classe Classe { get; private set; }

        public Trilha Trilha { get; private set; }

        public int Nivel { get; private set; }

        public int NEX { get; private set; }

        public void Validar()
        {
            var validator = new PersonagemValidador();
            var result = validator.Validate(this);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

                throw new BadRequestException(errorMessages);
            }
        }

        public void Editar(Personagem personagem)
        {
            Nome = personagem.Nome;
            Idade = personagem.Idade;
            Classe = personagem.Classe;
            Trilha = personagem.Trilha;
            NEX = personagem.NEX;
            Nivel = personagem.Nivel;
        }
    }
}
