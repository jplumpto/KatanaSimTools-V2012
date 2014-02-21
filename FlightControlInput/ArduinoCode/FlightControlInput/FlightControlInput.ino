/*
  Mega analogWrite() test
 	
  This sketch fades LEDs up and down one at a time on digital pins 2 through 13.  
  This sketch was written for the Arduino Mega, and will not work on previous boards.
 	
  The circuit:
  * LEDs attached from pins 2 through 13 to ground.

  created 8 Feb 2009
  by Tom Igoe
  
  This example code is in the public domain.
  
 */

const int pitchPin = 12;
const int rollPin = 11;
const int yawPin = 10;

const int EXPECTED_ARRAY_SIZE = 5;

uint8_t rcvArray[5];
int arraySize = 0;
int ptr = 0;    
int checksum = 0;

void updatePWM();

void setup() {
  // set pins as outputs:
  pinMode(pitchPin, OUTPUT); 
  pinMode(rollPin, OUTPUT); 
  pinMode(yawPin, OUTPUT); 
  
  Serial.begin(38400);
}

void loop() {
  
  if (Serial.available() > 0)
  {
    
    
    while (Serial.available() > 0 && arraySize < EXPECTED_ARRAY_SIZE)
    {
      if (arraySize == 0) 
      {
        if (Serial.peek() == (byte)0xAA)
        {
          Serial.read();
          ptr = 0;
          checksum = 0;
          arraySize++;
        }
        else
        {
          byte temp = Serial.read();
          //Serial.print("Wrong header: ");
          //Serial.println(temp,DEC);
          arraySize = 0;
         }
      } //header
      else if (arraySize == EXPECTED_ARRAY_SIZE - 1)
      {
        if (Serial.read() == (checksum % 256) )
        {
          updatePWM();
          //Serial.println("Correct format");
        }
        else
        {
          //Serial.println("Incorrect format");
        }
        
        arraySize = 0;
        ptr = 0;
        checksum = 0;
        
      } //checksum
      else
      {
        rcvArray[ptr] = Serial.read();
        checksum += rcvArray[ptr];
        arraySize++;
        ptr++;
      } //values
      
    } //While
    
  } // if
  
}


void updatePWM()
{
  analogWrite(rollPin,rcvArray[0]);
  analogWrite(pitchPin,rcvArray[1]);
  analogWrite(yawPin,rcvArray[2]);
  
}
