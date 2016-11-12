//#include <OneWire.h>
#include <SoftwareSerial.h>

int DS18S20_Pin = 4; //DS18S20 Signal pin on digital 2

//Temperature chip i/o
//OneWire ds(DS18S20_Pin);  // on digital pin 2

//SoftwareSerial display(3, 2);

//#include <ESP8266WiFi.h>

#include <WiFi.h>

const char* ssid = "green";
const char* password = "test1234";

const char* host = "192.168.0.105";

const char* deviceId = "1";
const char* deviceType = "TEMP";
String deviceName = "TEMP1";
WiFiClient client;
IPAddress server(192,168,0,105); 

void setup()
{
  Serial.begin(115200);
  Serial.println();
    WiFi.begin(ssid, password);
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, LOW);

  printStatus();

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.println("connecting...");
    
  printStatus();
  }
  
      IPAddress ip = WiFi.localIP();
          Serial.println(ip);
}

void printStatus()
{
  if(WiFi.status() == WL_CONNECTED)
{
  Serial.println("WL_CONNECTED...");
}

if(WiFi.status() == WL_NO_SHIELD)
{
  Serial.println("WL_NO_SHIELD...");
}

if(WiFi.status() == WL_IDLE_STATUS)
{
  Serial.println("WL_IDLE_STATUS...");
}


if(WiFi.status() == WL_NO_SSID_AVAIL)
{
  Serial.println("WL_NO_SSID_AVAIL...");
}

if(WiFi.status() == WL_SCAN_COMPLETED)
{
  Serial.println("WL_SCAN_COMPLETED...");
}

if(WiFi.status() == WL_CONNECT_FAILED)
{
  Serial.println("WL_CONNECT_FAILED...");
}

if(WiFi.status() == WL_CONNECTION_LOST)
{
  Serial.println("WL_CONNECTION_LOST...");
}

if(WiFi.status() == WL_DISCONNECTED)
{
  Serial.println("WL_DISCONNECTED...");
}
}

void connect()
{
  if (client.connect(server, 80))
  {
    IPAddress ip = WiFi.localIP();
     printStatus();
    Serial.print(" connected with ip ");
    Serial.println(ip);
    digitalWrite(LED_BUILTIN, HIGH);
  }
  else
  {
    Serial.println("failed to connect");
     printStatus();
    digitalWrite(LED_BUILTIN, LOW);
  }
}


void loop(void)
{
  if (!client.connected())
  {
    connect();
  }

  while (client.connected())
  {
      String line = client.readStringUntil('\n');
      Serial.println(line);

      if (line == "ReadValue")
      {
        if (deviceType == "TEMP")
        {
          //float temperature = getTemp();
          //int tmp = (int) temperature;

          Serial.println(75);
          client.print(75);
        }
      }

      else if (line == "ReadName")
      {
          Serial.println(deviceName);
        client.print(deviceName);
      }
    
  }

digitalWrite(LED_BUILTIN, LOW);
   printStatus();
  client.stop();
}

/*
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
*/
