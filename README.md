# Mini Ecommerce

API REST desenvolvida como projeto de portfólio para demonstrar conhecimentos em desenvolvimento backend com ASP.NET Core 8.

O projeto simula o backend de uma loja online — cobrindo cadastro e autenticação de usuários, catálogo de produtos, gerenciamento de endereços, criação de pedidos e processamento de pagamentos com controle de estoque.

---

## Decisões técnicas

**Arquitetura em camadas**
O projeto é organizado em Controllers, Services e Repositories, separando responsabilidades e facilitando manutenção e testes. Os serviços não conhecem detalhes de infraestrutura — dependem apenas de interfaces, o que permite trocar implementações sem afetar o restante do sistema.

**Autenticação e autorização**
Autenticação implementada com ASP.NET Core Identity, gerando tokens JWT com claims de perfil (cliente e administrador). O controle de acesso é feito via `[Authorize(Roles = "...")]` nos endpoints, garantindo que operações administrativas — como gerenciar estoque — não fiquem expostas para usuários comuns.

**Testes unitários**
Testes implementados para `AuthService` e `PaymentService` com xUnit, Moq e FluentAssertions. O banco de dados é substituído por um provider InMemory do EF Core nos testes, isolando a lógica de negócio da infraestrutura. A escolha por testar apenas esses dois serviços foi intencional — são os fluxos de maior risco da aplicação.

**Docker**
A aplicação e o banco de dados rodam em containers orquestrados via Docker Compose. As migrations e a criação do banco são executadas automaticamente no startup, eliminando qualquer configuração manual de infraestrutura.

**Integração externa**
Produtos e categorias são importados da [DummyJSON API](https://dummyjson.com/) na inicialização, simulando um cenário real de integração com fornecedores externos.

---

## Stack

| | |
|---|---|
| ASP.NET Core 8 | Framework principal |
| Entity Framework Core + SQL Server | Persistência de dados |
| ASP.NET Core Identity + JWT | Autenticação e autorização |
| Docker + Docker Compose | Containerização |
| xUnit + Moq + FluentAssertions | Testes unitários |

---

## Como rodar

### Pré-requisitos
- Docker

### 1. Clone o repositório

```bash
git clone https://github.com/jessicavieiradev/miniEcommerceApi
cd miniEcommerceApi
```

### 2. Crie o arquivo `.env`

Na raiz do projeto:

```env
BD_PASSWORD=        # senha do banco SQL Server
JWT_SECRET=         # chave secreta para geração dos tokens JWT
ADMIN_EMAIL=        # e-mail do usuário administrador criado na primeira execução
ADMIN_USERNAME=     # username do administrador
ADMIN_PASSWORD=     # senha do administrador
```

### 3. Configure os secrets da aplicação

Na pasta `miniEcommerceApi`:

```bash
dotnet user-secrets init
```

```json
{
  "Jwt": {
    "SecretKey": "<mesma chave definida em JWT_SECRET>"
  },
  "AdminSeed": [
    {
      "Email": "<mesmo valor de ADMIN_EMAIL>",
      "UserName": "<mesmo valor de ADMIN_USERNAME>",
      "Password": "<mesmo valor de ADMIN_PASSWORD>"
    }
  ],
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=miniecommercebd;User Id=sa;Password=<mesmo valor de BD_PASSWORD>;TrustServerCertificate=True;"
  }
}
```

### 4. Suba os containers

```bash
docker compose up --build
```

O banco é criado, as migrations são aplicadas e o usuário admin é gerado automaticamente.

### 5. Acesse a documentação

```
http://localhost:5000/swagger
```
