# Blog
A blog server made with ASP.NET and its CLI client.

### Prerequisites
* .Net SDK 8.0 installed
* A running Microsoft SQL Server (server-side only)
    + To create the server with Docker:
        ```sh
        $ docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<yourPassword>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
        ```

### Getting Started
* Server
    1. Set up the `ConnectionString` user secret:
        ```sh
        $ dotnet user-secrets set "ConnectionString" "Server=localhost; Database=<yourDatabase>; User Id=sa; Password=<yourPassword>; TrustServerCertificate=True"
        ```
    3. Run the migrations to initialize the database: 
        ```sh
        $ dotnet ef database update
        ```
    4. Run `dotnet run server --launch-profile https` to start the HTTPS server.
* Client
    1. Start the CLI: 
        ```sh
        $ dotnet run client <server_address>:<port>
        ```
    2. Enter `help` to list all available commands.
