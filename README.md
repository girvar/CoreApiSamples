# Sample to show multitenancy

## Db connections
Uses windows authentication. Databases are created & migrations done on start

## Adding migrations
cd "\CoreApiSamples\Repositories\"
dotnet ef migrations add Patient_Initial -s .. --project .. --context PatientContext -o Repositories/Migrations/Patient

# Run application
dotnet run

# Use Postman
Use collection in postman to get request