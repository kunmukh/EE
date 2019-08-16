//Kunal Mukherjee
//Project 1: Automatic Door
//2/16/2019
#include "stm32f446.h"
#include <stdio.h>
#include <stdlib.h>

/*stm446Template.c July 1, 2017
*/

//*Global functions	
int getKeyPressed(void);
void delayOne(void);
void delayTwo(void);
void delayTen(void);

//*initialize global constants
int codeInputGottenFirst = 0; //set the 6-digit code
int codeInputGotten = 0;     //get the user input complete
int codeInputStatus = 0;     //flag to see if xode is correct or not

int code [6]; //the got set
int inputCode [6]; //the user input code

int main()
 {
	//*Initialize varibales		
	int i;
	int charPres = 99;
	int codeIndexFirst = 0;
	int codeIndex = 0;
	int codeCheck = 0;
	int reset = 0;
	
	//Enabling Clock bits
	RCC_AHB1ENR |= 1;     //Bit 0 is GPIOA clock enable bit
  RCC_AHB1ENR |= 4;     //Bit 3 is GPIOC clock enable bit	
	
	//I/O bits	
	//KeyPad
	GPIOC_MODER &= ~(3 << (2 * 1)); //C1 = input
	GPIOC_MODER &= ~(3 << (2 * 3)); //C3 = input
	GPIOC_MODER &= ~(3 << (2 * 5)); //C5 = input
	
	GPIOC_MODER |= (1 << (2 * 2)); //C2 = output
	GPIOC_MODER |= (1 << (2 * 4)); //C4 = output
	GPIOC_MODER |= (1 << (2 * 6)); //C6 = output
	GPIOC_MODER |= (1 << (2 * 7)); //C7 = output
	
	GPIOC_OTYPER |= (1 << (1 * 2)); //C2 = open-drain
	GPIOC_OTYPER |= (1 << (1 * 4)); //C4 = open-drain
	GPIOC_OTYPER |= (1 << (1 * 6)); //C6 = open-drain
	GPIOC_OTYPER |= (1 << (1 * 7)); //C7 = open-drain	
	
	GPIOC_OSPEEDER |= (2 << (2 * 2)); //C2 = fast-speed
	GPIOC_OSPEEDER |= (2 << (2 * 4)); //C4 = fast-speed
	GPIOC_OSPEEDER |= (2 << (2 * 6)); //C6 = fast-speed
	GPIOC_OSPEEDER |= (2 << (2 * 7)); //C7 = fast-speed	
	//GRIO_PUPDR = reset value is 0, so no pull-up/pull-down
	
	//LED and Solenoid
	GPIOA_MODER |= (1 << (2 * 0)); //A0 = output red
	GPIOA_MODER |= (1 << (2 * 1)); //A1 = output green
	GPIOA_MODER |= (1 << (2 * 6)); //A6 = output red
	GPIOA_MODER |= (1 << (2 * 7)); //A7 = output green
	GPIOA_MODER |= (1 << (2 * 5)); //A5 = output solenoid
	//GPIOA_OTYPER default is push/pull-up/pull-down
	
	GPIOA_OSPEEDER |= (2 << (2 * 0)); //A0 = fast-speed
	GPIOA_OSPEEDER |= (2 << (2 * 1)); //A1 = fast-speed
	GPIOA_OSPEEDER |= (2 << (2 * 6)); //A6 = fast-speed
	GPIOA_OSPEEDER |= (2 << (2 * 7)); //A7 = fast-speed
	GPIOA_OSPEEDER |= (2 << (2 * 5)); //A5 = fast-speed	
	
	//initialization of the code
	for (i = 0; i < 6; i++){code[i] = 99; inputCode[i] = 99;}	

	//turn solenoid off
	GPIOA_ODR |= (1 << (1 * 5)); //A5 = high- Solenoid

	 //Main program loop	 
	 while(1)
	 {
			while (!codeInputGottenFirst) //getting the inout code for the first time
			{
				delayOne();
				charPres = getKeyPressed();
				if (charPres != 99)
				{
					code[codeIndexFirst] = charPres;
					codeIndexFirst++;
					GPIOA_ODR |= (1 << (1 * 0)); //A0 = high; //red
					delayTwo();
					GPIOA_ODR &= ~(1 << (1 * 0)); //A0 = low; //red
				}
				
				if (codeIndexFirst == 6){codeInputGottenFirst = 1;}
			}
			delayTwo();
			GPIOA_ODR |= (1 << (1 * 0)); //A0 = high; //red;
			
			while (!codeInputGotten) //getting the inout code
			{
				delayOne();
				charPres = getKeyPressed();
				if (charPres != 99)
				{
					inputCode[codeIndex] = charPres;
					codeIndex++;
					GPIOA_ODR |= (1 << (1 * 1)); //A1 = high;
					delayTwo();
					GPIOA_ODR &= ~(1 << (1 * 1)); //A1 = low;
				}
				
				if (codeIndex == 6){codeInputGotten = 1;}
			}
			
			//check to see reset
			for (i = 0; i < 6; i++)
			{
				if (inputCode[i] != 0)
				{
					reset = 0;
					break;
				}else{
					reset = 1;
				}
			}
			
			if (reset == 0)
			{
					//check to see if code is same
					for (i = 0; i < 6; i++)
					{
						if (code[i] == inputCode[i])
						{
							codeInputStatus = 1;
						}
						else
						{
							codeInputStatus = 0;
							break;
						}
					}
					
					if (codeInputStatus == 1) //code is right
					{
						delayTwo();
						GPIOA_ODR &= ~(1 << (1 * 5)); //A5 = low- Solenoid ON
						for (i = 0; i < 5; i++)
						{
							GPIOA_ODR |= (1 << (1 * 6)); //A6 = high;
							delayTwo();
							GPIOA_ODR &= ~(1 << (1 * 6)); //A6 = low;
							delayTwo();
						}
						delayTen();
						GPIOA_ODR |= (1 << (1 * 5)); //A5 = high- Solenoid OFF
						
					}
					else
					{
						delayTwo();
						for (i = 0; i < 5; i++)
						{
							GPIOA_ODR |= (1 << (1 * 7)); //A7 = red
							delayTwo();
							GPIOA_ODR &= ~(1 << (1 * 7)); //A7 = red
							delayTwo();
						}
					}				
			}else{
					delayTwo();
					GPIOA_ODR &= ~(1 << (1 * 0)); //A0 = high; //red;
					codeInputGottenFirst = 0;
					codeIndexFirst = 0;
					for (i = 0; i < 6; i++){code[i] = 99;}
			}
			
			
			//clear the code entered
			//initialization of the code
			for (i = 0; i < 6; i++){inputCode[i] = 99;}	
			codeIndex = 0;			
			codeInputStatus = 0;
			codeInputGotten = 0;		
			delayOne();
		 
	 }
	 
 } 
 
 int getKeyPressed()
 {
	 GPIOC_ODR |=  0xD4;
	 int j;
	 
	 GPIOC_ODR &= ~(1 << 2);//set C2 low, 3
	 delayOne();
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 1))) //check if C1 is low, 2
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 2; //'2'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 3))) //check if C3 is low, 4
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 1; //'1'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 5))) //check if C5 is low, 6
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 3; //'3'			 
		 }
	 }
	 GPIOC_ODR |= (1 << 2);//set C2 high
	 GPIOC_ODR &= ~(1 << 4);//set C4 low, 5
	 delayOne();
	 while(! (GPIOC_IDR & (1 << 1))) //check if C1 is low, 2
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 0; //'0'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 3))) //check if C3 is low, 4
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 10; //'*'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 5))) //check if C5 is low, 6
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 11; //'#'			 
		 }
	 }
	 GPIOC_ODR |= (1 << 4);//set C4 high
	 GPIOC_ODR &= ~(1 << 6);//set C6 low, 7
	 delayOne();
	 while(! (GPIOC_IDR & (1 << 1))) //check if C1 is low, 2
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 8; //'8'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 3))) //check if C3 is low, 4
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 7; //'7'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 5))) //check if C5 is low, 6
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 9; //'9'			 
		 }
	 }
	 GPIOC_ODR |= (1 << 6);//set C6 high
	 GPIOC_ODR &= ~(1 << 7);//set C7 low, 8
	 delayOne();
	 while(! (GPIOC_IDR & (1 << 1))) //check if C1 is low, 2
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 5; //'5'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 3))) //check if C3 is low, 4
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 4; //'4'			 
		 }
	 }
	 j = 0;
	 while(! (GPIOC_IDR & (1 << 5))) //check if C5 is low, 6
	 {
		 if(j == 0)
		 {
			 j = 1;
			 return 6; //'6'			 
		 }
	 }
	 GPIOC_ODR |= (1 << 7);//set C7 high
	 
	 
	 return 99;
	 
 }
 
 void delayOne(void)
 {
	 int i,j;
	 for (i = 0; i < 5000; i++);
	 //for (i = 0; i < 16000; i++){ for (j = 0; j < 250; j++);} //1 sec
 }
 
 void delayTwo(void)
 {
	 int i,j;
	 for (i = 0; i < 16000; i++){ for (j = 0; j < 350; j++);} //1 sec
 }
 
 void delayTen(void)
 {
	int i,j;
	for (i = 0; i < 16000; i++){ for (j = 0; j < 1250; j++);} //10 sec
 }

 
 
