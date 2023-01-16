Debug:
- npm install (if node_modules not exists)
- dotnet build
	- dotnet restore
- dotnet swagger
- npx openapi

Prod: 
- npm install (if node_modules not exists)
- dotnet build DEBUG
	- dotnet restore
- dotnet swagger (based on DEBUG build)
- npx openapi
- npm install (if node_modules not exists)
- dotnet publish
	- dotnet build PROD
	- dotnet restore
- npm install ?
- npm run build