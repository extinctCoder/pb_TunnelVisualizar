#include <ESP8266WiFi.h>
#include <Arduino.h> 
const char* ssid     = "seraphGetway";
const char* password = "webGetway";
const char* host = "192.168.0.106";
const int httpPort = 80;
int iddata = 1;
int data_waterFlow = -1;
int data_waterlavel = -1;
const int sensor_pin = D2;
byte sensorInterrupt = 0;
float calibrationFactor = 4.5;
volatile byte pulseCount;
float flowRate;
unsigned long oldTime;
unsigned int flowMilliLitres;
unsigned long totalMilliLitres;
int value = 0;
void setup() {
  Serial.begin(115200);
  delay(10);
  Serial.println();
  Serial.println();
  Serial.println("pb_TunnelVisualizer_System initialiazing ...");
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  pinMode(sensor_pin, INPUT);
  pulseCount = 0;
  flowRate = 0.0;
  oldTime = 0;
  digitalWrite(sensor_pin, HIGH);
  attachInterrupt(digitalPinToInterrupt(sensor_pin), pulseCounter, RISING);
}
void loop() {
  delay(1000);
  ++value;
  Serial.print("connecting to ");
  Serial.println(host);
  WiFiClient client;
  if (!client.connect(host, httpPort)) {
    Serial.println("connection failed");
    return;
  }
  else{
    Serial.println("connection successfull");
    }
  updateSensorData();
  String url = "/index.php?iddata=";
  url += iddata;
  url += "&data_waterFlow=";
  url += data_waterFlow;
  url += "&data_waterlavel=";
  url += data_waterlavel;
  Serial.print("Requesting URL: ");
  Serial.println(url);
  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" + 
               "Connection: close\r\n\r\n");
  unsigned long timeout = millis();
  while (client.available() == 0) {
    if (millis() - timeout > 5000) {
      Serial.println(">>> Client Timeout !");
      client.stop();
      return;
    }
  }
  while(client.available()){
    String line = client.readStringUntil('\r');
    Serial.print(line);
  }
  Serial.println();
  Serial.println("closing connection");
}
void pulseCounter() {
    pulseCount++;
}
void updateSensorData(){
  if (WiFi.status() == WL_CONNECTED && (millis() - oldTime) > 1000)
    {
        Serial.print("data updating : ");
        detachInterrupt(sensorInterrupt);
        flowRate = ((1000.0 / (millis() - oldTime)) * pulseCount);
        oldTime = millis();
        flowMilliLitres = (flowRate / 60) * 1000;
        totalMilliLitres += flowMilliLitres;
        data_waterFlow = flowRate;
        pulseCount = 0;
        attachInterrupt(sensorInterrupt, pulseCounter, FALLING);
    }
}
