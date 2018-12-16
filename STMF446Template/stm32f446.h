#include "stm32f446.h"

/*stm446Template.c July 1, 2017
 This program toggles a bit on PA7 as fast as possible
*/

int main()
 {
	 int tmp,i;
	 //Clock bits
	 RCC_AHB1ENR |= 4; ; //Bit 0 is GPIOC clock enable bit
	 //I/O bits
	 GPIOC_MODER |= 0x1; //Bits 15-14 = 01 for digital output on PA7
	 //OTYPER register resets to 0 so it is push/pull by default
	 GPIOC_OSPEEDER |= 0xC000; //Bits 15-14 = 11 for high speed on PA7
	 //PUPDR defaults to no pull up no pull down
	 //Main program loop
	 tmp = 0;
	 
	 while(1)
	 {
		 GPIOC_ODR = tmp; //Only PA7 is set up for output so other bits
		 tmp = ~tmp; // have no effect.
		 
		 for (i=0; i<80000;i++);
	 }
 }