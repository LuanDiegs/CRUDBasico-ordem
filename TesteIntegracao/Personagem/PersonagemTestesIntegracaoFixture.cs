using Aplicacao.AppService.Personagem.DTO;
using Bogus;
using Infra.Repositorio;
using Infra.Repositorio.Entidades.Personagens.Entidade.Enums;
using Microsoft.Extensions.DependencyInjection;
using E = Infra.Repositorio.Entidades.Personagens.Entidade;

namespace TesteIntegracao.Personagem
{
    public class PersonagemTestesIntegracaoFixture
    {
        private readonly Faker _faker = new (locale: "pt_BR");
        public const int QuantidadeDadosCriados = 10;

        public PersonagemTestesIntegracaoFixture(TesteIntegracaoFactory factory)
        {
            Banco = ObterContexto(factory);

            var personagens = GerarPersonagens(QuantidadeDadosCriados);
            Banco.AddRange(personagens);
            Banco.SaveChanges();
        }

        public PersonagemRequest GerarRequestBase()
        {
            return new PersonagemRequest()
            {
                Nome = _faker.Random.String2(50),
                Classe = _faker.PickRandom<Classe>(),
                Trilha = _faker.PickRandom<Trilha>(),
                Idade = _faker.Random.Int(18, 80),
                Nivel = _faker.Random.Int(0, 10),
                NEX = _faker.Random.Int(0, 99),
            };
        }

        public PersonagemEditarRequest GerarRequestEditarBase()
        {
            var personagemAleatorio = _faker.PickRandom(Banco.Personagens).FirstOrDefault();

            return new PersonagemEditarRequest()
            {
                Id = personagemAleatorio.Id,
                Nome = _faker.Random.String2(50),
                Classe = _faker.PickRandom<Classe>(),
                Trilha = _faker.PickRandom<Trilha>(),
                Idade = _faker.Random.Int(18, 80),
                Nivel = _faker.Random.Int(0, 10),
                NEX = _faker.Random.Int(0, 99),
            };
        }

        public List<E.Personagem> GerarPersonagens(int quantidade)
        {
            var personagens = new Faker<E.Personagem>(locale: "pt_BR")
                .RuleFor(x => x.Nome, y => y.Random.String2(50))
                .RuleFor(x => x.Classe, y => y.PickRandom<Classe>())
                .RuleFor(x => x.Trilha, y => y.PickRandom<Trilha>())
                .RuleFor(x => x.Idade, y => y.Random.Int(18, 80))
                .RuleFor(x => x.Nivel, y => y.Random.Int(0, 10))
                .RuleFor(x => x.NEX, y => y.Random.Int(0, 99))
                .GenerateBetween(quantidade, quantidade);

            return personagens;
        }

        protected OrdemDbContext ObterContexto(TesteIntegracaoFactory factory)
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var contexto = scope.ServiceProvider.GetService<OrdemDbContext>();

            return contexto;
        }

        public OrdemDbContext Banco { get; private set; }
    }
}