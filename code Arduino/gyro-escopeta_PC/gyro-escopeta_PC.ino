//SDA - Pin A4
//SCL - Pin A5
#include "I2Cdev.h"
#include "MPU6050.h"
#include "Wire.h"
const int mpuAddress = 0x68;  // Puede ser 0x68 o 0x69
MPU6050 mpu(mpuAddress);
int ax, ay, az;
int gx, gy, gz;
long tiempo_prev;
float dt;
float ang_x, ang_y;
float ang_x_prev, ang_y_prev;
void updateFiltered()
{
   dt = (millis() - tiempo_prev) / 1000.0;
   tiempo_prev = millis();
 
   //Calcular los ángulos con acelerometro
   float accel_ang_x = atan(ay / sqrt(pow(ax, 2) + pow(az, 2)))*(180.0 / 3.14);
   float accel_ang_y = atan(-ax / sqrt(pow(ay, 2) + pow(az, 2)))*(180.0 / 3.14);
 
   //Calcular angulo de rotación con giroscopio y filtro complementario
   ang_x = 0.98*(ang_x_prev + (gx / 131)*dt) + 0.02*accel_ang_x;
   ang_y = 0.98*(ang_y_prev + (gy / 131)*dt) + 0.02*accel_ang_y;
 
   ang_x_prev = ang_x;
   ang_y_prev = ang_y;
}
 
void setup()
{
   //comunicacion con Pc
   Serial.begin(9600);
   //giroscopio
   Wire.begin();
   mpu.initialize();
   Serial.println(mpu.testConnection() ? F("IMU iniciado correctamente") : F("Error al iniciar IMU"));
   //boton gatillo
   pinMode(10,INPUT);
}
 
void loop() 
{
   // Leer las aceleraciones y velocidades angulares
   mpu.getAcceleration(&ax, &ay, &az);
   mpu.getRotation(&gx, &gy, &gz);
   updateFiltered();

   //boton gatillo
   int aaa=0;
   if(digitalRead(10))
    aaa=1;

   //enviar datos
   Serial.print(ang_x);
   Serial.print(",");
   Serial.print(ang_y);
   Serial.print(",");
   Serial.println(aaa);
   Serial.flush();
   delay(25);
}
