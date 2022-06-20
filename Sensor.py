# import adafruit library
import sys
import adafruit_dht
import board
import time
import requests
import configparser
from datetime import datetime

default_path = "/home/pi/.config"


class ApiConnection:
    # variables for requests
    base_url = None
    api_key = None
    response = None
    rasp_id = None

    # path for debug option
    debug_path = "{}/BuildingMonitoring/{}_debugInfo.txt".format(default_path, datetime.now().strftime("%d-%m-%Y"))

    def __init__(self, base_url, api_key, rasp_id):
        self.base_url = base_url
        self.api_key = api_key
        self.rasp_id = rasp_id

    def send_measurements(self, humidity, temperature):
        payload = {"humid": humidity, "temper": temperature}

        if sys.argv[1].lower() == "debug":
            current_time_string = datetime.now().strftime("%H:%M:%S")
            f = open(self.debug_path, 'a+')
            f.write("[{}] {}\r\n".format(current_time_string, payload))
            f.close()
            return True

        self.response = requests.put(self.base_url + "rooms/" + str(self.rasp_id) + "/measurement/", json=payload)

        if self.response.status_code == 200:
            return True
        else:
            return False

    def get_update_rate(self):
        self.response = requests.get(self.base_url + "rooms/config")
        if self.response.status_code != 200:
            return None
        return int(self.response.json()["updateRate"])


class Main:
    # variables
    # timeout is in seconds
    timeout = None
    running = True
    api_url = None
    api_key = None
    rasp_id = None

    # path to the config file
    config_path = r"/home/pi/.config/BuildingMonitoring/config.ini"

    # define sensor and data port
    # port GPIO23 (Pin 16 on Rasp 4)
    dht_sensor = adafruit_dht.DHT22(board.D23, use_pulseio=False)

    # create instance for the api connection
    api_connection = None

    def main(self):

        # read config
        self._init_config()

        # read sensor data
        while self.running:
            # logfile for storing errors
            log_file = "{}/BuildingMonitoring/{}_logInfo.txt".format(default_path, datetime.now().strftime("%d-%m-%Y"))
            try:
                self.dht_sensor.measure()
                humidity = self.dht_sensor.humidity
                temperature = self.dht_sensor.temperature
                print(str(temperature) + "C")
                print(str(humidity) + "%")
                success = self.api_connection.send_measurements(humidity, temperature)
                if not success:
                    raise Exception("Error at sending data to the API please contact the developer")
                if not sys.argv[1] == "debug":
                    self._timeout_and_update()
                else:
                    time.sleep(self.timeout)

            except RuntimeError:
                # The DHT Device gives faulty errors. catch and try again in 2 sec
                time.sleep(2.0)
                continue
            except Exception as error:
                # When other error occurs while reading terminate the script adn write to log file
                current_time_string = datetime.now().strftime("%H:%M:%S")
                f = open(log_file, 'a+')
                f.write("[{}] {}\r\n".format(current_time_string, repr(error)))
                f.close()
                self.dht_sensor.exit()
                print("Error occurred Look at the logfiles at " + log_file)
                input("Press Enter to continue...")
                raise sys.exit(0)

    def _timeout_and_update(self):
        def _update_time():
            new_update_rate = self.api_connection.get_update_rate()
            if new_update_rate != self.timeout & new_update_rate is not None:
                self.timeout = new_update_rate
                self._update_config()

        for x in range(self.timeout):
            time.sleep(1)
            if x % 5 == 0:
                _update_time()
        _update_time()

    def _init_config(self):
        config_parser = configparser.RawConfigParser()
        config_parser.read(self.config_path)
        self.api_url = config_parser.get("Default", "API-URL")
        self.api_key = config_parser.get("Default", "API-KEY")
        temp_timeout = config_parser.get("Default", "UPDATE-RATE")
        temp_rasp_id = config_parser.get("Default", "RASP-ID")

        not_int_msg = "is not an int.\r\n" + "Please check the config file.\r\n" + "Exiting script\r\n"
        try:
            self.timeout = int(temp_timeout)
        except ValueError:
            print("update rate" + not_int_msg)
            input("Press Enter to continue...")
            sys.exit(1)
        try:
            self.rasp_id = int(temp_rasp_id)
        except ValueError:
            print("rasp-id " + not_int_msg)
            input("Press Enter to continue...")
            sys.exit(1)

        self.api_connection = ApiConnection(self.api_url, self.api_key, self.rasp_id)

    def _update_config(self):
        config_parser = configparser.RawConfigParser()
        config_parser.read(self.config_path)
        config_parser.set("Default", "UPDATE-RATE", str(self.timeout))


if __name__ == "__main__":
    Main.main(Main())
