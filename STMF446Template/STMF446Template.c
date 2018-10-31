#include "stm32f446.h"

/*stm446Template.c July 1, 2017
 This program toggles a bit on PA7 as fast as possible
*/

int main()
 {int tmp,i;
 //Clock bits
 RCC_AHB1ENR |= 1; //Bit 0 is GPIOA clock enable bit
 //I/O bits
 GPIOA_MODER |= 0x4000; //Bits 15-14 = 01 for digital output on PA7
 //OTYPER register resets to 0 so it is push/pull by default
 GPIOA_OSPEEDER |= 0xC000; //Bits 15-14 = 11 for high speed on PA7
 //PUPDR defaults to no pull up no pull down
 //Main program loop
 tmp = 0;
 while(1)
 {GPIOA_ODR = tmp; //Only PA7 is set up for output so other bits
 tmp = ~tmp; // have no effect.
 for (i=0; i<10000;i++);
 }
 }
