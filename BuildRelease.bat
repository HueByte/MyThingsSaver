@echo off
@REM mode con: cols=70 lines=50
set root=%~dp0%
set client=%root%src\App\client
set app=%root%src\App

echo "Root %root%"
echo "Client %client%"
echo "App %app%"

echo ==== Building DEBUG build for client gen ====
cd %app% 
dotnet build


echo ==== Building Linux ARM x86 ====
cd %app%
dotnet publish -c Release /p:DebugType=None /p:DebugSymbols=false -r linux-arm -p:PublishSingleFile=true --self-contained true -o %root%Release/Linux86-Arm


echo ==== Building Windows x64 ====
cd %app%
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true --output %root%Release/Windowsx64


echo ==== Building Windows x32 ====
cd %app%
dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --self-contained true --output %root%Release/Windowsx86


echo ==== Building Linux x64 Standalone ====
cd %app%
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true -o %root%Release/Linux64_Standalone


echo ==== Building MacOS x64 ====
cd %app%
dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained true -o %root%Release/MacOSx64


echo ==== Cleaning build files ====
cd %client% 

rmdir "%root%Release/Windowsx64/client" /S /Q 
rmdir "%root%Release/Windowsx86/client" /S /Q 
rmdir "%root%Release/Linux64_Standalone/client" /S /Q 
rmdir "%root%Release/Linux86-Arm/client" /S /Q 
rmdir "%root%Release/MacOSx64/client" /S /Q 

rmdir "%client%/build" /S /Q
cd %root%
