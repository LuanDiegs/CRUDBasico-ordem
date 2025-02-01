using Aplicacao.AppService.Personagem.DTO;
using Infra.Repositorio.Entidades.Personagens.Entidade;

namespace Infra.Repositorio.Personagens.Interface;

public interface IPersonagemAppService
{
    Task<PersonagemResponse> InserirPersonagemAsync(PersonagemRequest request);

    Task<PersonagemResponse> EditarPersonagemAsync(PersonagemEditarRequest request);

    Task<PersonagemResponse> ApagarPersonagemAsync(int personagemId);

    List<Personagem> GetTodosOsPersonagens();
}
