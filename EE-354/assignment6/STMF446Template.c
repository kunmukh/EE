#include "stm32f446.h"

/*stm446Template.c July 1, 2017
 This program toggles a bit on PA7 as fast as possible
*/
extern void delaye(void); //0.005s delay

int main()
 {
	 int i;
	 //Clock bits
	 RCC_AHB1ENR |= 4; ; //Bit 0 is GPIOC clock enable bit
	 //I/O bits
	 GPIOC_MODER |= 0x1; //Bits 0 = 01 for digital output on PC0
	 //OTYPER register resets to 0 so it is push/pull by default
	 GPIOC_OSPEEDER |= 0x3; //Bits 10 = 11 for high speed on PC0
	 //PUPDR defaults to no pull up no pull down
	 //Main program loop	 
	 while(1)
	 {
		 for (i=0; i<8570; i++);		 //0.0042865 sec delay
		 delaye();
	 }
 } 
