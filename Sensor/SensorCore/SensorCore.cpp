#include "SensorCore.h"
#include <WiFiClientSecure.h>

SensorCore::SensorCore(int id)
{
	sensorId = id;
}

bool SensorCore::Initialize()
{
  /****
   * Initialize Serial
   */

  Serial.begin(9600);

  while (!Serial)
  {
    ; // wait for serial port to connect.
  }

  Serial.setTimeout(20000);
	
  #if defined(DEBUG)
	Serial.println("Serial connected.");
  #endif
  
  InitializeWiFi();
}

void SensorCore::InitializeWiFi()
{
   #if defined(DEBUG)
		Serial.println("Initializing WiFi...");
     #endif
	
  if (WiFi.status() == WL_CONNECTED)
  {
	  #if defined(DEBUG)
		Serial.println("Disconnecting WiFi...");
	   #endif
	
    WiFi.disconnect();
    delay(5000);
    RefreshWiFiStatus();
  }

  // attempt to connect to Wifi network:
  while (wifiStatus != WL_CONNECTED)
  {
	  #if defined(DEBUG)
		Serial.print("Attempting to connect to WEP network, SSID: ");
		Serial.println(ssid);
	#endif
    
    WiFi.begin(ssid.c_str(), ssidPassword.c_str());

    // wait 10 seconds for connection:
    delay(5000);

    wifiStatus = WiFi.status();
  }

  #if defined(DEBUG)
	// show wifi info
	Serial.println("");
	Serial.println("WiFi connected");
	Serial.println("IP address: ");
	Serial.println(WiFi.localIP());
  #endif
}

void SensorCore::SendData(String data, String type)
{
  WiFiClientSecure client;
  const String host = "greenhand.azurewebsites.net";
  const int httpsPort = 443;
  const char* fingerprint = "E9 59 FD 5C 80 F7 6D F7 A5 93 AA E0 96 86 E6 04 F7 4B E8 B0";

  #if defined(DEBUG)
	Serial.println("Connecting to server...");
    #endif

  if (client.connect(host.c_str(), httpsPort))
  {
	  #if defined(DEBUG)
		Serial.println("Connected to Server.");
	  #endif

    if (client.verify(fingerprint, host.c_str())) 
    {
		#if defined(DEBUG)
			Serial.println("certificate matches");
			Serial.println("Sending request... /sensor/store/"+apiKey + "/" + sensorId + "/" + type + "/" + data);
		#endif
          // Make a HTTP request:
          client.println("POST /sensor/store/"+apiKey + "/" + sensorId + "/" + type + "/" + data + " HTTP/1.1");
          client.println("Host:"+host);
          client.println("Content-Length: 0");
          client.println("Connection: Keep-Alive");
          client.println();
      
		#if defined(DEBUG)
          Serial.println("Request sent.");
		  #endif
    } 
  
    else 
    {
		#if defined(DEBUG)
			Serial.println("certificate doesn't match");
	    #endif
    }
  }

  else
  {
	  #if defined(DEBUG)
		Serial.println("Server connect failed.");
	#endif
  }
}

void SensorCore::ReconfigureWifi()
{
    ssid = Serial.readStringUntil('\n');
    ssidPassword = Serial.readStringUntil('\n');
    InitializeWiFi();
}

void SensorCore::RefreshWiFiStatus()
{
  wifiStatus = WiFi.status();

  #if defined(DEBUG)
	  switch (wifiStatus)
	  {
		case WL_CONNECTED:
		  Serial.println("Connected.");
		  break;
		case WL_CONNECTION_LOST:
		  Serial.println("Connection Lost.");
		  break;
		case WL_DISCONNECTED:
		  Serial.println("Disconnected.");
		  break;
	  }
  #endif
  
  if (wifiStatus != WL_CONNECTED)
  {
    InitializeWiFi();
  }
}

void SensorCore::GetId()
{
	#if defined(DEBUG)
	Serial.println("Getting Sensor Id.");
	#endif

	Serial.write(String(sensorId).c_str());
	Serial.write('\n');

	#if defined(DEBUG)
		Serial.println("Sent ID: "+ sensorId);
	#endif
}

void SensorCore::SetApiKey()
{
	#if defined(DEBUG)
		Serial.println("Waiting for API key...");
	#endif

	apiKey = Serial.readStringUntil('\n');

	#if defined(DEBUG)
		Serial.println("Read in API key: " + apiKey);
	#endif

	Serial.write("Ok\n");
}

void SensorCore::ValidateSensor()
{
	#if defined(DEBUG)
		Serial.println("Validating Sensor...");
	#endif

		Serial.write("asuh dude\n");
}
