
requirements:
-Visual studio 2022

-.NET SDK 8.0 (TargetFramework:
net8.0)

-SQL Server / SQL Server Express / LocalDB

settings before running:
1. in file appsettings.json , make sure you got 2 valid connection strings :

   "ConnectionStrings": {
    "DevHiveDbConnectionString": "server=localhost\\MSSQLSERVER01;Database=DevHiveDb;Trusted_Connection=True;TrustServerCertificate=Yes",
    "DevHiveAuthDbConnectionString": "server=localhost\\MSSQLSERVER01;Database=DevHiveAuthDb;Trusted_Connection=True;TrustServerCertificate=Yes"
   }

  And if you don't have localhost\MSSQLSERVER01 , 
 change to something else, for example:
  .\SQLEXPRESS
  or
  (localdb)\MSSQLLocalDB

2. set Cloudinary in user secrets (important! for the project to run)
   "Cloudinary": {
    "CloudName": "SET_IN_USER_SECRETS",
    "ApiKey": "SET_IN_USER_SECRETS",
    "ApiSecret": "SET_IN_USER_SECRETS"
  }

 3. create DB and run EF Migrations:
  in package manager console:
  Update-Database -Context AuthDbContext
  Update-Database -Context DevHiveDbContext
 
 run:
 open the project in visual studio and and run (IIS Express / https)
 
 SuperAdmin(seed) user:
 Email: superadmin@devhive.com
 !Password: SuperAdmin123!

  
