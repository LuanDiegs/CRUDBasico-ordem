using Infra.Exceptions;
using Infra.Exceptions.TextosErros;
using Infra.Repositorio.Entidades.Personagens.Entidade;
using Infra.Repositorio.Entidades.Personagens.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio.Entidades.Personagens
{
    public class PersonagemRepositorio(OrdemDbContext context) : IPersonagemRepositorio
    {
        private readonly OrdemDbContext _dbContext = context;

        public async Task InserirPersonagemAsync(Personagem personagem)
        {
            _dbContext.Personagens.AddRange(personagem);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Personagem> EditarPersonagemAsync(Personagem personagemAAtualizar, Personagem personagemExistente)
        {
            personagemExistente.Editar(personagemAAtualizar);

            await _dbContext.SaveChangesAsync();

            return personagemAAtualizar;
        }

        public async Task ApagarPersonagemAsync(Personagem personagemAApagar)
        {
            _dbContext.Personagens.Remove(personagemAApagar);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Personagem> ObterPersonagemPorId(int id)
        {
            var personagem = await _dbContext.Personagens.FirstOrDefaultAsync(x => x.Id == id);
            if (personagem == null)
            {
                throw new BadRequestException([Erros.PersonagemNaoEncontrado]);
            }

            return personagem;
        }

        public List<Personagem> ObterTodosOsPersonagens()
        {
            return [.. _dbContext.Personagens];
        }
    }
}
