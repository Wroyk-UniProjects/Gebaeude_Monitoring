# import adafruit library
import adafruit_dht
import board
import time
import requests


class Main:

    def main(self):
        # variables
        # timeout is in seconds
        timeout = 60
        running = True

        #  config will be later used
        # config_path = None

        # define sensor and data port
        # port GPIO2 (Pin 3 on Rasp 4)
        dht_sensor = adafruit_dht.DHT22(board.D2, use_pulseio=False)

        # create instance for teh api connection
        api_connection = self.ApiConnection("https://building-monitoring.azurewebsites.net/api/", None)
        # read sensor data

        while running:
            humidity = dht_sensor.humidity
            temperature = dht_sensor.temperature
            print(str(temperature) + "C")
            print(str(humidity) + "%")
            success = api_connection.send_measurements(humidity, temperature)
            if not success:
                print("Error at sending data to the API please contact the developer")
            time.sleep(timeout)

    class ApiConnection:

        # variables for requests
        base_url = None
        api_key = None
        response = None
        rasp_id = 1

        def __init__(self, base_url, api_key):
            self.base_url = base_url
            self.api_key = api_key

        def send_measurements(self, humidity, temperature):
            payload = {"humid": humidity, "temper": temperature}
            self.response = requests.put(self.base_url + "rooms/" + str(self.rasp_id) + "/measurement/", json=payload)

            if self.response.status_code == 201:
                return True
            else:
                return False


if __name__ == "__main__":
    Main.main(Main())
