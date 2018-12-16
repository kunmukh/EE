//Kunal Mukherjee
//10/8/2018
//Accel.c
//Write a program in C which will input an analog value 
//from the x-output of the ADXL335 accelerometer. 
//Connect four LEDs to your board as shown in the figure below.
//The x-output puts out a voltage from 0 to 3.3 volts that 
//is somewhat proportional to the amount of tilt along 
//the x-axis. Your C program should indicate this tilt 
//by lighting up one of the four LEDs to indicate full
//left to full right.

#include <at89c51cc03.h>

#define FIRST_LED  414
#define SECOND_LED 459
#define THIRD_LED  503
#define FORTH_LED  548


void turn_led(int num){
	P1_1 = 1;
	P1_2 = 1;
	P1_3 = 1;
	P1_4 = 1;

	switch(num){
		case 1:
			P1_1 = 0;
			break;

		case 2:
			P1_2 = 0;
			break;
		
		case 3:
			P1_3 = 0;
			break;

		case 4:
			P1_4 = 0;
			break;
		
		default:
			break;
	}
}

void main(void)
{
	unsigned char tmp;
	int i, result;
	double x;
	
  ADCF = 0x01; // P1.0 = ADC[0]
  ADCON = 0x20; // Enable ADC Function
  ADCLK = 0x00; // Prescaler to 0
  EA = 0; //Turn off interrupts
	
	result = 0;
	x = 0;
	i = 0;
	
	while(1)
	{		
    ADCON &= 0xF8; // Reset ADC Channel Select
    ADCON |= 0x00; // Select ADC = Ch0
    ADCON |= 0x20; // Use Standard mode
    ADCON |= 0x08; // Start ADC Convert
		
    tmp = (ADCON & 0x10); // Get done bit
    while(tmp != 0x10) // Loop until complete
			tmp = (ADCON & 0x10);
			
		result = ADDH; // Send 8 MSB to P2
		result *= 4;
		result += ADDL;
	
		if (result < FIRST_LED - 45)
			turn_led(5);
		else if (result < FIRST_LED)
			turn_led(1);
		else if (result < SECOND_LED)
			turn_led(2);
		else if (result < THIRD_LED)
			turn_led(3);
		else if (result < FORTH_LED)
			turn_led(4);
		else
			turn_led(5);
		
		ADCON &= 0xEF; //clear ADEOC = 0	
		
		for (i = 0; i < 33; i++);
	}
}