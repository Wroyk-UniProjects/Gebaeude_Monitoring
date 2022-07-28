# Building Monitoring Pi

## Prerequisites

In order to be able to run the python script you will have to fullfill specific prerequisites.

#### Step One: The Pi OS/image
Please us the official [Paspberry Pi imager](https://www.raspberrypi.com/software/) to get the right image
Select Raspberry Pi OS as the OS.

#### Step Two: Installing dependencies
For installing the dependencies run the given shell script.
```dependencies.sh```

### Step Three: Running the script
Now you can run the script by running the shell script file.
```run.sh``` 

## Remote access for the Pi
Remote access for the Pi is already enabled. 
For Windows maschines please the Remote Desktop Connection

For linux please open the terminal on **YOUR** maschine and run
```sudo apt install remmina```
Once installed open it and connect to the Raspberry Pi

For the Connection you have to use the IP adresse for the Raspberry Pi. If only one Raspberry Pi is in the local network you can use raspberrypi as the name instead. 
