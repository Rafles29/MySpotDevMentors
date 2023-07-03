# DB

## Add migration
 from Infrastructure folder
```bash
dotnet ef migrations add Init --startup-project ../MySpot.Api -o ./DAL/Migrations
```

```bash
dotnet ef database update --startup-project ../MySpot.Api
```