#!/bin/sh
sudo apt-get update
sudo apt-get install libgpiod2 -y
sudo apt-get install xrdp -y
sudo systemctl enable xrdp
pip3 install -r requirements.txt
