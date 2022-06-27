#!/bin/sh
apt-get install wget -y
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c Current
apt-get install unzip -y
apt-get install curl -y