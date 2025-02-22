//Kunal Mukherjee
//10/9/10
#include <at89c51cc03.h>
#include <stdio.h>
#include <stdlib.h>

//*Global functions	

//*Global functions for hardware code
void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state);
unsigned char GetLedStatus(unsigned char row,
                           unsigned char column);
void DoLED(unsigned char r, unsigned char c);
unsigned char getUpDn (void);
void makeInitialGenUp(void);
void makeInitialGenDown(void);
unsigned char checkStatus(unsigned char row,
                        unsigned char col);
void OutputEncoder(unsigned char rfRow);
void makeHrGlass(void);
//external assembly lanaguge program
extern int Assem(); //return 0
	
//*initialize global constants
unsigned char down = 0;
unsigned char up = 1;
unsigned char flat = 2;

//*initialize global variables
unsigned char LED[16];
unsigned char refreshRow = 0; //row to be refreshed

int main()
{
  //*Main code

	//*local variable  
	int i, ledRow, ledCol;

	//*Initialize timer 0 interrupt
	//set the clock control register to double clock
	CKCON = 0x01; 
	//set up Timer 0 in the 16-bit auto-reload mode, 
	//not gated, with an internal clock
	TMOD = 0x01;
	//calculate value for timer zero 
	//refresh rate 2 ms = 2000 us
	//2000/.2127 = 9402 counts
	//65536-9402 = 56134 counts
	//56134 = 0xDB46
	TH0 = 0xDB;
	TL0 = 0x46;
	
	//*Set up timer 0 for multplexing	
	//enable T0 interrupt and the global interrupt
	TR0 = 1;
	ET0 = 1; 
	EA =1;  
	
	//*Set up A/D converter for Tilt sensor
	ADCF  = 0x01 ; //P1.0 = ADC[0]
	ADCON = 0X20; //enable ADC Function
	ADCLK = 0X00; //Prescalar to 0
	
	//*Initialize varibales	
	
	//*Clear LED memeory map
   for (i = 0; i <16; i++)
	{
		LED[i] = 0x00;
	}

	//Place hour glass outlinne in memory map
	if (getUpDn() == down)
       makeInitialGenUp();
   else
       makeInitialGenDown();
	 
  //*Place hour glass outlinne in memory map
	makeHrGlass();
	 
	 //*Start timer for multiplexing display
	//turn on timer 0
	TR0 = 1;

	//*main program loop
	while(1) //user != 100
    {			
			if(getUpDn() == down) //Start at top and move down
			{
					for (ledRow = 15; ledRow >= Assem(); ledRow--)
				{
					for (ledCol = 7; ledCol >= Assem(); ledCol--)
					{
						DoLED(ledRow,ledCol);
					}
				}
			}
			else //STart at bottom and move up
			{
				for (ledRow = Assem(); ledRow < 16; ledRow++)
				{
					for (ledCol = Assem(); ledCol < 8; ledCol++)
					{
						DoLED(ledRow, ledCol);
					}
				}
			}
			
			while(P1_5 == Assem()); //if stopped wait here user, pause  feature
			                        //P1_5 = 0
					
			if(P1_4 == Assem()) //P1_4 = 0 //reset feature
				{
					//*Clear LED memeory map
						 for (i = Assem(); i <16; i++)
						{
							LED[i] = 0x00;
						}	
						
					//*Place hour glass outlinne in memory map
						if (getUpDn() == down)
						{
							makeInitialGenUp();
						}							
						else if(getUpDn() == up)
						{
							makeInitialGenDown();   
						}		

						makeHrGlass();
				}                                           
    } 		
		
}

void DoLED(unsigned char r, unsigned char c)
	{

		//printf("r: %i c: %i status:%i\n", r,c, GetLedStatus(r,c));
		if(getUpDn() != flat)
		{
			if (getUpDn()== down)
			{
				//look at the bottom LED
				//Move LED at (r,c) accordign to algirithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r+1 < 16) &&
                    (GetLedStatus(r+1,c) == 0) &&
                    (checkStatus(r+1,c) != 0))
				{
					SetLed(r+1, c, 1);
					SetLed(r  , c, 0);
				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r+1 < 16) &&
                        (c-1 >= 0) &&
                        (GetLedStatus(r+1,c - 1) == 0) &&
                        (checkStatus(r+1,c - 1) != 0))
					{
						SetLed(r+1, c - 1, 1);
						SetLed(r  , c, 0);
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r+1 < 16)&&
                            (c+1 <= 7)&&
                            (GetLedStatus(r+1,c + 1) == 0) &&
                            (checkStatus(r+1,c + 1) != 0))
					{
						SetLed(r+1, c + 1, 1);
						SetLed(r  , c, 0);
					}
				}
			}
			else
			{
				//look at the top LED
				//Move LED at (r,c) accordign to algirithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r-1 >= 0) &&
                    (GetLedStatus(r-1,c) == 0) &&
                    (checkStatus(r-1,c) != 0))
				{
					SetLed(r-1, c, 1);
					SetLed(r  , c, 0);
				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r-1 >= 0) &&
                        (c-1 >= 0) &&
                        (GetLedStatus(r-1,c - 1) == 0) &&
                        (checkStatus(r-1,c - 1) != 0))
					{
						SetLed(r-1, c - 1, 1);
						SetLed(r  , c, 0);
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r-1 >= 0) &&
                            (c+1 <= 7) &&
                            (GetLedStatus(r-1,c + 1) == 0) &&
                            (checkStatus(r-1,c+1) != 0))
					{
						SetLed(r-1, c + 1, 1);
						SetLed(r  , c, 0);
					}
				}
			}
		}
	}

//gets the led status of the row and col
unsigned char GetLedStatus(unsigned char row,
                           unsigned char column)
{
	unsigned char temp = LED[row];

	return temp & (1 << column);
}

//sets the led at the point
void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state)
{
	if(state)
		LED[row] |= (1 << column);
	else
		LED[row] &= ~(1 << column);
}

//looks at the accelerometer and gives a result out
unsigned char getUpDn (void)
{
    unsigned char tmp;
		int i,result;	
		
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
		
		ADCON &= 0xEF; //clear ADEOC = 0	
		
		for (i = 0; i < 33; i++);
		
		//return down;
		if (result > 535)
			return down;
		else if (result < 380)
			return up;
		else
			return flat;
}

//generates the hour glass
void makeHrGlass(void)
	{

		SetLed(5,7, 0);
		SetLed(6,7, 0);
		SetLed(7,7, 0);
		SetLed(8,7, 0);
		SetLed(9,7, 0);
		SetLed(10,7, 0);

		SetLed(5,0, 0);
		SetLed(6,0, 0);
		SetLed(7,0, 0);
		SetLed(8,0, 0);
		SetLed(9,0, 0);
		SetLed(10,0, 0);

		SetLed(6,6, 0);
		SetLed(7,6, 0);
		SetLed(8,6, 0);
		SetLed(9,6, 0);

		SetLed(6,1, 0);
		SetLed(7,1, 0);
		SetLed(8,1, 0);
		SetLed(9,1, 0);

		SetLed(7,5, 0);
		SetLed(8,5, 0);

		SetLed(7,2, 0);
		SetLed(8,2, 0);
	}
	
		///check to see if the location to go, is 
	//part of the hour glass or not
unsigned char checkStatus(unsigned char row,
                        unsigned char col)
{
   if ((row == 5) || (row == 6) || (row == 7) ||
       (row == 8) || (row == 9) || (row == 10))
   {
       if((col == 0) || (col == 7))
       {
           return 0;
       }
   }

   if  ((row == 6) || (row == 7) ||
        (row == 8) || (row == 9))
    {
        if((col == 6) || (col == 1))
       {
           return 0;
       }
    }

    if((row == 7) ||
        (row == 8))
    {
        if((col == 5) || (col == 2))
       {
           return 0;
       }
    }

    return 1;

}

//Makes the initial generation for Up
void makeInitialGenUp(void)
{
    int i , j;
    for (i = 0; i < 8; i++)
    {
        for (j = 0; j < 7; j++)
        {
            SetLed(j,i,1);
        }
    }
}

//Makes the initial generation for Down
void makeInitialGenDown(void)
{
    int i , j;
    for (i = 0; i < 8; i++)
    {
        for (j = 15; j > 8; j--)
        {
            SetLed(j,i,1);
        }
    }
}


	//*Timer 0 multiplexor interrupt
	//Convert refreshRow to port bits for decoders
	//Make decoders active
	//Send d to the 74LS244 port
	void T0Int() interrupt 1 using 1
	{
		unsigned char d;
		//*reload the timer value
		//calculate value for timer zero 
		//refresh rate 2 ms = 2000 us
		//2000/.2127 = 9402 counts
		//65536-9402 = 56134 counts
		//56134 = 0xDB46
		TH0 = 0xDB;
		TL0 = 0x46;
		
		//* Get the data d = LED[refreshRow]
		d = LED[refreshRow];
		//Send d to the 74LS244 port	
		P3 = d;
		
		//convert refreshRow to port bits for decoder
		OutputEncoder(refreshRow);		
		
		//Update refreshRow
		refreshRow++;
		if(refreshRow == 16)
			refreshRow = 0;
	}
	
	// Assume we have two 3x8 decoders, 74LS138
	//both of which are connected to P1.4, P1.5, P1.7, 
	//with their enable pins being (P0.1, and P0.2)

	void OutputEncoder(unsigned char rfRow)
	{
		// Handle choosing the right encoder based on MSB
		P0_2 = P0_3 = 0;
		if((rfRow & (1 << 3)) == 0)		
			P0_3 = 1;		
		else
			P0_2  = 1;
		
		// Output the least significan 3 bits from 
		//the input number onto the encoders input
		P0_7 = ((rfRow >> 2) & 1);
		P0_6 = ((rfRow >> 1) & 1);
		P0_5 = ((rfRow >> 0) & 1);
		
	}
	
