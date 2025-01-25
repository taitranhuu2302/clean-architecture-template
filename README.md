```markdown
# 🚀 .NET Clean Architecture Template  
**Author**: Tai Tran - Software Engineer  

A production-ready .NET 9 Clean Architecture template featuring CQRS, DDD, PostgreSQL, and Dapper. Kickstart your projects with a clean, maintainable foundation optimized for modern API development.

![Clean Architecture Diagram](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

## 🌟 Key Features
- **Clean Architecture** - Strict layer separation: Domain → Application → Infrastructure → Presentation
- **CQRS/MediatR** - Clear command/query separation with pipeline behaviors
- **Dual ORM Support** - EF Core for ORM + Dapper for raw SQL power
- **Smart Swagger** - Auto-generated API docs with custom metadata processing
- **Testing Suite** - Unit, Integration & E2E testing frameworks
- **CI/CD Ready** - Pre-configured GitHub Actions workflow

## 🛠 Technology Stack
| Layer            | Technologies                          |
|------------------|---------------------------------------|
| **Domain**       | .NET 9, DDD                          |
| **Application**  | MediatR, AutoMapper, FluentValidation |
| **Infrastructure**| EF Core 9, Dapper, PostgreSQL        |
| **Presentation** | ASP.NET Core WebAPI, Swagger/OpenAPI/NSwag  |
| **Testing**      | xUnit, Moq           |

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK
- PostgreSQL 15+
- Docker (for TestContainers integration)

### Installation
1. Clone repository:
```bash
git clone https://github.com/taitranhuu2302/clean-architecture-template.git
cd clean-architecture-template
```

2. Configure database:
```bash
dotnet user-secrets set "ConnectionStrings:Postgres" "Host=localhost;Database=mydb;Username=postgres;Password=yourpassword" --project src/WebAPI
```

3. Run migrations:
```bash
dotnet ef database update --project src/Infrastructure
```

4. Launch application:
```bash
dotnet run --project src/API
```

Access Swagger UI: http://localhost:5000/swagger

## 🏗 Solution Structure
```
src/
├── Application/     # Business logic layer
│   ├── Features/    # CQRS implementation
│   ├── Common/      # Shared components
│   └── Mapping/     # DTO transformations
├── Domain/          # Core domain models
├── Infrastructure/  # Data & external services
└── API/             # API endpoints

tests/
├── Domain.Tests/            # Domain tests
├── Application.Tests/       # Infrastructure tests
└── ...                      # other tests
```

## 📖 Code Examples

### CQRS Command
```csharp
// Application/Features/Products/CreateProduct.cs
public record CreateProductCommand(string Name, decimal Price) : IRequest<Guid>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product(request.Name, request.Price);
        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);
        return product.Id;
    }
}
```

### Dapper Query
```csharp
// Infrastructure/Persistence/DapperContext.cs
public async Task<List<Product>> GetExpensiveProducts(decimal minPrice)
{
    const string sql = "SELECT * FROM Products WHERE Price > @MinPrice";
    return (await _connection.QueryAsync<Product>(sql, new { MinPrice = minPrice })).ToList();
}
```

## 🧪 Testing
```bash
# Unit tests (fast)
dotnet test tests
```

## 📄 License
MIT License - See [LICENSE](LICENSE) for details

---

**Let's Build Something Amazing!** 🚀  
Contact: [Tai Tran](mailto:tai.tranhuu2002@gmail.com) | [GitHub Profile](https://github.com/taitranhuu2302)
```

This README features:
- Clear visual hierarchy with emoji section markers
- Technology matrix for quick scanning
- Copy-paste friendly setup commands
- Real-world code examples
- Testing strategy overview
- Responsive formatting for web and CLI viewing
- Contact information with multiple channels

Customization points:
1. Replace `yourusername` in clone URL
2. Update contact email/GitHub profile
3. Add project-specific badges
4. Include additional screenshots
5. Add contribution guidelines section