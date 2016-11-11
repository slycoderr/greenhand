#include <OneWire.h>
#include <SoftwareSerial.h>

int DS18S20_Pin = 4; //DS18S20 Signal pin on digital 2

//Temperature chip i/o
OneWire ds(DS18S20_Pin);  // on digital pin 2

SoftwareSerial display(3, 2);

#include <ESP8266WiFi.h>

const char* ssid = "********";
const char* password = "********";

const char* host = "www.example.com";

const char* deviceId = "1";
const char* deviceType = "TEMP";
const char* deviceName = deviceId + deviceType;


void setup()
{
  Serial.begin(115200);
  Serial.println();

  Serial.printf("Connecting to %s ", ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
  Serial.println(" connected");
}

void loop(void)
{
  WiFiClient client;

  Serial.printf("\n[Connecting to %s ... ", host);
  if (client.connect(host, 80))
  {
    //Serial.println("connected]");

    //Serial.println("[Sending a request]");
    //client.print(String("GET /") + " HTTP/1.1\r\n" +
    //			 "Host: " + host + "\r\n" +
    //			 "Connection: close\r\n" +
    //			 "\r\n"
    //			);

    //Serial.println("[Response:]");

    while (client.connected())
    {
      if (client.available())
      {
        String line = client.readStringUntil('\n');
        Serial.println(line);

        if (line == "ReadValue")
        {
          if (deviceType == "TEMP")
          {
            float temperature = getTemp();
            int tmp = (int) temperature;

            Serial.println(tmp);
            client.print(tmp);
          }
        }

        else if (line == "ReadName")
        {
          client.print(deviceName);
        }
      }
    }
    client.stop();
    Serial.println("\n[Disconnected]");
  }
  else
  {
    Serial.println("connection failed!]");
    client.stop();
  }
}


float getTemp() {
  //returns the temperature from one DS18S20 in DEG Celsius

  byte data[12];
  byte addr[8];

  if ( !ds.search(addr)) {
    //no more sensors on chain, reset search
    ds.reset_search();
    return -1000;
  }

  if ( OneWire::crc8( addr, 7) != addr[7]) {
    Serial.println("CRC is not valid!");
    return -1000;
  }

  if ( addr[0] != 0x10 && addr[0] != 0x28) {
    Serial.print("Device is not recognized");
    return -1000;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44, 1); // start conversion, with parasite power on at the end

  byte present = ds.reset();
  ds.select(addr);
  ds.write(0xBE); // Read Scratchpad

  for (int i = 0; i < 9; i++) { // we need 9 bytes
    data[i] = ds.read();
  }

  ds.reset_search();

  byte MSB = data[1];
  byte LSB = data[0];

  float tempRead = ((MSB << 8) | LSB); //using two's compliment
  float TemperatureSum = tempRead / 16;

  return (TemperatureSum * 18 + 5) / 10 + 32;
}
