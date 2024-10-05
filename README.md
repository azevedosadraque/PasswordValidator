## Descrição

Esta API expõe um endpoint para validar se uma senha é válida seguindo os critérios fornecidos:

- Deve conter ao menos 9 caracteres
- Deve conter ao menos 1 dígito, 1 letra minúscula, 1 letra maiúscula e 1 caractere especial
- Não deve possuir caracteres repetidos

### Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas em sua máquina:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download) ou mais recente
- Um editor de código, como [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- PowerShell para rodar o script de cobertura de testes (`generate_coverage_report.ps1`), se estiver em um ambiente Windows.

### Como Rodar

1. Clone o repositório.
2. Restaure as dependências com `dotnet restore`.
3. Rode a aplicação com `dotnet run`.
4. Acesse a API em `http://localhost:5000/api/password/validate`.

### Exemplos de Uso

Aqui estão alguns exemplos de requisições para validar uma senha:

#### Requisição via cURL:

```bash
curl -X POST "http://localhost:5000/api/password/validate" -H "Content-Type: application/json" -d "{\"password\":\"AbTp9!fok\"}"
```

#### Swagger

Para acessar o Swagger no ambiente de desenvolvimento, basta executar a aplicação, e a página será redirecionada automaticamente

### Testes

Para rodar os testes:

```bash
dotnet test
```
#### Testes Unitários com Cobertura de Código
Para rodar os testes com cobertura de código, execute o script generate_coverage_report.ps1 dentro da pasta .\PasswordValidator\UnitTests

#### Testes de Integração com Cobertura de Código
Para rodar os testes com cobertura de código, execute o script generate_coverage_report.ps1 dentro da pasta .\PasswordValidator\IntegrationTests 

#### Testes de Aceitação com Cobertura de Código
Para rodar os testes com cobertura de código, execute o script generate_coverage_report.ps1 dentro da pasta .\PasswordValidator\AcceptanceTests

## Relatório de Tomada de Decisão de Desenvolvimento

### Criação e Estruturação Inicial (Commit 1 ao Commit 2)

- Criei o repositório para o projeto e, em seguida, estruturei a solução seguindo os padrões de **Domain-Driven Design (DDD)**. Essa organização ajuda a separar claramente as camadas de **Domain**, **Application**, **Infrastructure** e **Presentation**, facilitando a manutenibilidade e escalabilidade.

### Desenvolvimento e Implementação das Regras de Validação (Commit 3 ao Commit 6)

- Adicionei o pacote **xUnit** para garantir uma cobertura de testes robusta.
  
- Iniciei a implementação das regras de validação diretamente na entidade `Password`, encapsulando o comportamento de validação no método `IsValid()`.

- Após implementar as regras de validação na entidade, decidi seguir o princípio de responsabilidade única (SOLID) e movi essas regras para uma classe dedicada `PasswordValidator`, desacoplando a lógica da entidade. Isso facilita tanto os testes quanto a manutenção do código, além de garantir maior flexibilidade para possíveis alterações futuras.

- Reorganizei a localização do `PasswordValidator`, movendo-o do namespace de **interface** para o namespace de **service**, refletindo melhor sua responsabilidade no projeto.

### Refatorações Baseadas em Princípios SOLID (Commit 7 ao Commit 11)

- Criei a `PasswordController`, o `ValidatePasswordRequest`, e o `ValidatePasswordRequestHandler` como esqueleto, preparando a camada de apresentação para as validações de senha, mas sem implementar as funcionalidades nesse momento.

- Removi o método `IsValid()` da entidade `Password`, uma vez que ele se tornou redundante após a introdução da classe `PasswordValidator` com responsabilidade única pelas validações.

- Refatorei o `ValidatePasswordRequestHandler`, adicionando logs para melhor rastreabilidade das validações e modificando a lógica para fornecer feedback mais detalhado sobre as falhas de validação. A decisão de incluir mensagens detalhadas foi feita para melhorar a experiência do usuário e facilitar a identificação de problemas com a senha.

- Corrigi uma mensagem de erro incompleta no `PasswordValidator` para garantir que o feedback fosse claro e preciso.

- Removi um bloco de `try-catch`, já que, neste cenário, não há integrações com banco de dados ou outras dependências que justifiquem o tratamento de exceções.

### Configurações de MediatR e Dependency Injection (Commit 12 ao Commit 15)

- Configurei o **MediatR** e implementei **Dependency Injection**. A escolha do **MediatR** foi feita para desacoplar a camada de aplicação da controller, permitindo uma melhor separação de responsabilidades e facilitando testes e manutenção futura.

- Integrei o **MediatR** na camada de aplicação, gerenciando a comunicação entre a camada de apresentação e as regras de negócio de forma desacoplada.

- Configurei a injeção de dependências na camada de domínio, marcando os serviços como **transient** devido ao baixo custo computacional e à ausência de necessidade de persistência de estado entre requisições.

- Adicionei a configuração de injeção de dependências no arquivo `Program.cs`, garantindo que todas as dependências fossem resolvidas corretamente.

### Ajustes e Melhorias de Feedback (Commit 16 ao Commit 19)

- Configurei o **Swagger UI** para documentar a API, facilitando o teste e a compreensão da API por outros desenvolvedores ou stakeholders.

- Refatorei a classe `Program.cs`, organizando o código para melhorar a legibilidade e a estrutura, facilitando futuras manutenções.

- Adicionei o **HealthCheck** da API, permitindo monitorar a saúde da aplicação de maneira contínua.

- Corrigi a validação de caracteres repetidos, ajustando o código para que ele tratasse letras maiúsculas e minúsculas como o mesmo caractere, conforme as regras de validação.

### Feedback Mais Detalhado e Melhor Organização (Commit 20 ao Commit 23)

- Criei um **Value Object** para encapsular as informações de senha e expô-las corretamente na camada de aplicação, em vez de utilizar diretamente um **DTO** na camada de domínio, corrigindo uma violação de estrutura.

- Encapsulei o parâmetro de entrada em um **DTO**, permitindo flexibilidade para possíveis extensões ou mudanças nas informações necessárias para validação sem impactar outras partes da aplicação.

- Decidi fornecer um retorno mais detalhado da API, com mensagens descritivas sobre falhas de validação (por exemplo, falta de uma letra maiúscula). Implementei uma estrutura de **ApiResponse** para padronizar as respostas da API e centralizar a validação do `ModelState`. Isso visa oferecer uma melhor experiência ao usuário, permitindo que ele saiba exatamente onde a senha falhou.

- Removi a entidade `Password`, pois com a refatoração para usar o `PasswordValidator`, a entidade não era mais necessária.

### Testes e Cobertura (Commit 24 ao Commit 28)

- Adicionei testes para a `PasswordController` e criei um script para executar a cobertura dos testes com **Coverlet**. Além disso, separei os métodos de **ApiResponse** em dois — um para sucesso e outro para erros — evitando ambiguidades nos parâmetros genéricos.

- Implementei testes para o `ValidatePasswordRequestHandler`, garantindo que as regras de validação fossem testadas de forma isolada e correta.

- Removi uma validação redundante de nulo, simplificando a lógica e melhorando a eficiência.

- Corrigi um problema de namespace configurado incorretamente na `Program.cs`, garantindo que a estrutura do projeto seguisse os padrões estabelecidos.

- Adicionei a [ProducesResponseType(StatusCodes.Status500InternalServerError)] annotation no endpoint.

### Configuração do ReadMe (Commit 29 ao Commit 32)

- Configurei o arquivo `README.md` para incluir informações relevantes sobre o projeto, como:

  - **Instruções de Execução**: Descrevi os passos necessários para executar a aplicação localmente, incluindo os pré-requisitos, instalação de dependências e comandos para rodar o projeto, facilitando a reprodução por outros desenvolvedores.
  
  - **Documentação da API**: Adicionei uma seção detalhando o endpoint exposto pela API, explicando como utilizá-lo. Também incluí links para o **Swagger UI** para que a API possa ser testada diretamente por meio da interface gráfica.

  - **Relatórios de Testes e Cobertura**: Incluí instruções sobre como rodar os testes unitários com o **xUnit** e gerar relatórios de cobertura usando o **Coverlet**, além de mostrar como acessar esses relatórios para verificar a qualidade do código.

### Testes de Integração e Aceitação (Commit 33 ao Commit 36)

- Implementei testes de integração para validar a funcionalidade de ponta a ponta da API.
- Criei um script para gerar relatórios de cobertura no projeto de integração.
- Adicionei testes de aceitação para garantir que o comportamento do sistema correspondesse aos requisitos de negócio, incluindo cenários do mundo real.
- Renomeei os projetos de IntegrationTest e AcceptanceTest para **IntegrationTests** e **AcceptanceTests**.

### Atualização do README (Commit 37 em diante)

- Atualizei o `README.md` com informações adicionais após a conclusão dos testes de integração e aceitação.

### Conclusão

Durante o desenvolvimento da API de validação de senhas, segui os princípios de **SOLID**, **Clean Code** e **DDD**, priorizando a flexibilidade e manutenibilidade. A implementação do **MediatR** e a separação das responsabilidades através de interfaces e classes dedicadas tornaram o código mais testável e preparado para futuras manutenções. Além disso, a decisão de fornecer feedbacks mais detalhados sobre falhas de validação melhora significativamente a experiência do usuário final. A API foi desenvolvida de maneira robusta e está pronta para eventuais evoluções e ajustes futuros.

