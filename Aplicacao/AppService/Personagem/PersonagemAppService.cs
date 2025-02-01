using Aplicacao.AppService.Personagem.DTO;
using Infra.Repositorio.Entidades.Personagens.Interface;
using Infra.Repositorio.Personagens.Interface;
using E = Infra.Repositorio.Entidades.Personagens.Entidade;

namespace Aplicacao.AppService.Personagem
{
    public class PersonagemAppService : IPersonagemAppService
    {
        private readonly IPersonagemRepositorio _repositorio;

        public PersonagemAppService(
            IPersonagemRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<PersonagemResponse> InserirPersonagemAsync(PersonagemRequest request)
        {
            var entidade = new E.Personagem(
                request.Nome,
                request.Idade,
                request.Classe,
                request.Trilha,
                request.Nivel,
                request.NEX);

            entidade.Validar();

            await _repositorio.InserirPersonagemAsync(entidade);

            return new PersonagemResponse()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                NEX = entidade.NEX,
            };
        }

        public List<E.Personagem> GetTodosOsPersonagens()
        {
            return _repositorio.ObterTodosOsPersonagens();
        }

        public async Task<PersonagemResponse> EditarPersonagemAsync(PersonagemEditarRequest request)
        {
            var personagemAAtualizar = new E.Personagem(
                request.Id,
                request.Nome,
                request.Idade,
                request.Classe,
                request.Trilha,
                request.Nivel,
                request.NEX);

            personagemAAtualizar.Validar();

            var personagemExistente = await _repositorio.ObterPersonagemPorId(personagemAAtualizar.Id);
            var personagemAtualizado = await _repositorio.EditarPersonagemAsync(personagemAAtualizar, personagemExistente);

            return new PersonagemResponse()
            {
                Id = personagemAtualizado.Id,
                Nome = personagemAtualizado.Nome,
                NEX = personagemAtualizado.NEX,
            };
        }

        public async Task<PersonagemResponse> ApagarPersonagemAsync(int personagemId)
        {
            var personagemExistente = await _repositorio.ObterPersonagemPorId(personagemId);

            await _repositorio.ApagarPersonagemAsync(personagemExistente);

            return new PersonagemResponse()
            {
                Id = personagemExistente.Id,
                Nome = personagemExistente.Nome,
                NEX = personagemExistente.NEX,
            };
        }
    }
}
