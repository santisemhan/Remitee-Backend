<div align="center" id="top">
  <img src="https://github.com/user-attachments/assets/e07b0b03-6b80-4497-8398-b025f1388ce4" width="300" alt="PSG Logo" />
</div>
<br/>

<div align="center" id="top">
  <img src="https://github.com/user-attachments/assets/868878ee-1302-42f6-a8a4-441f2d5c14ab" width="900" alt="Remitee Swagger" />
</div>

## :dart: About ##
This repository contains the backend application for the Remitee technical challenge, built with .NET 8.


## :checkered_flag: Get Started ##

### 🧰 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local instance)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### 🚀 Running the Project

```bash
# Clone this project
git clone https://github.com/santisemhan/Remitee-Backend

# Navigate into the folder
cd Remitee-Backend

# Restore dependencies
dotnet restore
```

### ⚙️ Configuration

Update the connection string in `appsettings.json` to point to your local SQL Server instance. Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RemiteeLocal;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 🗃️ Create and Seed the Database

1. Apply migrations (choose one depending on your environment):

- From terminal:
  ```bash
  # Apply migrations
  dotnet ef database update
  ```

- Or from the Visual Studio Package Manager Console:
  ```powershell
  # Apply migrations
  Update-Database
  ```

2. Populate initial data:

Run the following SQL script to insert sample data:

```sql
-- scripts/data.sql
```

Execute it using SQL Server Management Studio (SSMS) or your preferred SQL client connected to the local instance.

### 🧪 Test the API

Run the API:

```bash
dotnet run
```

Once the API is running, you can access the Swagger documentation at:

```
https://localhost:5001/api/docs
```
