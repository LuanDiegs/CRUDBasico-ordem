using Bogus;

namespace TesteIntegracao.Personagem
{
    public class TestesIntegracaoBase : IClassFixture<TesteIntegracaoFactory>
    {
        public readonly TesteIntegracaoFactory _factory;
        public readonly HttpClient _client;
        public readonly Faker _faker = new();

        public TestesIntegracaoBase(TesteIntegracaoFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}