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
- **Entity Framework Core 9** - Modern ORM for data access
- **ASP.NET Core Web API** - RESTful API framework
- **SQL Server** - Database (LocalDB/Full instance)
- **Scalar** - Modern API documentation and testing UI
- **Clean Architecture** - Architectural pattern
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Domain-Driven Design** - Business logic modeling
- **Dependency Injection** - Built-in DI container

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
- **🗑️ Soft Delete** - Data preservation with logical deletion
- **⏰ Audit Trail** - Track creation and modification timestamps
- **✅ Validation** - Comprehensive input validation
- **🎯 Business Rules** - Domain-specific business logic enforcement
- **🔒 Fine Calculation** - Automatic overdue fine calculation
- **📊 Repository Pattern** - Generic and specific repositories
- **🎨 DTO Pattern** - Separation of domain and data transfer objects
- **🔄 Async/Await** - Full async operations for scalability
- **📝 Structured Logging** - Comprehensive logging with ILogger
- **🌐 CORS Support** - Cross-origin resource sharing enabled

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/9.0))
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code
- Git

### Quick Start (Visual Studio)

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/library-management-system.git
   ```

2. **Open solution**
   - Open `LibraryApp.sln` in Visual Studio 2022

3. **Update Database**
   - Package Manager Console (PMC):
     ```powershell
     Update-Database -Project LibraryApp.Data -StartupProject LibraryApp.Web
     ```

4. **Run the application**
   - Press `F5` or click Run
   - Scalar UI will automatically open in your browser

### Installation (.NET CLI)

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/library-management-system.git
   cd LibraryApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update connection string** (Optional)
   - Edit `LibraryApp.Web/appsettings.json`
   - Modify `DefaultConnection` if needed (default uses LocalDB)

4. **Run migrations**
   ```bash
   dotnet ef database update --project LibraryApp.Data --startup-project LibraryApp.Web
   ```

5. **Run the application**
   ```bash
   dotnet run --project LibraryApp.Web
   ```

6. **Access the API**
   - **Scalar UI**: `https://localhost:5001/scalar/v1` (Interactive docs)
   - **API Base**: `https://localhost:5001/api`
   - **OpenAPI Spec**: `https://localhost:5001/openapi/v1.json`

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
    ├── Controllers/               # RESTful API controllers
    │   ├── AuthorsController.cs      # Author CRUD endpoints
    │   ├── CategoriesController.cs   # Category CRUD endpoints
    │   ├── MembersController.cs      # Member CRUD endpoints
    │   └── BorrowRecordsController.cs # Borrowing endpoints
    ├── Program.cs                 # Application entry point & DI setup
    ├── appsettings.json          # Configuration (DB connection, etc.)
    └── Properties/
        └── launchSettings.json    # Launch configuration for Scalar
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

## 🔧 RESTful API Endpoints

### 📖 Authors API
- `GET /api/authors` - Get all authors
- `GET /api/authors/{id}` - Get author by ID
- `POST /api/authors` - Create new author
- `PUT /api/authors/{id}` - Update author
- `DELETE /api/authors/{id}` - Delete author (soft delete)
- `GET /api/authors/search?searchTerm={term}` - Search authors
- `GET /api/authors/with-books` - Get authors with books

### 📚 Categories API
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category (soft delete)
- `GET /api/categories/search?searchTerm={term}` - Search categories
- `GET /api/categories/with-books` - Get categories with books

### 👥 Members API
- `GET /api/members` - Get all members
- `GET /api/members/{id}` - Get member by ID
- `POST /api/members` - Create new member
- `PUT /api/members/{id}` - Update member
- `DELETE /api/members/{id}` - Delete member (soft delete)
- `GET /api/members/search?searchTerm={term}` - Search members
- `GET /api/members/active` - Get active members
- `GET /api/members/email/{email}` - Get member by email

### 📝 Borrow Records API
- `GET /api/borrowrecords` - Get all borrow records
- `GET /api/borrowrecords/{id}` - Get borrow record by ID
- `POST /api/borrowrecords` - Create new borrow record (borrow book)
- `PUT /api/borrowrecords/{id}/return` - Return book
- `GET /api/borrowrecords/member/{memberId}` - Get records by member
- `GET /api/borrowrecords/book/{bookId}` - Get records by book
- `GET /api/borrowrecords/active` - Get active borrow records
- `GET /api/borrowrecords/overdue` - Get overdue records
- `GET /api/borrowrecords/{id}/fine` - Calculate fine amount

**Total: 31 RESTful API Endpoints** 🎯

## 📖 API Documentation with Scalar

This project uses **Scalar** for modern, interactive API documentation:

### ✨ Features:
- **Interactive Testing** - Test endpoints directly from the UI
- **Automatic Code Examples** - C#, JavaScript, Python, cURL examples
- **Beautiful UI** - Modern, responsive design with dark mode
- **Fast Search** - Quickly find endpoints
- **Request/Response Examples** - Clear documentation

### 🌐 Access Scalar UI:
```
https://localhost:5001/scalar/v1
```

### 📋 Scalar Features Demonstrated:
- ✅ All 31 endpoints automatically documented
- ✅ Request/Response schemas
- ✅ Try-it-out functionality
- ✅ HTTP status codes explanation
- ✅ Structured error responses

## 🔥 API Usage Examples

### Create a New Author
```bash
curl -X POST https://localhost:5001/api/authors \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "J.R.R.",
    "lastName": "Tolkien",
    "biography": "English writer and philologist",
    "birthDate": "1892-01-03"
  }'
```

### Get All Categories
```bash
curl https://localhost:5001/api/categories
```

### Search Members
```bash
curl "https://localhost:5001/api/members/search?searchTerm=john"
```

### Borrow a Book
```bash
curl -X POST https://localhost:5001/api/borrowrecords \
  -H "Content-Type: application/json" \
  -d '{
    "memberId": "your-member-id",
    "bookId": "your-book-id"
  }'
```

### Return a Book
```bash
curl -X PUT https://localhost:5001/api/borrowrecords/{id}/return
```

### Calculate Fine
```bash
curl https://localhost:5001/api/borrowrecords/{id}/fine
```

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

## ✨ Recent Updates

### v2.0.0 - RESTful API Implementation (October 2025)
- ✅ **4 RESTful Controllers** added (31 total endpoints)
- ✅ **Scalar Integration** - Modern API documentation UI
- ✅ **UnitOfWork Pattern** - Complete implementation
- ✅ **Domain Services** - BorrowingService, BookAvailabilityService
- ✅ **Repository Interfaces** - Full IRepository<T> implementation
- ✅ **Dependency Injection** - Centralized service registration
- ✅ **CORS Support** - Cross-origin requests enabled
- ✅ **Structured Logging** - ILogger integration throughout
- ✅ **CancellationToken** - Request cancellation support
- ✅ **Soft Delete** - All entities support logical deletion

## 🚀 Future Enhancements

- [ ] Pagination support for list endpoints
- [ ] Authentication and Authorization (JWT)
- [ ] Email notifications for overdue books
- [ ] Advanced filtering and sorting
- [ ] API rate limiting
- [ ] Advanced reporting and analytics
- [ ] Background job processing (Hangfire)
- [ ] Caching implementation (Redis)
- [ ] API versioning
- [ ] Health checks endpoint

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
