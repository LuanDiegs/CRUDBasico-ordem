🧙‍♂️ <b>Projeto: CRUD com Temática de Ordem Paranormal<b/> 🧙‍♀️<br>
Este projeto é um CRUD (Create, Read, Update, Delete) desenvolvido com uma temática inspirada no universo de Ordem Paranormal. O sistema utiliza um banco de dados MySQL e é estruturado com base em conceitos da arquitetura DDD (Domain-Driven Design), garantindo uma organização clara e eficiente do código.

🛠️ Tecnologias e Conceitos Utilizados
- Arquitetura DDD: O projeto é dividido em camadas bem definidas:

  - Repositório: Responsável por manipular os dados no banco de dados.

  - Aplicação: Realiza o tratamento e a manipulação dos dados antes de enviá-los para a camada de repositório.

  - Controllers: Gerencia as requisições e respostas da API.

- Exceptions Personalizadas: Foram criadas exceções customizadas para fornecer mensagens claras e amigáveis ao usuário em caso de erros.

- Validações com FluentValidation: Utiliza-se o FluentValidation para validar as entidades, evitando a repetição de código e garantindo que as entidades estejam sempre válidas antes de serem processadas.

- Testes Unitários: Foram implementados testes unitários para garantir o bom funcionamento das entidades e suas validações.

- Testes de Integração com TestContainers: Para garantir uma testagem mais próxima do ambiente real, utiliza-se TestContainers para criar um banco de dados real em um container Docker durante a execução dos testes de integração.
