#!/bin/bash

$root = $PWD

cd $root/backend/App
dotnet publish r linux-arm -o $root/Deploy

cd $root/client
npm update --all
npm run build 
mv $root/client/build $root/Deploy
