using Infra.Repositorio.Entidades.Personagens.Entidade;

namespace Infra.Repositorio.Entidades.Personagens.Interface;

public interface IPersonagemRepositorio
{
    Task InserirPersonagemAsync(Personagem personagem);

    Task<Personagem> EditarPersonagemAsync(Personagem personagemAAtualizar, Personagem personagemExistente);

    Task ApagarPersonagemAsync(Personagem personagemId);

    List<Personagem> ObterTodosOsPersonagens();

    Task<Personagem> ObterPersonagemPorId(int id);
}
