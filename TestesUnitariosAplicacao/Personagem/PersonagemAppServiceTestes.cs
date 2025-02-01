using Aplicacao.AppService.Personagem;
using Aplicacao.AppService.Personagem.DTO;
using Bogus;
using FluentAssertions;
using Infra.Exceptions;
using Infra.Exceptions.TextosErros;
using E = Infra.Repositorio.Entidades.Personagens.Entidade;
using Infra.Repositorio.Entidades.Personagens.Entidade.Enums;
using Infra.Repositorio.Entidades.Personagens.Interface;
using Infra.Repositorio.Personagens.Interface;
using Moq;
using Xunit;
using Bogus.DataSets;
using Infra.Repositorio.Entidades.Personagens.Entidade;

namespace TestesUnitariosAppService.Personagem
{
    public class PersonagemAppServiceTestes
    {
        private readonly IPersonagemAppService _appService;
        private readonly Mock<IPersonagemRepositorio> _repositorio = new();
        private readonly Faker _faker;

        public PersonagemAppServiceTestes()
        {
            _appService = new PersonagemAppService(_repositorio.Object);
            _faker = new Faker(locale: "pt_BR");
        }

        [Fact]
        public async Task InserePersonagem_SemProblemas()
        {
            var personagem = CriarRequestPersonagem();

            var response = await _appService.InserirPersonagemAsync(personagem);

            _repositorio.Verify(x => x.InserirPersonagemAsync(It.IsAny<Infra.Repositorio.Entidades.Personagens.Entidade.Personagem>()), Times.Once);
            response.Should().BeAssignableTo<PersonagemResponse>();
            response.NEX.Should().Be(personagem.NEX);
            response.Nome.Should().Be(personagem.Nome);
        }

        [Fact]
        public async Task InserePersonagem_MenorDeIdade()
        {
            var personagem = CriarRequestPersonagem();
            personagem.Idade = _faker.Random.Int(0, 17);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.AgenteMenorDeIdade);
        }

        [Fact]
        public async Task InserePersonagem_IdadeMaiorQue80()
        {
            var personagem = CriarRequestPersonagem();
            personagem.Idade = _faker.Random.Int(81, 100);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.AgentesComMaisDe80Anos);
        }

        [Fact]
        public async Task InserePersonagem_NivelNegativo()
        {
            var personagem = CriarRequestPersonagem();
            personagem.Nivel = _faker.Random.Int(-100, -1);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.NivelDoAgenteNegativo);
        }

        [Fact]
        public async Task InserePersonagem_NivelMaiorQue10()
        {
            var personagem = CriarRequestPersonagem();
            personagem.Nivel = _faker.Random.Int(11);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.NivelDoAgenteMaiorQue10);
        }

        [Fact]
        public async Task InserePersonagem_NEXNegativo()
        {
            var personagem = CriarRequestPersonagem();
            personagem.NEX = _faker.Random.Int(-100, -1);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.NEXDoAgenteNegativo);
        }

        [Fact]
        public async Task InserePersonagem_NEXMaiorQue99()
        {
            var personagem = CriarRequestPersonagem();
            personagem.NEX = _faker.Random.Int(100);

            var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(async () => await _appService.InserirPersonagemAsync(personagem));

            exception._errors.Should().Contain(Erros.NEXDoAgenteMaiorQue99);
        }

        [Fact]
        public async Task EditarPersonagem_SemProblemas()
        {
            var (personagemRequestEditar, personagemAEditar) = CriarRequestEditarEEntidadePersonagem();
            var personagemExistente = CriarPersonagem();
            _repositorio.Setup(x => x.ObterPersonagemPorId(personagemRequestEditar.Id)).ReturnsAsync(personagemExistente);
            _repositorio.Setup(x => x.EditarPersonagemAsync(It.IsAny<E.Personagem>(), personagemExistente)).ReturnsAsync(personagemAEditar);

            var response = await _appService.EditarPersonagemAsync(personagemRequestEditar);

            _repositorio.Verify(x => x.EditarPersonagemAsync(It.IsAny<E.Personagem>(), personagemExistente), Times.Once);
            response.Should().BeAssignableTo<PersonagemResponse>();
        }

        [Fact]
        public async Task ApagarPersonagemAsync_SemProblemas()
        {
            var personagemExistente = CriarPersonagem();
            _repositorio.Setup(x => x.ObterPersonagemPorId(personagemExistente.Id)).ReturnsAsync(personagemExistente);

            var response = await _appService.ApagarPersonagemAsync(personagemExistente.Id);

            _repositorio.Verify(x => x.ApagarPersonagemAsync(personagemExistente), Times.Once);
            response.Should().BeAssignableTo<PersonagemResponse>();
        }

        private PersonagemRequest CriarRequestPersonagem()
        {
            return new PersonagemRequest()
            {
                Classe = _faker.PickRandom<Classe>(),
                Trilha = _faker.PickRandom<Trilha>(),
                Idade = _faker.Random.Int(18, 80),
                NEX = _faker.Random.Int(0, 99),
                Nivel = _faker.Random.Int(0, 10),
                Nome = _faker.Random.String(),
            };
        }

        private (PersonagemEditarRequest, E.Personagem) CriarRequestEditarEEntidadePersonagem()
        {
            PersonagemEditarRequest requestEditar = new()
            {
                Id = _faker.Random.Int(),
                Classe = _faker.PickRandom<Classe>(),
                Trilha = _faker.PickRandom<Trilha>(),
                Idade = _faker.Random.Int(18, 80),
                NEX = _faker.Random.Int(0, 99),
                Nivel = _faker.Random.Int(0, 10),
                Nome = _faker.Random.String(),
            };

            E.Personagem entidade = new(
                requestEditar.Id,
                requestEditar.Nome,
                requestEditar.Idade,
                requestEditar.Classe,
                requestEditar.Trilha,
                requestEditar.NEX,
                requestEditar.Nivel);

            return (requestEditar, entidade);
        }

        private E.Personagem CriarPersonagem()
        {
            return new(
                _faker.Random.Int(),
                _faker.Random.String(),
                _faker.Random.Int(18, 80),
                _faker.PickRandom<Classe>(),
                _faker.PickRandom<Trilha>(),
                _faker.Random.Int(0, 99),
                _faker.Random.Int(0, 10));
        }
    }
}