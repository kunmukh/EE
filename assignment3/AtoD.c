//Kunal Mukherjee
//9/27/2018
//AtoD.c
//Write a program in C which will input 2 bits from port 
//P0.0 and P0.1. Take P0.1 to be the MSB. 
//If these two bits are 00 output 0 on the D to A converter. 
//If they are 01 output the saw tooth waveform in the top
// figure below. If they are 10 output the triangular waveform 
//in the middle figure below and if they are 11 output the 
//square wave in the bottom figure below. 

#include <at89c51cc03.h>

void main (void)
{
	//the variables
	unsigned int count;	
	unsigned char option = 3;	
	unsigned char toggle_saw = 1;	
	unsigned char toggle_pulse = 1;	
	unsigned char pulse_count = 0;
	unsigned int i;

	count = 0;
	toggle_pulse = 0;
	toggle_saw = 0;
	
	while(1)
	{
		//if 00 is selected option 0
		if (P0_1 == 0 && P0_0 == 0)
		{
			option = 0;
		}

		//if 10 is selected option 0
		else if (P0_1 == 0 && P0_0 == 1)
		{
			option = 1;
		}
		//if 11 is selected option 0
		else if (P0_1 == 1 && P0_0 == 0)
		{
			option = 2;
		}
		//default
		else if (P0_1 == 1 && P0_0 == 1)
		{
			option = 3;			
		}

		//sawtooth waveform
		if(option == 1)
		{
			count++;
			if (count >= 511)
				count = 0;
		}
		//pos sawtooth and neg sawtooth wav
		if(option == 2)
		{
			if(toggle_saw == 0)
			{
				count++;
				if(count >= 255) //if pos edge done, change to negative
				{
					toggle_saw = 1;
				}
			}
			else
			{
				count--;
				if(count == 0) //if neg edge done chnage to positve
				{
					toggle_saw = 0;
				}
			}
		}
		//a pulse
		if(option == 3)
		{
			if(toggle_pulse == 0)
			{
				count = 255;
				pulse_count++;
				if(pulse_count >= 255) //generate a pulse with a same frequency
				{
					toggle_pulse = 1;
				}
			}
			else
			{
				count = 0;
				pulse_count--;
				if(pulse_count == 0)
				{
					toggle_pulse = 0;
				}
			}		
		}
		
		//default
		if(option == 0)
		{
			count = 0;
		}
		
		if(option == 1)
			P2 = (unsigned char)(count / 2);
		else
			P2 = (unsigned char)(count);
		
		P4_0 = 0; 
		P4_0 = 1;
		
		// Delay
		for(i=0;i<73;i++){}
			
		
	}
}