//Kunal Mukherjee
//Project 2
//11/24/18
#include "stm32f446.h"
#include <stdio.h>
#include <stdlib.h>

/*stm446Template.c July 1, 2017
*/

//*Global functions	

//*Global functions for hardware code
void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state);
unsigned char GetLedStatus(unsigned char row,
                           unsigned char column);
void DoLED1(unsigned char r, unsigned char c);
void DoLED2(unsigned char r, unsigned char c);
void DoLED3(unsigned char r, unsigned char c);
unsigned char getUpDn (void);
void makeInitialGenUp(void);
void makeInitialGenDown(void);
void makeInitialGenUpWater(void);
void makeInitialGenDownWater(void);
unsigned char checkStatus(	unsigned char row,
							unsigned char col);
unsigned char checkStatus1(	unsigned char row,
							unsigned char col);
int getPachinokoComplete();
void makeHrGlass(void);
void makeWaterGlass(void);
void OutputEncoder(unsigned char rfRow);

//*initialize global constants
unsigned char down = 0;
unsigned char up = 1;
unsigned char flat = 2;
int pachinkoRoundflag = 1;

unsigned char LED[16];
unsigned char refreshRow = 0; //row to be refreshed


int main()
 {
	//temporary variables initialization
	int i, result, ledRow, ledCol;
	
	//Enabling Clock bits
	RCC_AHB1ENR |= 1;     //Bit 0 is GPIOA clock enable bit
	RCC_AHB1ENR |= 4;     //Bit 3 is GPIOC clock enable bit
	
	//I/O bits
	GPIOA_MODER &= 0;
	GPIOC_MODER &= 0;
	
	GPIOA_MODER |= 0x55555C00;	//Bits 0-4 for Input, 
								//5 for analog mode, 6-15 for output 							
	GPIOC_MODER |= 0x15;   	//Bits 0-3 for digitial output 
							
							
	//OTYPER register resets to 0 so it is push/pull by default
	GPIOA_OSPEEDER |= 0xFFFFFFFF;  	//Bits 0-15 for high speed on PA 1													 
	GPIOC_OSPEEDER |= 0x3F; 		//Bits 0-3 for high speed on PA 3
	 
	//Accelerometer setup
	RCC_APB2ENR |= 0x100;      //Bit 8 is ADC 1 clock enable bit
	GPIOA_MODER |= 0xC00;      //PA5 are analog
	GPIOA_PUPDR &= 0xFFFFF3FF; //Pins P5 are no pull up and pull down
	
	ADC1_CR2 |= 1;             //Bit 0 turn ADC on
	ADC1_CR2 |= 0x400;         //Bit 10 allows EOC to be set after conversion
	ADC_CCR |= 0x30000;        //Bits 16 and 17 = 11 so clock divided by 8
	ADC1_SQR3 |= 0x5;          //Bits 4:0 are channel number for first conversion
	// Channel is set to 5 which corresponds to PA5
	

	//Interrupt bits
	NVICISER0 |= (1 << 28); //Bit 28 in ISER0 corresponds to int 28 (TIM 2)
	TIM2_DIER |= 1; //Enable Timer 2 update interrupt enable
	TIM2_DIER |= (1 << 6); //Enable Timer 2 trigger interrupt enable
	
	//Timer 2 bits
	TIM2_CR1 |= (1 << 7); //Auto reload is buffered
	TIM2_PSC = 0; //Don't use prescaling
	TIM2_ARR = 32000; //16MHz/32000 = 500 Hz
	TIM2_CR1 |= 1; //Enable Timer 2
	TIM2_EGR |= 1;

	//*Initialize varibales	
	
	//*Clear LED memeory map
   for (i = 0; i <16; i++)
	{
		LED[i] = 0x00;
	}
	
	 //Main program loop	 
	 while(1)
	 {		 	
		//if A0 == 1 then Algorithm 1
		if (GPIOA_IDR &= (1 << 0) == 1)
		{
			if(getUpDn() == down) //Start at top and move down
            {
                for (ledRow = 15; ledRow >= 0; ledRow--)
                {
                    for (ledCol = 0; ledCol <8; ledCol++)
                    {
                        DoLED1(ledRow,ledCol);
                    }
                }
            }
            else //STart at bottom and move up
            {
                for (ledRow = 0; ledRow < 16; ledRow++)
                {
                    for (ledCol = 0; ledCol < 8; ledCol++)
                    {
                        DoLED1(ledRow, ledCol);
                    }
                }
            }
			//if stopped wait here user, pause  feature
			//A4 = 0
			while(GPIOA_IDR &= (1 << 4) == 1);
			
			//A3 = 0 //reset feature
			if (GPIOA_IDR |= (1 << 3) == 1)
			{
				//*Clear LED memeory map
				for (i = 0; i <16; i++)
				{
					LED[i] = 0x00;
				}
				
				//*Place hour glass outlinne in memory map
				if (getUpDn() == down)
					makeInitialGenUp();
				else
					makeInitialGenDown();
				
				makeHrGlass();
			}
		}
		//if A1 == 1 then Algorithm 2
		else if (GPIOA_IDR &= (1 << 1) == 1)
		{
			if(getUpDn() == down) //Start at top and move down
                {
                    randCol = (int) rand() % 8;
                    if(pachinkoRoundflag == 1)
                    {
                        SetLed(0, randCol, 1);
                        printHRglass();
                        pachinkoRoundflag = 0;
                    }

                    for (ledRow = 15; ledRow >= 0; ledRow--)
                    {
                        for (ledCol = 0; ledCol <8; ledCol++)
                        {
                            DoLED2(ledRow,ledCol);
                        }
                    }
                }
                else //STart at bottom and move up
                {
                    randCol = (int) rand() % 8;
                    if(pachinkoRoundflag == 1)
                    {
                        SetLed(15, randCol, 1);
                        printHRglass();
                        pachinkoRoundflag = 0;
                    }

                    for (ledRow = 0; ledRow < 16; ledRow++)
                    {
                        for (ledCol = 0; ledCol < 8; ledCol++)
                        {
                            DoLED2(ledRow, ledCol);
                        }
                    }
                }
			
			//if stopped wait here user, pause  feature
			//A4 = 0
			while(GPIOA_IDR &= (1 << 4) == 1);
			
			//A3 = 0 //reset feature
			if (GPIOA_IDR |= (1 << 3) == 1 || getPachinokoComplete())
			{
				//*Clear LED memeory map
				for (i = 0; i <16; i++)
				{
					LED[i] = 0x00;
				}	
			}
		}
		//if A2 == 1 then Algorithm 3
		else if (GPIOA_IDR &= (1 << 2) == 1)
		{
			if(getUpDn() == down) //Start at top and move down
            {
                for (ledRow = 15; ledRow >= 0; ledRow--)
                {
                    for (ledCol = 0; ledCol <8; ledCol++)
                    {
                        DoLED3(ledRow,ledCol);
                    }
                }
            }
            else //STart at bottom and move up
            {
                for (ledRow = 0; ledRow < 16; ledRow++)
                {
                    for (ledCol = 0; ledCol < 8; ledCol++)
                    {
                        DoLED3(ledRow, ledCol);
                    }
                }
            }
			
			//if stopped wait here user, pause  feature
			//A4 = 0
			while(GPIOA_IDR &= (1 << 4) == 1);
			
			//A3 = 0 //reset feature
			if (GPIOA_IDR |= (1 << 3) == 1)
			{
				//*Clear LED memeory map
				for (i = 0; i <16; i++)
				{
					LED[i] = 0x00;
				}
				
				//*Place hour glass outlinne in memory map
				if (getUpDn() == down)
				{
					makeInitialGenUpWater();
				}
				else
				{
					makeInitialGenDownWater();
				}
				
				makeWaterGlass();
			}
		}
	 }
	 return 0;
 }
 
void TIM2_IRQHandler()
 {
	unsigned char d;
	
	GPIOA_ODR &= ~(1 << 6); //A6 = 0
	GPIOA_ODR &= ~(1 << 7); //A7 = 0
	GPIOA_ODR &= ~(1 << 8); //A8 = 0
	GPIOA_ODR &= ~(1 << 9); //A9 = 0
	GPIOA_ODR &= ~(1 << 10); //A10 = 0
	GPIOA_ODR &= ~(1 << 11); //A11 = 0
	GPIOA_ODR &= ~(1 << 12); //A12 = 0
	GPIOA_ODR &= ~(1 << 13); //A13 = 0
	
	//* Get the data d = LED[refreshRow]
	d = LED[refreshRow];
	GPIOA_ODR |= (d << 6);
	
	//convert refreshRow to port bits for decoder
	OutputEncoder(refreshRow);		
		
	//Update refreshRow
	refreshRow++;
	if(refreshRow == 16)
		refreshRow = 0;
	
	TIM2_SR &= 0xFFFE; //Turn off interrupt
	
 }
 
 void OutputEncoder(unsigned char rfRow)
	{
		// Handle choosing the right encoder based on MSB
		GPIOA_ODR &= ~(1 << 14); //A14 = 0
		GPIOA_ODR &= ~(1 << 15); //A15 = 0
		
		
		if((rfRow & (1 << 3)) == 0)		
			GPIOA_ODR |= (1 << 14); //A14 = 1		
		else
			GPIOA_ODR |= (1 << 15); //A15 = 1	
		
		// Output the least significan 3 bits from 
		//the input number onto the encoders input	
		//CLEAR C0,C1,2
		GPIOC_ODR &= ~(1 << 0); //c0 = 0
		GPIOC_ODR &= ~(1 << 1); //c1 = 0
		GPIOC_ODR &= ~(1 << 2); //c2 = 0
		
		GPIOC_ODR |= (((rfRow >> 2) & 1) << 2);
		GPIOC_ODR |= (((rfRow >> 1) & 1) << 1);
		GPIOC_ODR |= (((rfRow >> 0) & 1) << 0);
	}
 
 unsigned char getUpDn (void)
{   
	ADC1_CR2 |= 0x40000000;      // Bit 30 does software start of A/D conversion
	while((ADC1_SR & 0x2) == 0); //Bit 1 is End of Conversion
		 
	result = ADC1_DR & 0xFFF;		
			
	if	(result > 2023)
		return up;	
	else if(result < 1737)
		return down;
	else
		return flat;
	
	return up;
}
 
 void DoLED1(unsigned char r, unsigned char c)
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
                        (c > 0) &&
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
                            (c < 7) &&
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

void DoLED2(unsigned char r, unsigned char c)
	{
	    if(getUpDn() != flat)
		{
			if (getUpDn()== down)
			{
				//look at the bottom LED
				//Move LED at (r,c) accordign to algirithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r+1 < 16) &&
                    (GetLedStatus(r+1,c) == 0))
				{
					SetLed(r+1, c, 1);
					SetLed(r  , c, 0);
					//to check if bottom is hit
					if (r+1 == 15 || GetLedStatus(r+2,c) != 0)
                        pachinkoRoundflag = 1;
				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r+1 < 16) &&
                        (c-1 >= 0) &&
                        (GetLedStatus(r+1,c - 1) == 0))
					{
						SetLed(r+1, c - 1, 1);
						SetLed(r  , c, 0);

						if (r+1 == 15 || GetLedStatus(r+2,c-1) != 0)
                            pachinkoRoundflag = 1;
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r+1 < 16)&&
                            (c+1 <= 7)&&
                            (GetLedStatus(r+1,c + 1) == 0))
					{
						SetLed(r+1, c + 1, 1);
						SetLed(r  , c, 0);

						if (r+1 == 15 || GetLedStatus(r+2,c+1) != 0)
                            pachinkoRoundflag = 1;
					}
				}
			}
			else
			{
				//look at the top LED
				//Move LED at (r,c) according to algorithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r-1 >= 0) &&
                    (GetLedStatus(r-1,c) == 0))
				{
					printf("Inn 1\n");
					SetLed(r-1, c, 1);
					SetLed(r  , c, 0);

					if (r-1 == 0 || GetLedStatus(r-2,c) != 0)
                    {
                        pachinkoRoundflag = 1;
                    }

				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r-1 >= 0) &&
                        (c > 0) &&
                        (GetLedStatus(r-1,c - 1) == 0))
					{
						printf("Inn 2\n");
						SetLed(r-1, c - 1, 1);
						SetLed(r  , c, 0);

						if (r-1 == 0 || GetLedStatus(r-2,c-1) != 0)
                        {
                            pachinkoRoundflag = 1;
                        }

					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r-1 >= 0) &&
                            (c < 7) &&
                            (GetLedStatus(r-1,c + 1) == 0))
					{
						printf("Inn 3\n");
						SetLed(r-1, c + 1, 1);
						SetLed(r  , c, 0);

						if (r-1 == 0 || GetLedStatus(r-2,c + 1) != 0)
                        {
                            pachinkoRoundflag = 1;
                        }
					}
				}
			}
		}
	}

void DoLED3(unsigned char r, unsigned char c)
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
                    (checkStatus1(r+1,c) != 0))
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
                        (checkStatus1(r+1,c - 1) != 0))
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
                            (checkStatus1(r+1,c + 1) != 0))
					{
						SetLed(r+1, c + 1, 1);
						SetLed(r  , c, 0);
					}

					else if((GetLedStatus(r,c) != 0) &&
                            (c+1 <= 7)&&
                            (GetLedStatus(r,c + 1) == 0) &&
                            (checkStatus1(r,c + 1) != 0))
					{
						SetLed(r, c + 1, 1);
						SetLed(r  , c, 0);
					}

					else if((GetLedStatus(r,c) != 0) &&
                            (c-1 >= 0)&&
                            (GetLedStatus(r,c - 1) == 0) &&
                            (checkStatus1(r,c - 1) != 0))
					{
						SetLed(r, c - 1, 1);
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
                    (checkStatus1(r-1,c) != 0))
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
                        (c > 0) &&
                        (GetLedStatus(r-1,c - 1) == 0) &&
                        (checkStatus1(r-1,c - 1) != 0))
					{
						SetLed(r-1, c - 1, 1);
						SetLed(r  , c, 0);
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r-1 >= 0) &&
                            (c < 7) &&
                            (GetLedStatus(r-1,c + 1) == 0) &&
                            (checkStatus1(r-1,c+1) != 0))
					{
						SetLed(r-1, c + 1, 1);
						SetLed(r  , c, 0);
					}

					else if((GetLedStatus(r,c) != 0) &&
                            (c < 7) &&
                            (GetLedStatus(r,c + 1) == 0) &&
                            (checkStatus1(r,c + 1) != 0))
					{
						SetLed(r, c + 1, 1);
						SetLed(r  , c, 0);
					}

					else if((GetLedStatus(r,c) != 0) &&
                            (c > 0) &&
                            (GetLedStatus(r,c - 1) == 0) &&
                            (checkStatus1(r,c - 1) != 0))
					{
						SetLed(r, c - 1, 1);
						SetLed(r  , c, 0);
					}
				}
			}
		}
	}

int getPachinokoComplete()
{
    int ledRow, ledCol;
    for (ledRow = 2; ledRow < 14; ledRow++)
    {
        for (ledCol = 0; ledCol < 8; ledCol++)
        {
            if(!GetLedStatus(ledRow, ledCol))
                return 0;
        }
    }
    return 1;
}

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

unsigned char checkStatus1(unsigned char row,
                        unsigned char col)
{
   if ((row == 4))
   {
       if(   (col == 0) || (col == 1)
          || (col == 2) || (col == 3)
          || (col == 4) || (col == 5)
          || (col == 6))
       {
           return 0;
       }
   }

   if((row == 10))
   {
        if(  (col == 4) || (col == 5)
          || (col == 6) || (col == 7)
          || (col == 3) || (col == 2)
          || (col == 1))
       {
           return 0;
       }
   }


    return 1;

}

unsigned char GetLedStatus(unsigned char row,
                           unsigned char column)
{
	unsigned char temp = LED[row];

	return temp & (1 << column);
}

void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state)
{
	if(state)
		LED[row] |= (1 << column);
	else
		LED[row] &= ~(1 << column);
}

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

void makeInitialGenUpWater(void)
{
    int i , j;
    for (i = 0; i < 4; i++)
    {
        for (j = 0; j < 7; j++)
        {
            SetLed(i,j,1);
        }
    }
}

void makeInitialGenDownWater(void)
{
    int i , j;
    for (i = 11; i < 15; i++)
    {
        for (j = 1; j < 8; j++)
        {
            SetLed(i,j,1);
        }
    }
}

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

void makeWaterGlass(void)
{
    SetLed(4,0,0);
    SetLed(4,1,0);
    SetLed(4,2,0);
    SetLed(4,3,0);
    SetLed(4,4,0);
    SetLed(4,5,0);
    SetLed(4,6,0);

    SetLed(10,1,0);
    SetLed(10,2,0);
    SetLed(10,3,0);
    SetLed(10,4,0);
    SetLed(10,5,0);
    SetLed(10,6,0);
    SetLed(10,7,0);
}


