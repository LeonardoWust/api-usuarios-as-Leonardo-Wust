# API de Gerenciamento de Usuários

## Descrição
Projeto desenvolvido como avaliação técnica para implementar uma API RESTful utilizando .NET 8. O sistema permite o gerenciamento de usuários (CRUD) com regras de negócio específicas, persistência em banco de dados e documentação de código.

## Tecnologias Utilizadas
- .NET 8.0: Framework principal.
- ASP.NET Core Minimal APIs: Para construção dos endpoints.
- Entity Framework Core: ORM para manipulação do banco de dados.
- SQLite: Banco de dados relacional (arquivo local).
- FluentValidation: Biblioteca para validação rigorosa de dados.
- Clean Architecture: Estrutura de separação de responsabilidades.

## Padrões de Projeto Implementados
- Repository Pattern: Abstração da camada de acesso a dados (`IUsuarioRepository`).
- Service Pattern: Encapsulamento das regras de negócio (`IUsuarioService`).
- DTO Pattern: Objetos de transferência de dados para segurança (`UsuarioCreateDto`, etc).
- Dependency Injection: Injeção de serviços e repositórios no container do .NET.

## Como Executar o Projeto

Para rodar a API na sua máquina, siga os passos abaixo. O sistema foi configurado para criar o banco de dados automaticamente na primeira execução.

### Pré-requisitos
* **.NET SDK 8.0** instalado.
* Git instalado.

### Passo a Passo

1.  **Clone o repositório**
    Abra o terminal e execute:
    ```bash
    git clone https://github.com/LeonardoWust
    ```

2.  **Acesse a pasta do projeto**
    ```bash
    cd api-usuarios-as-LeonardoWust
    cd APIUsuario
    ```

3.  **Execute a aplicação**
    Este comando vai restaurar os pacotes, compilar o código e rodar o servidor:
    ```bash
    dotnet run
    ```

4.  **Acesse a API**
    Assim que aparecer a mensagem `Now listening on...`, a API estará disponível em:
    * `http://localhost:5214` (ou a porta informada no terminal)

---

### Exemplos de Requisições
Para testar, você pode importar a Collection do Postman inclusa no projeto ou usar os exemplos abaixo (JSON):

**POST /usuarios (Criar Usuário)**
```json
{
  "nome": "João da Silva",
  "email": "joao@teste.com",
  "senha": "senha123",
  "dataNascimento": "1990-01-01",
  "telefone": "11999998888"
}

### Pré-requisitos
- .NET SDK 8.0 ou superior instalado.


## Estrutura do Projeto

O projeto segue os princípios da **Clean Architecture**, dividindo as responsabilidades em camadas concêntricas para garantir desacoplamento e testabilidade.

###  APIUsuario (Raiz)
Contém o ponto de entrada da aplicação e configurações globais.
* **Program.cs**: Arquivo principal que configura a Injeção de Dependência, o Banco de Dados e define os Endpoints (Rotas) da API.
* **appsettings.json**: Arquivo de configuração onde fica a *ConnectionString* do banco de dados.

###  Domain (Domínio)
O núcleo do sistema. Esta camada não depende de nenhuma outra.
* **Entities**: Contém a classe `Usuario.cs`, que representa a tabela do banco de dados e as regras de negócio corporativas (ex: propriedades obrigatórias).

###  Application (Aplicação)
Contém a lógica de negócio e os casos de uso. Depende apenas do Domain.
* **DTOs**: *Data Transfer Objects*. Classes usadas para transportar dados entre a API e o usuário (ex: `UsuarioCreateDto` para esconder a senha no retorno).
* **Interfaces**: Contratos que definem *o que* o sistema faz, mas não *como* (ex: `IUsuarioService`, `IUsuarioRepository`). Permite a inversão de dependência.
* **Services**: Implementação da lógica de negócio (ex: validação de idade mínima, verificação de email duplicado).
* **Validators**: Regras de validação de entrada utilizando a biblioteca *FluentValidation*.

###  Infrastructure (Infraestrutura)
Responsável pela comunicação com o mundo externo (Banco de Dados).
* **Persistence**: Contém o `AppDbContext`, que faz a ponte entre o código C# e o banco SQLite.
* **Repositories**: Implementação das interfaces de acesso a dados. É aqui que o *Entity Framework* executa os comandos SQL (INSERT, SELECT, UPDATE).

  

## Autor

Leonrado Gonçalves Wust

Curso: Análise e Desenvolvimento de Sistemas
