#!/bin/sh
apt-get wget -y
wget https://packages.microsoft.com/config/ubuntu/21.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt-get update; \
apt-get install -y apt-transport-https && \
apt-get update && \
apt-get install -y dotnet-sdk-6.0
apt-get update; \
apt-get install -y apt-transport-https && \
apt-get update && \
apt-get install -y aspnetcore-runtime-6.0
apt-get install unzip -y
apt-get install curl -y