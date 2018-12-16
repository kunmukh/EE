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
void SetLed(unsigned char row, unsigned char column, unsigned char state);
unsigned char GetLedStatus(unsigned char row, unsigned char column);
void DoLED1(unsigned char r, unsigned char c);
void DoLED2(unsigned char r, unsigned char c);
void DoLED3(unsigned char r, unsigned char c);
unsigned char getUpDn (void);
void makeInitialGenUp(void);
void makeInitialGenDown(void);
void makeInitialGenUpWater(void);
void makeInitialGenDownWater(void);
unsigned char checkStatus(unsigned char row, unsigned char col);
unsigned char checkStatus1(unsigned char row,unsigned char col);
int getPachinokoComplete(void);
void makeHrGlass(void);
void makeWaterGlass(void);
void OutputEncoder(unsigned char rfRow);

//extern void checkStatus(void);


//*initialize global constants
unsigned char down = 0;
unsigned char up = 1;
unsigned char flat = 2;
int pachinkoRoundflag = 1;

unsigned char LED[16];
unsigned char refreshRow = 0; //row to be refreshed
int flag = 0;
int flag1 = 0;
int flag2 = 0;
int flag3 = 0;

int main()
 {
	//temporary variables initialization
	int i,it,jt, ledRow, ledCol, randCol;
	
	//Enabling Clock bits
  RCC_AHB1ENR |= 1;     //Bit 0 is GPIOA clock enable bit
	RCC_AHB1ENR |= 2; //Bit 1 is GPIOB clock enable bit
	RCC_APB1ENR |= 1; //Enable peripheral timer for timer 2 (bit 0)
	RCC_AHB1ENR |= 4;     //Bit 3 is GPIOC clock enable bit	
	
	//I/O bits	
	GPIOA_MODER |= 0x5C00;	//Bits 0-1 for Input, 														
	GPIOC_MODER |= 0x5555;   	//Bits 0-7 for digitial output 
	GPIOB_MODER |= 0x15;
							
							
	//OTYPER register resets to 0 so it is push/pull by default
	GPIOA_OSPEEDER |= 0xFFFFFF;  	//Bits 0-15 for high speed on PA 1													 
	GPIOC_OSPEEDER |= 0xFFFF; 		//Bits 0-7 for high speed on PA 3
	GPIOB_OSPEEDER |= 0x3003F;
	 
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
		LED[i] = 0;
	}		

	 //Main program loop	 
	 while(1)
	 {		 	
		 //if interrupt triggered then turn flag off and restart timer
		if (flag)
		{
			TIM2_CR1 |= 1; //Restart timer 
			flag = 0;		
		}			
		 
		//if A0 == 1 then Algorithm 1
		if (GPIOA_IDR & (1 << 0))
		{		
			if(flag1 == 0)
			{
				flag1 = 1;
				flag2 = 0;
				flag3 = 0;
				pachinkoRoundflag = 1;
				
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
						
						for (it = 0; it < 64000; it++)
						{
							for (jt = 0; jt < 5; jt++);
						}		
		}
		
		
		
		//if A1 == 1 then Algorithm 2
		else if (GPIOA_IDR & (1 << 1))
		{			
			if(flag2 == 0 || getPachinokoComplete())
			{
				flag2 = 1;
				flag1 = 0;
				flag3 = 0;
				pachinkoRoundflag = 1;
				//*Clear LED memeory map
				for (i = 0; i <16; i++)
				{
					LED[i] = 0x00;
				}				
			}
			
			if(getUpDn() == down) //Start at top and move down
                {
                    randCol = (int) rand() % 8;
                    if(pachinkoRoundflag == 1)
                    {
                        SetLed(0, randCol, 1);                        
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
								
						for (it = 0; it < 64000; it++)
						{
							for (jt = 0; jt < 2; jt++);
						}				
			
		}
		
		
		
		//if B8 == 1 then Algorithm 3
		else if (GPIOB_IDR & (1 << 8))
		{
						
			if(flag3 == 0)
			{
				flag3 = 1;
				flag2 = 0;
				flag1 = 0;
				pachinkoRoundflag = 1;
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

       for (it = 0; it < 64000; it++)
						{
							for (jt = 0; jt < 5; jt++);
						}
		} 
	 
	 }
	 
 }
 
 
 //timer intreupt handler
void TIM2_IRQHandler()
 {
	 unsigned char d;
	
	GPIOC_ODR &= ~(1 << 0); //C0 = 0
	GPIOC_ODR &= ~(1 << 1); //C1 = 0
	GPIOC_ODR &= ~(1 << 2); //C2 = 0
	GPIOC_ODR &= ~(1 << 3); //C3 = 0
	GPIOC_ODR &= ~(1 << 4); //C4 = 0
	GPIOC_ODR &= ~(1 << 5); //C5 = 0
	GPIOC_ODR &= ~(1 << 6); //C6 = 0
	GPIOC_ODR &= ~(1 << 7); //C7 = 0
	
	//* Get the data d = LED[refreshRow]
  d = LED[refreshRow];
	GPIOC_ODR |= d;
	
	//convert refreshRow to port bits for decoder
	OutputEncoder(refreshRow);			
	 
	//Update refreshRow
	refreshRow++;
	if(refreshRow == 16)
		refreshRow = 0;
	
	flag = 1;
	TIM2_SR &= 0xFFFE; //Turn off interrupt
	
 }
 
 
 //the output encoder algorithms for 74LS244
 
 void OutputEncoder(unsigned char rfRow)
	{
		// Handle choosing the right encoder based on MSB
		GPIOA_ODR &= ~(1 << 6); //A6 = 0
		GPIOA_ODR &= ~(1 << 7); //A7 = 0
		
		
		if((rfRow & (1 << 3)) == 0)		
			GPIOA_ODR |= (1 << 6); //A6 = 1		
		else
			GPIOA_ODR |= (1 << 7); //A7 = 1	
		
		// Output the least significan 3 bits from 
		//the input number onto the encoders input	
		//CLEAR C0,C1,2
		GPIOB_ODR &= ~(1 << 0); //A8 = 0
		GPIOB_ODR &= ~(1 << 1); //A9 = 0
		GPIOB_ODR &= ~(1 << 2); //A10 = 0		
		
		
		GPIOB_ODR |= (((rfRow >> 2) & 1) << 2);//(((rfRow >> 0) & 1) << 0);
		GPIOB_ODR |= (((rfRow >> 1) & 1) << 1);//(((rfRow >> 1) & 1) << 1);
		GPIOB_ODR |= (((rfRow >> 0) & 1) << 0);//(((rfRow >> 2) & 1) << 2);
	} 
	
 unsigned char getUpDn (void)
{   
	int result;
	ADC1_CR2 |= 0x40000000;      // Bit 30 does software start of A/D conversion
	while((ADC1_SR & 0x2) == 0); //Bit 1 is End of Conversion
		 
	result = ADC1_DR & 0xFFF;		
			
	if	(result > 2023)
		return up;	
	else if(result < 1737)
		return down;
	else
		return flat;	

   //return down;	
}
 

//hourglass algorithm
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

	
	//Pachinko algorithm
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


	//Waterfall algorithm
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

	
	
	//check to see if Panchino is Complete
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


//check to see if the hour glass led status has hit or not
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


//check to see if it has hit a boundary
unsigned char checkStatus1(unsigned char row,
                        unsigned char col)
{
   if (row == 4)
   {
       if(   (col == 0) || (col == 1)
          || (col == 2) || (col == 3)
          || (col == 4) || (col == 5)
          || (col == 6))
       {
           return 0;
       }
   }
   if(row == 10)
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


//get the led status of a led
unsigned char GetLedStatus(unsigned char row,
                           unsigned char column)
{
	unsigned char temp = LED[row];

	return temp & (1 << column);
}


//set the led status
void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state)
{
	if(state)
		LED[row] |= (1 << column);
	else
		LED[row] &= ~(1 << column);
}


//Make initial gen of hour glass up
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


//Make initial gen of hour glass down
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


//Make the initial generation of water up
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


//Make the initial generation of water down
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


//Make the Hour Glass outline
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


	//Make the Water Glass Outline
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


