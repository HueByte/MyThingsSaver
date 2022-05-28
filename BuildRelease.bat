@echo off
@REM mode con: cols=70 lines=50
set root=%cd%
set client=%root%/backend/App/client
set app=%root%/backend/App

echo ==== Restore Packages ====
echo .
dotnet restore %app%

echo ==== Building Client ====
cd %client%
@REM start /B /wait "Building Front" cmd /c "npm update" 
@REM start /B /wait "Fixing audits" cmd /c "npm audit fix"
start /B /wait "Building Front" cmd /c "npm run build" 

echo ==== Building Windows x64 ====
cd %app%
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --output %root%/Release/Windowsx64


echo ==== Building Windows x32 ====
cd %app%
dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --output %root%/Release/Windowsx86


echo ==== Building Linux x64 Standalone ====
cd %app%
dotnet publish -r linux-x64 --self-contained true -o %root%/Release/Linux64_Standalone


echo ==== Building Linux ARM x86 ====
cd %app%
dotnet publish -r linux-arm -o %root%/Release/Linux86-Arm


echo ==== Building MacOS x64 ====
cd %app%
dotnet publish -r osx-x64 -o %root%/Release/MacOSx64


echo ==== Moving client to releases ====
cd %client% 

xcopy "%client%/build" "%root%/Release/Windowsx64/build" /E /I
xcopy "%client%/build" "%root%/Release/Windowsx86/build" /E /I
xcopy "%client%/build" "%root%/Release/Linux64_Standalone/build" /E /I
xcopy "%client%/build" "%root%/Release/Linux86-Arm/build" /E /I
xcopy "%client%/build" "%root%/Release/MacOSx64/build" /E /I

rmdir "%client%/build" /S /Q
cd %root%
