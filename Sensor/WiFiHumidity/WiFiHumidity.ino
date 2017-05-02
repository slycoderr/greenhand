//#define DEBUG

#include "SensorCore.h"
#include "Adafruit_HTU21DF.h"

SensorCore sensor = SensorCore(3);
Adafruit_HTU21DF htu = Adafruit_HTU21DF();

void setup()
{
  sensor.Initialize();

  if (!htu.begin())
  {
    #if defined(DEBUG)
        Serial.println("Couldn't find humidity sensor!");
    #endif
    
    while (1);
  }

  #if defined(DEBUG)
    Serial.println("Humidity sensor initialized");
  #endif
}

void loop()
{
  #if defined(DEBUG)
    Serial.println("Waiting for a command...");
  #endif
  
  String line = Serial.readStringUntil('\n');

  #if defined(DEBUG)
    Serial.println("Read:"+line);
  #endif

  if (line == "config")
  {
    sensor.ReconfigureWifi();
  }

  else if (line == "Hello")
  {
	  sensor.ValidateSensor();
  }

  else if (line == "GetId")
  {
	  sensor.GetId();
  }

  else if (line == "SetApiKey")
  {
	  sensor.SetApiKey();
  }

  else
  {
    ReadHumidity();
  }
}

void ReadHumidity()
{
  #if defined(DEBUG)
    Serial.println("reading humidity.... ");
  #endif

  float h = htu.readHumidity();

  #if defined(DEBUG)
    Serial.println(h + String("%"));
  #endif
  
  sensor.SendData(String((int)h), "humid");
}
