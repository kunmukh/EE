//Kunal Mukherjee;
//2/26/2019
//Assignment 4: Frequency Detector

#include "stm32l432.h"
int i = 0, k = 0;
int average = 0, counter = 0;
int main()
{
	int ki;
	
	RCC_AHB2ENR  |= (1 << 0);    //GPIOA clk enable
	RCC_AHB2ENR  |= (2 << 0);    //GPIOB clk enable
	RCC_APB1ENR1 |= (1 << 0);   //TIM2 clk enbale
	
	//set PB3 as output LED
	GPIOB_MODER &= ~(3 << (2 * 3)); 	
	GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output
	
	//set up PA1 as alternate func TIM2_CH2
	//no pull up and pull down
	GPIOA_MODER   &= ~(3 << (2 * 1)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 1)); //set port PA1 is alternate 10	
	GPIOA_AFRL    |=  (1 << (4 * 1)); //alt func 1, port pin 1, 
	                                //control bit are 4 bit wide	
	
	//interrupt initialization
	NVIC_ISER0 |= (1 << 28);  //enable TIM2 global int     
	TIM2_DIER  |=  (1 << 2); //CC2IE  Capture/Compare 2 interrupt enable
	TIM2_DIER  |=  (1 << 6); //TIE trigger enable, 
	TIM2_DIER  |=  (1 << 0); //UIE update interr enb 
	                         //(update on a overflow)
	
	//timer initialization	
	TIM2_CCMR1 &= ~(3 << 8); //clear
	TIM2_CCMR1 |=  (1 << 8); //CC2 config as input  
													 //select input capture mode	
	TIM2_CCER  |=  (1 << 4);	//capture enable//edge?	
	
	//start timer
	TIM2_EGR |= (1 << 0); //UG: update event 	
	TIM2_CR1 |= (1 << 0); //CEN: counter enabled
	
	while(1){
	//delay for 1 sec
		//check the number of counter up
		//decide what to dow ith the light
		//delay for I sec
		for (i = 0; i < 1000; i++)
		{
			for (k = 0; k < 625; k++);
		}	
				
		ki = counter;
		ki++;
		
		if (counter > 2594)
		{
			GPIOB_ODR |=  (1 << 3);
		}
		else
		{
			GPIOB_ODR &= ~(1 << 3);				
		}
		
		counter = 0;
		
	}
	
}

void TIM2_IRQHandler()
{
	int freq_tick = 0, j = 0;
	//look at why in the interrupt 
	//is frequency (period) in range
	if ((TIM2_SR & (1 << 2)) == (1 << 2)) //Capture/Compare 2 interrupt flag
	{
		freq_tick = TIM2_CCR2 & 0xFF;	
		average = ((average + freq_tick)/counter);	
		counter++;
	}	
	
	/*if (counter % 100 == 0)
	{
		counter = 0;
		
		j = average;
		j++;
		
		if (average > 300)
		{
			GPIOB_ODR |=  (1 << 3);
		}
		else
		{
			GPIOB_ODR &= ~(1 << 3);				
		}
	}*/
	
	//clear flags
	TIM2_SR &= ~(1 << 0); //update interrupt flag cleared
	TIM2_CR1 |= (1 << 0); //CEN: counter enabled
	
}
