#include <ESP8266WiFi.h>

class SensorCore
{
  public:
  SensorCore(int id);

  bool Initialize();
  void SendData(String data, String type);
  void ReconfigureWifi();

  private:
  
  void RefreshWiFiStatus();
  void InitializeWiFi();

  String ssid;
  String ssidPassword;
  int wifiStatus = WL_IDLE_STATUS;
  int sensorId;
  String apiKey = "50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce";
};

