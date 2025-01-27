# Projeto Full Stack - Catálogo de Produtos

## **Descrição do Projeto**
Este projeto é uma aplicação full stack para gerenciamento de um catálogo de produtos. Ele é composto por um backend em .NET e um frontend em React. O backend implementa um CRUD completo para a entidade Produto, além de endpoints de consulta para Fornecedor e Categoria. O frontend oferece uma interface interativa para interagir com o backend.

---

## **Configuração e Execução do Projeto**

### **Requisitos Pré-Instalação**
Certifique-se de ter as seguintes versões instaladas em seu ambiente:
- **.NET SDK**: 8.0.405
- **Node.js**: v22.5.1
- **Docker**: 27.4.0
- **MySQL**: 8.0.36

### **Configuração do Backend**
1. Navegue até o diretório do backend:
   ```bash
   cd diretório_raiz_do_projeto_completo/APICatalogo/APICatalogo
   ```

2. Atualize o banco de dados:
   ```bash
   dotnet ef database update
   ```

3. Execute o backend usando Docker Compose:
   ```bash
   docker-compose up --build
   ```

4. Para desenvolvimento local, utilize:
   ```bash
   dotnet watch
   ```

### **Configuração do Frontend**
1. Navegue até o diretório do frontend:
   ```bash
   cd diretório_raiz_do_projeto_completo/frontend
   ```

2. Instale as dependências:
   ```bash
   npm install
   ```

3. Execute o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```

---

## **Tecnologias Utilizadas**

### **Backend**
- **Linguagem**: C#
- **Framework**: ASP.NET Core 8.0
- **Banco de Dados**: MySQL
- **ORM**: Entity Framework Core
- **Cache Distribuído**: Redis (imagem Docker `redis:7.0`)
- **Bibliotecas**:
  - AutoMapper
  - BCrypt.Net-Next
  - Microsoft.AspNetCore.Authentication.JwtBearer
  - Pomelo.EntityFrameworkCore.MySql
  - Swashbuckle (OpenAPI/Swagger)

### **Frontend**
- **Linguagem**: JavaScript/TypeScript
- **Framework**: React 18
- **Build Tool**: Vite
- **Bibliotecas**:
  - Material-UI
  - Axios
  - Ag-Grid
  - TailwindCSS

---

## **Decisões de Design**

### **Backend**
- **Padrão Repository**:
  Utilizado para todas as entidades, garantindo modularidade e desacoplamento entre as camadas de acesso a dados e lógica de negócios.

- **Padrão DTO**:
  Implementado apenas para a entidade Produto, pois foi a única com CRUD completo.

- **Seed Data**:
  As entidades Fornecedor e Categoria são populadas diretamente no banco de dados via seed, utilizando o comando:
  ```bash
  dotnet ef database update
  ```

- **Injeção de Dependência**:
  Utilizada para facilitar a modularidade, o desacoplamento e a implementação de testes unitários.

- **Cache Distribuído**:
  Adicionado para melhorar a performance, com o uso de Redis.

### **Frontend**
- **Componentização**:
  Cada parte da interface foi dividida em componentes reutilizáveis para facilitar a manutenção e a escalabilidade do projeto.

---

## **Testes Unitários**

### **Backend**
Para executar os testes unitários do backend, navegue até o diretório do backend e execute:
```bash
cd diretório_raiz_do_projeto_completo/APICatalogo/APICatalogo

dotnet test
```

### **Frontend**
Para executar os testes unitários do frontend, navegue até o diretório do frontend e execute:
```bash
cd diretório_raiz_do_projeto_completo/frontend

npm run test
```

---

## **Dependências**

### **Backend**
As dependências do backend estão listadas no arquivo `APICatalogo.csproj`.

### **Frontend**
As dependências do frontend estão listadas no arquivo `package.json`.

---

Caso tenha dúvidas ou problemas durante a configuração ou execução do projeto, consulte a documentação oficial das tecnologias utilizadas ou entre em contato com Hugo de Souza Caramez.

