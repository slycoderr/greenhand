#define DEBUG

#include "SensorCore.h"
#include <Wire.h>
#include "Adafruit_MCP9808.h"

SensorCore sensor = SensorCore(2);
Adafruit_MCP9808 tempsensor = Adafruit_MCP9808();

void setup()
{
  sensor.Initialize();

  if (!tempsensor.begin())
  {
    #if defined(DEBUG)
        Serial.println("Couldn't find temp sensor!");
    #endif
    while (1);
  }

  #if defined(DEBUG)
    Serial.println("temp sensor initialized");
  #endif
}

void loop()
{
  String line = Serial.readStringUntil('\n');

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
    ReadTemp();
  }

  delay(30000);
}

void ReadTemp()
{
  #if defined(DEBUG)
    Serial.println("wake up temp sensor.... "); // wake up MSP9808 - power consumption ~200 mikro Ampere
  #endif
  
  tempsensor.shutdown_wake(0);   // Don't remove this line! required before reading temp

  float c = tempsensor.readTempC();
  float f = c * 9.0 / 5.0 + 32;

  #if defined(DEBUG)
    Serial.println(f + String("*F"));
  #endif
  
  delay(250);
  
  #if defined(DEBUG)
    Serial.println("Shutdown MCP9808.... ");
  #endif
  
  tempsensor.shutdown_wake(1);
  sensor.SendData(String(f), "temp");
}

