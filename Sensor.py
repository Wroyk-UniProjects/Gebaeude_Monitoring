# import adafruit library
import adafruit_dht
import board

# variables

humidity = None
temperature = None
timeout = None
running = True

# config will be later used
configPath = None

# define sensor and data port
# port GPIO2 (Pin 3 on Rasp 4)
dhtSensor = adafruit_dht.DHT22(board.D2, use_pulseio=False)
gpio = 2

# read sensor data

while running:
    humidity = dhtSensor.humidity
    temperature = dhtSensor.temperature
    print(temperature)
    print(humidity)
