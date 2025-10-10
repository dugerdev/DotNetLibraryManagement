# 📚 Library Management System

A comprehensive library management system built with **Clean Architecture** principles using **.NET 9** and **Entity Framework Core**. This project demonstrates modern software development practices, enterprise-level patterns, and scalable architecture design.

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles, ensuring separation of concerns, testability, and maintainability. The architecture is organized into four distinct layers:

```
┌─────────────────────────────────────────┐
│           Presentation Layer            │
│         (Web API Controllers)           │
├─────────────────────────────────────────┤
│           Application Layer             │
│    (Use Cases, DTOs, Application        │
│           Services, Mappers)            │
├─────────────────────────────────────────┤
│            Domain Layer                 │
│   (Entities, Interfaces, Services,      │
│        Exceptions, Value Objects)       │
├─────────────────────────────────────────┤
│         Infrastructure Layer            │
│   (Data Access, EF Core Context,        │
│    Repository Implementations)          │
└─────────────────────────────────────────┘
```

## 🛠️ Technology Stack

- **.NET 9** - Latest .NET framework
- **Entity Framework Core** - Modern ORM for data access
- **ASP.NET Core Web API** - RESTful API framework
- **SQL Server** - Database (configurable)
- **Clean Architecture** - Architectural pattern
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Domain-Driven Design** - Business logic modeling

## 📋 Features

### Core Functionality
- **📖 Book Management** - Add, update, delete, and search books
- **👤 Author Management** - Manage authors and their information
- **👥 Member Management** - Handle library members
- **📚 Category Management** - Organize books by categories
- **📝 Borrowing System** - Complete borrowing and return workflow
- **📊 Statistics** - Library usage statistics and reports

### Advanced Features
- **🔍 Advanced Search** - Search across multiple entities
- **📈 Pagination** - Efficient data loading
- **🗑️ Soft Delete** - Data preservation with logical deletion
- **⏰ Audit Trail** - Track creation and modification timestamps
- **✅ Validation** - Comprehensive input validation
- **🎯 Business Rules** - Domain-specific business logic enforcement

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/library-management-system.git
   cd library-management-system
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update connection string**
   - Open `appsettings.json` in the Web project
   - Update the connection string to point to your SQL Server instance

4. **Run migrations**
   ```bash
   dotnet ef database update --startup-project LibraryApp.Web
   ```

5. **Run the application**
   ```bash
   dotnet run --project LibraryApp.Web
   ```

6. **Access the API**
   - API: `https://localhost:7000`
   - Swagger UI: `https://localhost:7000/swagger`

## 📁 Project Structure

```
LibraryApp/
├── LibraryApp.Domain/              # Domain Layer
│   ├── Common/                     # Shared interfaces and services
│   │   ├── IRepository.cs         # Generic repository interface
│   │   ├── IUnitOfWork.cs         # Unit of work interface
│   │   ├── IBorrowingService.cs   # Domain service interfaces
│   │   └── ValueObjects/          # Email, ISBN, Money value objects
│   ├── Entities/                   # Domain entities
│   │   ├── Author.cs              # Author entity
│   │   ├── Book.cs                # Book entity
│   │   ├── Member.cs              # Member entity
│   │   ├── Category.cs            # Category entity
│   │   └── BorrowRecord.cs        # Borrowing record entity
│   ├── Enums/                      # Domain enums
│   │   └── BorrowStatus.cs        # Borrowing status enum
│   └── Exceptions/                 # Domain-specific exceptions
│       ├── DomainException.cs     # Base domain exception
│       ├── BookNotAvailableException.cs
│       └── MemberCannotBorrowException.cs
├── LibraryApp.Application/         # Application Layer
│   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Books/                 # Book-related DTOs
│   │   ├── Authors/               # Author-related DTOs
│   │   ├── Members/               # Member-related DTOs
│   │   ├── Categories/            # Category-related DTOs
│   │   └── BorrowRecords/         # Borrowing-related DTOs
│   ├── Services/                  # Application services
│   │   ├── AuthorApplicationService.cs
│   │   ├── BookApplicationService.cs
│   │   ├── MemberApplicationService.cs
│   │   ├── CategoryApplicationService.cs
│   │   └── BorrowRecordApplicationService.cs
│   └── Mappers/                   # Entity-DTO mappers
├── LibraryApp.Data/               # Infrastructure Layer
│   ├── Context/                   # EF Core context
│   │   └── LibraryDbContext.cs    # Main database context
│   ├── Configurations/            # EF Core configurations
│   │   ├── AuthorConfiguration.cs
│   │   ├── BookConfiguration.cs
│   │   └── ...
│   └── Repositories/              # Repository implementations
│       ├── AuthorRepository.cs
│       ├── BookRepository.cs
│       └── ...
└── LibraryApp.Web/                # Presentation Layer
    ├── Controllers/               # API controllers
    │   ├── AuthorsController.cs
    │   ├── BooksController.cs
    │   └── ...
    ├── Program.cs                 # Application entry point
    └── appsettings.json          # Configuration
```

## 🎯 Key Design Patterns

### Repository Pattern
```csharp
public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    // ... other CRUD operations
}
```

### Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IAuthorRepository Authors { get; }
    IBookRepository Books { get; }
    IMemberRepository Members { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### Domain Services
```csharp
public interface IBorrowingService
{
    Task<BorrowRecord> BorrowBookAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default);
    Task ReturnBookAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateFineAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);
}
```

## 🔧 API Endpoints

### Authors
- `GET /api/authors` - Get all authors
- `GET /api/authors/{id}` - Get author by ID
- `POST /api/authors` - Create new author
- `PUT /api/authors/{id}` - Update author
- `DELETE /api/authors/{id}` - Delete author

### Books
- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get book by ID
- `POST /api/books` - Create new book
- `PUT /api/books/{id}` - Update book
- `DELETE /api/books/{id}` - Delete book

### Members
- `GET /api/members` - Get all members
- `GET /api/members/{id}` - Get member by ID
- `POST /api/members` - Create new member
- `PUT /api/members/{id}` - Update member
- `DELETE /api/members/{id}` - Delete member

### Borrowing
- `POST /api/borrowing/borrow` - Borrow a book
- `POST /api/borrowing/return` - Return a book
- `GET /api/borrowing/overdue` - Get overdue books

## 🧪 Testing

The project is designed with testability in mind:

- **Unit Testing** - Domain logic and application services
- **Integration Testing** - API endpoints and data access
- **Repository Testing** - Data access layer testing
- **Domain Service Testing** - Business logic validation

## 📊 Database Schema

The database includes the following main entities:

- **Authors** - Author information and metadata
- **Books** - Book details, availability, and relationships
- **Members** - Library member information
- **Categories** - Book categorization
- **BorrowRecords** - Borrowing history and status

## 🎨 Code Quality

- **SOLID Principles** - Single responsibility, open/closed, etc.
- **Clean Code** - Readable, maintainable code structure
- **XML Documentation** - Comprehensive code documentation
- **Consistent Naming** - Clear and descriptive naming conventions
- **Error Handling** - Proper exception handling and validation

## 🚀 Future Enhancements

- [ ] Authentication and Authorization
- [ ] Email notifications for overdue books
- [ ] Advanced reporting and analytics
- [ ] Mobile application
- [ ] Multi-tenant support
- [ ] Caching implementation
- [ ] Background job processing

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Muhammed Düger**
- GitHub: [@dugerdev](https://github.com/dugerdev)
- LinkedIn: [Muhammed Düger](https://linkedin.com/in/dugerdev)
- Email: muhammedduger@gmail.com

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- .NET community for excellent documentation
- Entity Framework Core team for the amazing ORM

---

⭐ **If you found this project helpful, please give it a star!** ⭐
