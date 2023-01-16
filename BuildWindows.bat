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


echo ==== Building Windows x64 ====
cd %app%
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true --output %root%Release/Windowsx64


echo ==== Cleaning build files ====
cd %client% 

rmdir "%root%Release/Windowsx64/client" /S /Q 
rmdir "%client%/build" /S /Q
cd %root%
