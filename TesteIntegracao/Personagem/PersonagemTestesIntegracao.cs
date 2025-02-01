using Aplicacao.AppService.Personagem.DTO;
using Aplicacao.Comum.Exception.DTO;
using FluentAssertions;
using Infra.Exceptions.TextosErros;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using E = Infra.Repositorio.Entidades.Personagens.Entidade;

namespace TesteIntegracao.Personagem
{
    public class PersonagemTestesIntegracao(TesteIntegracaoFactory factory) : TestesIntegracaoBase(factory)
    {
        private const string EndpointEntidade = "/api/personagem";
        private readonly PersonagemTestesIntegracaoFixture _fixture = new (factory);

        [Fact]
        public async Task InserirPersonagem_InsereSemProblema()
        {
            var personagemAInserir = _fixture.GerarRequestBase();
            var quantidadeRegistrosExistentes = await ObterQuantidadeDeRegistrosExistentes();

            var response = await _client.PostAsJsonAsync(EndpointEntidade, personagemAInserir);
            var personagemInserido = await response.Content.ReadFromJsonAsync<PersonagemResponse>();

            _fixture.Banco.ChangeTracker.Clear();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            personagemInserido.Should().NotBeNull();
            personagemInserido.Nome.Should().Be(personagemAInserir.Nome);
            _fixture.Banco.Personagens.Count().Should().Be(quantidadeRegistrosExistentes + 1);
        }

        [Fact]
        public async Task EditarPersonagem_EditaSemProblema()
        {
            var personagemAEditar = _fixture.GerarRequestEditarBase();

            var response = await _client.PatchAsJsonAsync(EndpointEntidade, personagemAEditar);
            var personagemAlterado = await response.Content.ReadFromJsonAsync<PersonagemResponse>();

            _fixture.Banco.ChangeTracker.Clear();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            personagemAlterado.Should().NotBeNull();
            personagemAlterado.Nome.Should().Be(personagemAEditar.Nome);
            _fixture.Banco.Personagens.FirstOrDefault(x => x.Id == personagemAEditar.Id)?.Nome.Should().Be(personagemAEditar.Nome);
        }

        [Fact]
        public async Task EditarPersonagem_PersonagemInexistente_NaoEdita()
        {
            var personagemAEditar = _fixture.GerarRequestEditarBase();
            personagemAEditar.Id = 0;

            var response = await _client.PatchAsJsonAsync(EndpointEntidade, personagemAEditar);
            var responseErro = await response.Content.ReadFromJsonAsync<ResponseErros>();

            _fixture.Banco.ChangeTracker.Clear();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseErro.Erros.Should().NotBeEmpty();
            responseErro.Erros.Should().HaveCount(1);
            responseErro.Erros.Should().Contain(Erros.PersonagemNaoEncontrado);
        }

        [Fact]
        public async Task ApagarPersonagem_ApagaSemProblemas()
        {
            var personagemAApagar = _faker.PickRandom(_fixture.Banco.Personagens).FirstOrDefault();
            var quantidadeRegistrosExistentes = await ObterQuantidadeDeRegistrosExistentes();

            var response = await _client.DeleteAsync(EndpointEntidade + $"?personagemId={personagemAApagar.Id}");
            var personagemApagado = await response.Content.ReadFromJsonAsync<PersonagemResponse>();

            _fixture.Banco.ChangeTracker.Clear();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            personagemApagado.Should().NotBeNull();
            personagemApagado.Nome.Should().Be(personagemAApagar.Nome);
            _fixture.Banco.Personagens.Count().Should().Be(quantidadeRegistrosExistentes - 1);
        }

        [Fact]
        public async Task ApagarPersonagem_PersonagemInexistente_NaoApaga()
        {
            var idInvalido = 0;
            var quantidadeRegistrosExistentes = await ObterQuantidadeDeRegistrosExistentes();

            var response = await _client.DeleteAsync(EndpointEntidade + $"?personagemId={idInvalido}");
            var responseErro = await response.Content.ReadFromJsonAsync<ResponseErros>();

            _fixture.Banco.ChangeTracker.Clear();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseErro.Erros.Should().NotBeEmpty();
            responseErro.Erros.Should().HaveCount(1);
            responseErro.Erros.Should().Contain(Erros.PersonagemNaoEncontrado);
            _fixture.Banco.Personagens.Count().Should().Be(quantidadeRegistrosExistentes);
        }

        [Fact]
        public async Task GetPersonagens_RetornaTodos()
        {
            var response = await _client.GetAsync(EndpointEntidade);
            var personagens = await response.Content.ReadFromJsonAsync<List<E.Personagem>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            personagens.Should().NotBeNull();
            personagens.Should().HaveCount(_fixture.Banco.Personagens.Count());
        }

        private async Task<int> ObterQuantidadeDeRegistrosExistentes()
        {
            return _fixture.Banco.Personagens.Count();
        }
    }
}