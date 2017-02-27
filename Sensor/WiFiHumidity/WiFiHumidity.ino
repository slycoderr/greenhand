#define DEBUG

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
  String line = Serial.readStringUntil('\n');

  if (line == "config")
  {
    sensor.ReconfigureWifi();
  }

  else
  {
    ReadHumidity();
  }

  delay(30000);
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
