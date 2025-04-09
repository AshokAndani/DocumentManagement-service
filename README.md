# 📄 Document Management API

A .NET-based API for uploading, storing, indexing, and searching documents using Full-Text Search and JWT authentication.

---

## 🔧 Features

- Upload documents (stored as BLOBs)  
- Extract and store text from documents  
- Full-Text Search support  
- JWT-based authentication  
- Swagger UI documentation  

---

## 🛠️ Tech Stack

- ASP.NET Core (.NET 9)  
- Entity Framework Core  
- SQL Server (with Full-Text Search)  
- JWT Authentication  
- Swagger / Swashbuckle  

---

## 🚀 Deployment Steps

### 1. Clone the Repository

```bash
git clone https://github.com/your-org/document-management-api.git
cd document-management-api
```

### 2. Update Configuration

Create a file named `appsettings.Production.json` in the root of your project with the following content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DocumentDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtOptions": {
    "SecretKey": "your-secure-jwt-secret"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3. Apply EF Core Migrations

```bash
dotnet ef database update --configuration Production
```

### 4. Build & Publish

```bash
dotnet publish -c Release -o ./dist
```

### 5. Enable Full-Text Search in SQL Server
- Ensure Full-Text Search is installed and enabled.
- Create a Full-Text index on the ExtractedText column of the Documents table.

### 6. Access the API
- Base URL: `https://your-domain.com/api`
- Swagger UI: `https://your-domain.com/swagger`
