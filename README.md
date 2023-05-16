# Library: Blazor

An work-in-progress implementation of a domain-agnostic entity platformer.

## Start Up

1. Start MSSQL Server
1. Configure a connection string in `appsettings.json` / `appsettings.Development.json`
1. Upload initial database schema
1. Run the server

### Development

1. `dotnet watch --project Library.Client/` for HOT Reloading and watching changes
1. `dotnet build` to build all projects
1. `dotnet publish` to create a distributable

### Converter Tests
#### Done
- ValueConverter
- LogicalOperatorConverter
- FilterExpressionConverter
- QueryExpressionConverter
- ConditionExpressionConverter

#### To Do
- EntityConverter