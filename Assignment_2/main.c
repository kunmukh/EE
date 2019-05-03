//Kunal Mukherjee;
//1/27/2019
//Assignment 2: Analog to PWM

#include "stm32l432.h"

void ADC_Init(void);
void GPIO_Init(void);

int main(){

	unsigned int value ,i;	
	
	ADC_Init();
	GPIO_Init();
	
	while(1)
	{
		ADC_CR |= (1 << 2); //start adc regular conversion
		
		while((ADC_ISR & (1 << 3)) == 0); // end of regular sequence flag
		
		value = ADC_DR & 0xFFF; //only look at 12 bit
		
		TIM2_CCR2 = value * 4 ; //scale the value
		
		for (i =0;i<10000;i++); //have a delay
	}		 
 }

void GPIO_Init(void)
{	
	RCC_AHB2ENR |= (1 << 1); //set the GPIOB clk
	RCC_APB1ENR1 |= (1 << 0); //IO port B clock enable
	
	GPIOB_MODER &= ~(3 << (2 * 3)); //clear the GPIOB mode bits
	GPIOB_MODER |= (2 << (2 * 3));  //set port b3 is alternate 10
	GPIOB_AFRL |= (1 << (4 * 3));   //alt func 1, port pin 3, 
	                                //control bit are 4 bit wide
	
	TIM2_CR1 |= (1 << 7); //ARPE: Auto-reload preload enable	
	TIM2_PSC = 0;         //PSC set to 0
	TIM2_ARR = 16000;     //16MHz/16000= 500 Hz = 1 ms
	
	TIM2_CCMR1 |= 0x6800; //Channel 2; 
	                      //bit 11: OC2PE: Output compare 2 preload enable
	                      //0110: PWM mode 1 - In upcounting, 
                        //channel 1 is active as long as 
	                       //TIMx_CNT<TIMx_CCR1else inactive.
	
	TIM2_CCER |= (1 << 4); //CC2E: Capture/Compare 2 output enable.
	
	TIM2_CCR2 |= 1000;     //CCR2 is the value to be loaded in the actual 
	                       //capture/compare 2 register (preload value).
	
	TIM2_EGR |= (1 << 0); //UG: update event 
	
	TIM2_CR1 |= (1 << 0); //CEN: counter enabled
}
void ADC_Init(void)
{
	int i;
	RCC_AHB2ENR |= (1 << 0); //set the GPIOA clk
	RCC_AHB2ENR |= (1 << 13); //set ADC clk	
	
	//mode default to analog
	GPIOA_PUPDR &= ~(1  << 1); //PA1 to no pull or down  
	
	ADC_CR &= ~(1 << 29); //deep power mode cleared
	ADC_CR |= (1 << 28); //set voltage reg
	
	for (i=0; i <10000; i++);//wait for .5us
	
	ADC_CCR |= (1 << 22); //VREF ENAB
	ADC_CCR |= (1 << 16); //HCLK/1 (Synchronous clock mode) enb
	
	ADC_ISR |= (1 << 0); //ADC ready
	ADC_CR |= (1 << 0); //ENB ADC
	while ((ADC_ISR & (1 << 0)) == 0); //wait till ADC is ready
	
	//ADC_SQR1 L=1 length of sequence is 1
	ADC_SQR1 |= (6 << 6);//CH6 
	ADC_CFGR |= (1 << 16); //DISCEN: Discontinuous mode for regular channels	
}
