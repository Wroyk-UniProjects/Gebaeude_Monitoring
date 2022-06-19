#!/bin/sh
# Remove python2
sudo apt purge -y python2.7-minimal

# You already have Python3 but
# don't care about the version
sudo ln -s /usr/bin/python3 /usr/bin/python

# Same for pip
sudo apt install -y python3-pip
sudo ln -s /usr/bin/pip3 /usr/bin/pip

sudo apt-get update
sudo apt-get install libgpiod2 -y
sudo apt-get install xrdp -y
sudo systemctl enable xrdp
pip3 install -r requirements.txt

sudo mkdir /etc/BuildingMonitoring
sudo mv config.ini /etc/BuildingMonitoring
