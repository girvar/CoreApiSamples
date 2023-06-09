# Sample to show multitenancy
- Single application catering to multiple tenant
- Support hangfire for application with tenant specific hangfire queue
- Support for running background service for each tenant

## Db connections
- API DB: Uses windows authentication. Databases are created & migrations done on application start
- Hangfire DB: Create database using below command
IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'HangfireDb')
BEGIN
    CREATE DATABASE [HangfireDb]
    
END
	
## Adding migrations
cd "\CoreApiSamples\Repositories\"
dotnet ef migrations add Patient_Initial -s .. --project .. --context PatientContext -o Repositories/Migrations/Patient

# Run application
dotnet run

# Local Development
## Use Postman
Use collection in postman to get request

## Use Curl
curl -X GET http://localhost:6001/api/patients -H "tenant:mdacc"

curl -X POST -H "Content-Type: application/json" -H "tenant:mdacc" -d "{\"ID1\": \"MD-001\", \"firstName\": \"mdacc_firstname1\", \"lastName\": \"mdacc_lastname1\", \"dateOfBirth\": null, \"sex\": 0}" http://localhost:6001/api/patients
