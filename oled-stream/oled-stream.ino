extern "C" {
  #include "user_interface.h"
}

#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ArduinoOTA.h>
#include <WiFiUdp.h>
#include <Hash.h>
#include "SSD1306.h"

#define BUFFER_LEN 512
#define SHOW_FPS false


const char* wifiSSID = "Glennemeyer-WLAN2";
const char* wifiPassword = "1692744545583678";
unsigned int localPort = 7777;
char* wifiHostname = "espscr1";

WiFiUDP port;
char packageBuffer[BUFFER_LEN];
int x = 0;
int y = 63;

// Initialize the OLED display using brzo_i2c
// D4 -> SDA
// D5 -> SCL
// SSD1306Brzo display(0x3c, D4, D5);
SSD1306  display(0x3c, D3, D4);

static unsigned int lastMillis;


void setup() {
  Serial.begin(115200);
  Serial.println("\n");

  // wifi
  wifiSetup();
  wifiConnect();

  // setup stuff
  otaSetup();
  socketSetup();
  oledSetup();
  
  lastMillis = millis();
}

void loop() {
  if (WiFi.status() != WL_CONNECTED) {
    Serial.println("Disconnected!");

    wifiConnect();
  }

  ArduinoOTA.handle();

  socketLoop();
}

void otaSetup() {
  // ArduinoOTA.setPort(8266); // Port defaults to 8266
  ArduinoOTA.setHostname(wifiHostname);
  // ArduinoOTA.setPassword((const char *)"123"); // No authentication by default

  ArduinoOTA.onStart([]() {
    Serial.println("OTA Start");
  });

  ArduinoOTA.onEnd([]() {
    Serial.println("\nOTA End");

    ESP.restart(); // better do also per hand
    delay(100);
  });

  ArduinoOTA.onProgress([](unsigned int progress, unsigned int total) {
    Serial.printf("OTA Progress: %u%%\n", (progress / (total / 100)));
  });

  ArduinoOTA.onError([](ota_error_t error) {
    Serial.printf("OTA Error[%u]: ", error);

    if (error == OTA_AUTH_ERROR) Serial.println("Auth Failed");
    else if (error == OTA_BEGIN_ERROR) Serial.println("Begin Failed");
    else if (error == OTA_CONNECT_ERROR) Serial.println("Connect Failed");
    else if (error == OTA_RECEIVE_ERROR) Serial.println("Receive Failed");
    else if (error == OTA_END_ERROR) Serial.println("End Failed");
  });

  ArduinoOTA.begin();
}

void wifiSetup() {
  WiFi.mode(WIFI_STA);

  // init wifi
  WiFi.begin(wifiSSID, wifiPassword);
  wifi_station_set_hostname(wifiHostname);
}

void socketSetup() {
  Serial.print("Setup Socket ...\n");
  port.begin(localPort);
  Serial.printf("Port: %u\n\n", localPort);
}

void oledSetup() {
  display.init();

  display.flipScreenVertically();
  display.setFont(ArialMT_Plain_10);
  
  display.clear();
  display.drawString(0, 10, "ready...");
  display.drawString(0, 30, "hostname: " + (String)wifiHostname);
  display.display();
}

void wifiConnect() {
  Serial.printf("Connecting to %s .", wifiSSID);

  // wait for successful connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }

  Serial.println("\nConnected!\n");

  // print ip & hostname
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  Serial.print("Hostname: ");
  Serial.println(wifiHostname);
  Serial.println();
}

void socketLoop() {
  int packageSize = port.parsePacket(); // read data from socket
  
  // if packages received, interpret the data
  if (packageSize) {
    int len = port.read(packageBuffer, BUFFER_LEN);
    
    if(packageBuffer[0] == 'a') { // new frame byte
      x = 0;
      y = 63;
      
      packageBuffer[BUFFER_LEN];
    } else {
      drawBuffer();
    }    
  }
}

void drawBuffer() {
  for(int i = 0; i < BUFFER_LEN; i++) {
    if(SHOW_FPS && y < 12 && x < 45) display.setColor(BLACK); // black rect as background for fps 
    else if(packageBuffer[i] == '1') display.setColor(WHITE);
    else display.setColor(BLACK);

    display.setPixel(x, y);

    x++;
    
    if(x == 128) { // next col
      x = 0;
      y--;

      if(y < 0) { // frame complete
        y = 63;

        if(SHOW_FPS) {
          int fps = 1000 / (millis() - lastMillis);
          lastMillis = millis();
        
          display.setColor(WHITE);
          display.drawString(0, 0, "FPS: " + (String)fps);
        }
        
        display.display();
      }
    }
  }
}
