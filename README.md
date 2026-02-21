# Product Manager API

API REST para gerenciamento de produtos, desenvolvida em .NET 8 com PostgreSQL.

## Sobre o projeto

Esse projeto é uma API para cadastrar, editar, excluir e listar produtos com filtros. Também tem upload de imagem simulando um envio para a AWS S3 (salvo localmente).

Foi desenvolvido seguindo arquitetura em camadas, boas práticas de código e princípios SOLID.

## Tecnologias utilizadas

- .NET 8
- Entity Framework Core
- PostgreSQL
- Docker
- Swagger (documentação da API)
- xUnit e Moq (testes unitários)
- GitHub Actions (CI/CD)

## Arquitetura

O projeto está dividido em camadas:

- **ProductManager.API** — Controllers e configuração do Swagger
- **ProductManager.Application** — DTOs, Services e regras de negócio
- **ProductManager.Domain** — Entidades, Enums e interfaces
- **ProductManager.Infrastructure** — Acesso ao banco de dados com Entity Framework

A ideia dessa separação é que cada camada tenha sua responsabilidade. O Domain não conhece o banco de dados, e a API não acessa o banco diretamente. Tudo passa pelo Service e pelo Repository.

## Como rodar o projeto

Você precisa ter o Docker instalado.

1. Clone o repositório:
```bash
git clone https://github.com/Fhillipe1/ProductManager.git
cd ProductManager
```

2. Suba os containers:
```bash
docker compose up --build -d
```

3. Acesse o Swagger: http://localhost:5111/swagger

Para parar:
```bash
docker compose down
```

## Endpoints

- `POST /api/products` — Cadastrar produto
- `GET /api/products` — Listar produtos (aceita filtros por query string)
- `GET /api/products/{id}` — Buscar produto por ID
- `PUT /api/products/{id}` — Editar produto
- `DELETE /api/products/{id}` — Excluir produto
- `POST /api/products/{id}/image` — Upload de imagem

### Filtros na listagem

Exemplo: `/api/products?category=Periféricos&minPrice=50&maxPrice=300&status=1`

- `category` — filtrar por categoria
- `minPrice` e `maxPrice` — faixa de preço
- `status` — 1 para Ativo, 2 para Inativo

## Testes

Para rodar os testes:
```bash
dotnet test
```

Foram criados 8 testes unitários cobrindo os cenários do ProductService: criação, busca, atualização e exclusão de produtos, incluindo cenários de sucesso e de erro (produto não encontrado).

## CI/CD

O projeto tem um pipeline com GitHub Actions que roda automaticamente a cada push na main. Ele compila o projeto e roda os testes.

## O que eu aplicaria com mais tempo

- Validação dos DTOs com FluentValidation
- Paginação na listagem de produtos
- Autenticação com JWT
- Upload real para AWS S3
- Testes de integração
