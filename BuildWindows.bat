@echo off
mode con: cols=70 lines=50
set root=%cd%

cd %root%/backend/App 
echo Publishing the API
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --output %root%/Deploy/MyThingsSaver

cd %root%/client
echo Creating front-app build
start /B /wait "Building Front" cmd /c "npm update" 
start /B /wait "Fixing audits" cmd /c "npm audit fix"
start /B /wait "Building Front" cmd /c "npm run build" 
echo Moving build to %root%/Deploy/MyThingsSaver
move %cd%/build %root%/Deploy/MyThingsSaver/
pause