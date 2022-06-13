@echo off
set migrationName=%1
dotnet ef migrations add %migrationName% --startup-project ../App