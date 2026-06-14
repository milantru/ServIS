# Migrations

```
dotnet ef migrations list --project .\ServISData\ServISData.csproj --startup-project .\ServISWebApp\ServISWebApp.csproj
```

# Connect to local dev db
```
docker exec -it servisdb psql -U admin -d servisdb
```
