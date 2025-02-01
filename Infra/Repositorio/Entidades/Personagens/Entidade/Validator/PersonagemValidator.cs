using FluentValidation;
using Infra.Exceptions.TextosErros;
using Infra.Repositorio.Entidades.Personagens.Entidade;

namespace Infra.Repositorio.Entidades.Personagens.Entidade.Validator
{
    public class PersonagemValidador : AbstractValidator<Personagem>
    {
        public PersonagemValidador()
        {
            RuleFor(x => x.Idade)
                .LessThanOrEqualTo(80)
                .WithMessage(Erros.AgentesComMaisDe80Anos)
                .GreaterThanOrEqualTo(18)
                .WithMessage(Erros.AgenteMenorDeIdade);

            RuleFor(x => x.Nivel)
                .GreaterThanOrEqualTo(0)
                .WithMessage(Erros.NivelDoAgenteNegativo)
                .LessThanOrEqualTo(10)
                .WithMessage(Erros.NivelDoAgenteMaiorQue10);

            RuleFor(x => x.NEX)
                .GreaterThanOrEqualTo(0)
                .WithMessage(Erros.NEXDoAgenteNegativo)
                .LessThanOrEqualTo(99)
                .WithMessage(Erros.NEXDoAgenteMaiorQue99);

        }
    }
}
