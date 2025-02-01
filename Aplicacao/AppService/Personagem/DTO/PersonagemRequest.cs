using Infra.Repositorio.Entidades.Personagens.Entidade.Enums;

namespace Aplicacao.AppService.Personagem.DTO;

public class PersonagemRequest
{
    public string Nome { get; set; }

    public int Idade { get; set; }

    public Classe Classe { get; set; }

    public Trilha Trilha { get; set; }

    public int Nivel { get; set; }

    public int NEX { get; set; }
}
