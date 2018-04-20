#include <Wire.h>

//Direccion I2C de la IMU
const int  MPU =0x68;

//Ratios de conversion 
#define A_R 16384.0
#define G_R 131.0

//Conversion de radianes a grados 180/PI
#define RAD_A_DEG = 57.295779

//MPU-6050 da los valores en enteros de 16 bits
//Valores sin refinar
int16_t AcX, AcY, AcZ,GyX, GyY, GyZ;

//Angulos
float Acc[3];
float Gy[3];

float Angle1;
float Angle2;
float Angle3;

//Variable joystick
int xPin = A0;
int yPin = A1;
int Pulsador = 2;

int xPosicion = 0;
int yPosicion = 0;
int EstadoPulsador =0;

void setup() {
  // put your setup code here, to run once:
  //Inicio setup MPU
  Wire.begin();
  Wire.beginTransmission(MPU);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
  Serial.begin(9600);
  //Fin setup MPU
 
//Joystick
 //Serial.begin(9600);
  pinMode(xPin, INPUT);
  pinMode(yPin, INPUT);

  pinMode(Pulsador, INPUT);
  digitalWrite(Pulsador, HIGH);
  //fin setup Joystick
}

void loop() {

  //Inicio Lopp MPU
  // put your main code here, to run repeatedly:
  //Leer los valores del Acelerometro de la IMU
  Wire.beginTransmission(MPU);
  Wire.write(0x3B);//Pedir el registro 0x3B - corresponde al AcX
  Wire.endTransmission(false);
  Wire.requestFrom(MPU,6,true);//A parti del 0x3B, se piden 6 registros
  AcX=Wire.read()<<8|Wire.read();//Cada valor ocupa 2 registros
  AcY=Wire.read()<<8|Wire.read();
  AcZ=Wire.read()<<8|Wire.read();

  //A partir de los valores del acelerometro se calculan los angulosY,X
  //respectivamente con la formula de la tangente
  Acc[0] =atan((AcX/A_R)    /sqrt(pow((AcY/A_R),2) + pow((AcZ/A_R),2))) * RAD_TO_DEG;
  Acc[1] =atan((AcY/A_R)    /sqrt(pow((AcX/A_R),2) + pow((AcZ/A_R),2))) * RAD_TO_DEG;
  //Acc[2] =atan((AcZ/A_R)    /sqrt(pow((AcX/A_R),2) + pow((AcY/A_R),2))) * RAD_TO_DEG;

  //Leer los valores del Giroscopio
  Wire.beginTransmission(MPU);
  Wire.write(0x43);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU,4,true);//A diferencia del acelerometro solo se piden 4 registros
  GyX =Wire.read()<<8|Wire.read();
  GyY =Wire.read()<<8|Wire.read();
  //GyZ =Wire.read()<<8|Wire.read();

  //Calculo de angulos
  Gy[0] = GyX/G_R;
  Gy[1] = GyY/G_R;
  //Gy[2] = GyZ/G_R;

  //Aplicar el filtro Complementario
  Angle1 = 0.98*(Angle1+Gy[0]*0.010) + 0.02*Acc[0];
  Angle2 = 0.98*(Angle2+Gy[1]*0.010) + 0.02*Acc[1];
  //Angle3 = 0.98*(Angle3+Gy[2]*0.010) + 0.02*Acc[2];

  //Mostrar los valores por consola
  //Serial.print("Angle x:"); Serial.print(Angle1); Serial.print("\n");
  //Serial.print("Angle y: "); Serial.print(Angle2); Serial.print("\n-------------\n");

  //Serial.print(Angle1);Serial.print(",");Serial.print(Angle2);Serial.print("\n");
  

  //delay(10);//Nuestra dt sera pues 0.010 que es el intervalo de tiempo en cada medida
  //Fin Loop MPU

  //Inicio Loop Joystick
  xPosicion = analogRead(xPin);

  yPosicion = analogRead(yPin);

  EstadoPulsador = digitalRead(Pulsador);

  //Serial.println(String(Angle1)+","+String(Angle2)+","+String(Angle3)+","+String(xPosicion)+","+String(yPosicion)+","+(EstadoPulsador));

  Serial.println(String(Angle1)+","+String(Angle2));
}
