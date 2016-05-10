
#include "I2Cdev.h"

#include "MPU6050_6Axis_MotionApps20.h"
#include "Wire.h"

#include <MsTimer2.h>

MPU6050 mpu;

#define INTERRUPT_PIN 2  // use pin 2 on Arduino Uno & most boards

bool dmpReady = false;  // set true if DMP init was successful
uint8_t mpuIntStatus;   // holds actual interrupt status byte from MPU
uint8_t devStatus;      // return status after each device operation (0 = success, !0 = error)
uint16_t packetSize;    // expected DMP packet size (default is 42 bytes)
uint16_t fifoCount;     // count of all bytes currently in FIFO
uint8_t fifoBuffer[64]; // FIFO storage buffer

const uint8_t IMUAddress = 0x69;
const uint16_t I2C_TIMEOUT = 1000; // Used to check for errors in I2C communication

// orientation/motion vars
double fing[3];
Quaternion q;           // [w, x, y, z]         quaternion container
VectorInt16 aa;         // [x, y, z]            accel sensor measurements
VectorInt16 aa_prev;
VectorInt16 aaReal;     // [x, y, z]            gravity-free accel sensor measurements
VectorInt16 aaWorld;    // [x, y, z]            world-frame accel sensor measurements
VectorFloat gravity;    // [x, y, z]            gravity vector

volatile bool mpuInterrupt = false;     // indicates whether MPU interrupt pin has gone high
void dmpDataReady() {
    mpuInterrupt = true;
}

void setup() {
    Wire.begin();
    Wire.setClock(400000); // 400kHz I2C clock. Comment this line if having compilation difficulties
    TWBR = ((F_CPU / 400000L) - 16) / 2; // Set I2C frequency to 400kHz
    Serial.begin(115200);
    while (!Serial); // wait for Leonardo enumeration, others continue immediately

    Serial.println(F("Initializing I2C devices..."));
    mpu.initialize();
    pinMode(INTERRUPT_PIN, INPUT);

   Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));
    devStatus = mpu.dmpInitialize();

    // make sure it worked (returns 0 if so)
    if (devStatus == 0) {
        // turn on the DMP, now that it's ready
        Serial.println(F("Enabling DMP..."));
        mpu.setDMPEnabled(true);

        // enable Arduino interrupt detection
        Serial.println(F("Enabling interrupt detection (Arduino external interrupt 0)..."));
        //attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), dmpDataReady, RISING);
        mpuIntStatus = mpu.getIntStatus();

        dmpReady = true;

        // get expected DMP packet size for later comparison
        packetSize = mpu.dmpGetFIFOPacketSize();
    } else {
        // ERROR!
        // 1 = initial memory load failed
        // 2 = DMP configuration updates failed
        // (if it's going to break, usually the code will be 1)
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
    }

    MsTimer2::set (400, serialHandler);
    MsTimer2::start();
}


void loop() {
  fingerWrite();
  gyWrite();
}

void serialHandler () {
  
  for (int i; i < 3; i++) {
    Serial.print(fing[i]);
    Serial.print("\t");
  }

  Serial.print(q.w);
  Serial.print("\t");
  Serial.print(q.x);
  Serial.print("\t");
  Serial.print(q.y);
  Serial.print("\t");
  Serial.print(q.z);
  
  Serial.print("\t");
  Serial.print((aaWorld.x-aa_prev.x)/100);
  Serial.print("\t");
  Serial.print((aaWorld.y-aa_prev.y)/100);
  Serial.print("\t");
  Serial.print((aaWorld.z-aa_prev.z)/100);
  Serial.println();
    
  aa_prev = aaWorld;
        
  dmpReady = true;  // set true if DMP init was successful
}

void fingerWrite() {
  int iValue[3];
  double dV[3];
  double dR[3] = {-1.0, -1.0 -1.0};
  int i;
  for (i=0; i < 3; i++) {
    iValue[i] = analogRead(i);

    dV[i] = iValue[i] * 5.0 / 1023;

    if (0.005 < (5.0 - dV[i])) {
      dR[i] = 10 * 1000 * dV[i] / (5.0 - dV[i]);
    }

    //set parametor
    fing[i] = (dR[i]-30000) / 1000;


    }
}

void gyWrite(){
   // if programming failed, don't try to do anything
    if (!dmpReady) return;


    // reset interrupt flag and get INT_STATUS byte
    mpuInterrupt = false;
    mpuIntStatus = mpu.getIntStatus();

    // get current FIFO count
    fifoCount = mpu.getFIFOCount();

    // check for overflow (this should never happen unless our code is too inefficient)
    if ((mpuIntStatus & 0x10) || fifoCount == 1024) {
        // reset so we can continue cleanly
        Serial.println(F("FIFO overflow!"));
        mpu.resetFIFO();
    } else if (mpuIntStatus & 0x02) {
        while (fifoCount < packetSize) fifoCount = mpu.getFIFOCount();
        mpu.getFIFOBytes(fifoBuffer, packetSize);
        
        fifoCount -= packetSize;

        mpu.dmpGetQuaternion(&q, fifoBuffer);
        
        mpu.dmpGetAccel(&aa, fifoBuffer);
        mpu.dmpGetGravity(&gravity, &q);
        mpu.dmpGetLinearAccel(&aaReal, &aa, &gravity);
        mpu.dmpGetLinearAccelInWorld(&aaWorld, &aaReal, &q);
  Wire.beginTransmission(IMUAddress);
    }
}

