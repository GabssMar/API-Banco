# ProjetoDsin

Projeto desenvolvido em .NET 8, utilizando ASP.NET Core, Entity Framework Core e banco de dados SQLite. O objetivo é fornecer uma API para gerenciamento de usuários, veículos, infrações e anexos, com foco em operações típicas de um sistema de controle de multas de trânsito.

## Sumário

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Modelos de Dados](#modelos-de-dados)
- [Como Executar](#como-executar)
- [Exemplo de Uso](#exemplo-de-uso)
- [Dependências](#dependências)
- [Configuração](#configuração)
- [Licença](#licença)

---

## Sobre o Projeto

Este projeto expõe uma API RESTful para cadastro e consulta de usuários, veículos, infrações e anexos de evidências. Ele pode ser utilizado como backend para sistemas de fiscalização, controle de multas ou aplicações similares.

## Funcionalidades

- Cadastro, autenticação e consulta de usuários
- Cadastro e consulta de veículos
- Registro de infrações de trânsito
- Upload e associação de anexos (evidências) às infrações
- Consulta detalhada de infrações e seus relacionamentos

## Modelos de Dados

### Usuário

- `Id`: int
- `Nome`: string
- `Email`: string
- `Senha`: string
- `CodigoAgente`: int
- `CodigoOrg`: int

### Veículo (`DadosVeiculo`)

- `Id`: int
- `Placa`: string
- `Modelo`: string
- `Fabricante`: string
- `Cor`: string
- `Ano`: int
- `IdUsuario`: int

### Infração (`DetalhesInfracao`)

- `Id`: int
- `TipoInfracao`: string
- `CodigoInfracao`: string
- `LocalInfracao`: string
- `Data`: string
- `Hora`: string
- `Gravidade`: string
- `PontosCnh`: int
- `IdDadosVeiculo`: int

### Anexos

- `Id`: int
- `Evidencia`: byte[]
- `Comentarios`: string
- `IdDadosVeiculo`: int

## Como Executar

1. **Pré-requisitos**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - (Opcional) [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

2. **Clone o repositório**:
   ```bash
   git clone https://github.com/seu-usuario/ProjetoDsin.git
   cd ProjetoDsin
   ```

3. **Restaure as dependências**:
   ```bash
   dotnet restore
   ```

4. **Execute as migrações (se necessário)**:
   ```bash
   dotnet ef database update
   ```

5. **Inicie a aplicação**:
   ```bash
   dotnet run
   ```

6. Acesse a documentação Swagger em: `https://localhost:5163/swagger`

## Exemplo de Uso

Exemplo de requisição HTTP para consultar usuários:

```http
GET http://localhost:5163/Usuarios/
Accept: application/json
```

## Dependências

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Sqlite
- Swashbuckle.AspNetCore (Swagger)
- Microsoft.AspNetCore.OpenApi

Veja o arquivo `ProjetoDsin.csproj` para detalhes de versões.

## Configuração

As configurações principais estão nos arquivos `appsettings.json` e `appsettings.Development.json`. O banco de dados SQLite será criado automaticamente como `banco.db` na raiz do projeto.

## Licença

Este projeto é livre para uso acadêmico. Adapte a licença conforme necessário.

---