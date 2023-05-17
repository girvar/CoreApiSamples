# Sample to show multitenancy

## Db connections
Uses windows authentication. Databases are created & migrations done on start

## Adding migrations
cd "\CoreApiSamples\Repositories\"
dotnet ef migrations add Patient_Initial -s .. --project .. --context PatientContext -o Repositories/Migrations/Patient

# Run application
dotnet run

# Local Development
## Use Postman
Use collection in postman to get request

## Use Curl
curl -X GET http://localhost:6000/api/patients -H "tenant:mdacc"

curl -X POST -H "Content-Type: application/json" -H "tenant:mdacc" -d "{\"ID1\": \"MD-001\", \"firstName\": \"mdacc_firstname1\", \"lastName\": \"mdacc_lastname1\", \"dateOfBirth\": null, \"sex\": 0}" http://localhost:6000/api/patients
