//Kunal Mukherjee;
//1/27/2019
//Assignment 3: Interfacing Encoders

#include "stm32l432.h"

void GPIO_Init(void);

int counter = 0;

int main()
{
	//PA1 as Input and PA3 as output
	GPIO_Init();
	
	//enable interrupt in NVIC_ISER0
	NVIC_ISER0 |= (1 << 7);
	//select PA1 in system
	SYSCFG_EXTICR1 &= ~(7 << 4); //EXT1 = Line 1 = PA[1] = 000
	//interrupt mark register -- line EXT1 -- PA1
	EXTI_IMR1 |= (1 << 1);
	//interrupt condition
	EXTI_RTSR1 |= (1 << 1);	
	
	//GPIOB_ODR |= (1 << 3);	
	
	while(1)
	{	}
 }

 void EXTI1_IRQHandler()
 {	 
	 counter++;
	 
	 if (counter >= 10)
	 {
		 GPIOB_ODR ^= (1 << 3);
		 counter = 0;
	 }
	 EXTI_PR1 |= (1 << 1);
 }

 void GPIO_Init(void)
{	
	RCC_AHB2ENR |= (1 << 0); //set the GPIOA clk	
	RCC_AHB2ENR |= (2 << 0); //set the GPIOB clk	
	
	GPIOA_MODER &= ~(3 << (2 * 1)); //clear the GPIOA mode bits = Input		
	
	GPIOB_MODER &= ~(3 << (2 * 3)); 	
	GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output
	
}
