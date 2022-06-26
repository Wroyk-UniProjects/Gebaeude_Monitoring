#!/bin/sh
apt-get update
apt-get install -y apt-transport-https && 
apt-get update && 
apt-get install -y dotnet-sdk-6.0
apt-get install unzip -y
apt-get install curl