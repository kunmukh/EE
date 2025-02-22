//Kunal Mukherjee
//Assignment 7
//11/9/17
#include "stm32f446.h"

/*stm446Template.c July 1, 2017
*/

void turn_led(int num){
	
			GPIOA_ODR |= 0xF;
			GPIOC_ODR |= 0xF;
	
	switch(num){
		case 1:			
			GPIOA_ODR &= 0xE;
			break;
		case 2:			
			GPIOA_ODR &= 0xD;
			break;		
		case 3:			
			GPIOC_ODR &= 0xE;
			break;
		case 4:			
			GPIOC_ODR &= 0xD;
			break;		
		default:
			break;
	}
}

int main()
 {
	int i, result;
	//Clock bits
	RCC_AHB1ENR |= 1;     //Bit 0 is GPIOA clock enable bit
	RCC_AHB1ENR |= 4;  //Bit 0 is GPIOC clock enable bit
	//I/O bits
	GPIOA_MODER |= 0x5;    //Bits 0,1, = 01 6005
												  //for digital output on PA 0,1
	GPIOC_MODER |= 0x5;   //Bits 0,1, = 01 
												  //for digital output on PC 0,1
	//OTYPER register resets to 0 so it is push/pull by default
	GPIOA_OSPEEDER |= 0xF;  //Bits 0,1 = 11 
													 //for high speed on PA 0,1
	GPIOC_OSPEEDER |= 0xF; //Bits 0,1 = 11 
													 //for high speed on PC 0,1	
	 
	//Accelerometer setup
	RCC_APB2ENR |= 0x100;      //Bit 8 is ADC 1 clock enable bit
	GPIOA_MODER |= 0xC00;      //PA5 are analog
	GPIOA_PUPDR &= 0xFFFFF0FF; //Pins PA4-5 are no pull up and pull down
	
	ADC1_CR2 |= 1;             //Bit 0 turn ADC on
	ADC1_CR2 |= 0x400;         //Bit 10 allows EOC to be set after conversion
	ADC_CCR |= 0x30000;        //Bits 16 and 17 = 11 so clock divided by 8
	ADC1_SQR3 |= 0x5;          //Bits 4:0 are channel number for first conversion
	// Channel is set to 5 which corresponds to PA5
	
	 //Main program loop	 
	 while(1)
	 {		 		 
			ADC1_CR2 |= 0x40000000;      // Bit 30 does software start of A/D conversion
			while((ADC1_SR & 0x2) == 0); //Bit 1 is End of Conversion
		 
			result = ADC1_DR & 0xFFF;		
			
			if (result > 2048)
				turn_led(0);
			else if(result > 2023)
				turn_led(1);
			else if(result > 1936)
				turn_led(2);
			else if(result > 1737)
				turn_led(3);
			else if(result > 1588) // (1.28) * (4096 / 3.3) = 1588
				turn_led(4);
			else
				turn_led(5);					
			//			
			for (i=0; i<200;i++);
	 }
 }
