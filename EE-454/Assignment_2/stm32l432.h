//RCC starts at 0x4002 1000
#define RCC_AHB2ENR  (*((volatile unsigned long *) 0x4002104C)) //AHB2 peripheral clock enable register
#define RCC_APB1ENR1 (*((volatile unsigned long *) 0x40021058)) //APB1 peripheral clock enable register 1
	
//GPIOA start 0x4800 0000
#define GPIOA_MODER (*((volatile unsigned long *) 0x48000000)) //GPIO A Mode register
#define GPIOA_PUPDR (*((volatile unsigned long *) 0x4800000C)) //GPIO A Pudr register
	
//GPIOB start 0x4800 0400
#define GPIOB_MODER (*((volatile unsigned long *) 0x48000400)) //GPIO B Mode register
#define GPIOB_PUPDR (*((volatile unsigned long *) 0x4800040C)) //GPIO B Pudr register
#define GPIOB_BSRR  (*((volatile unsigned long *) 0x48000418)) //GPIO B Output Bit set/reset register
#define GPIOB_AFRL  (*((volatile unsigned long *) 0x48000420)) //GPIO B Alternate func register
	
//ADC start 0x5004 0000
#define ADC_ISR			(*((volatile unsigned long *) 0x50040000)) //ADC interrupt and status register
#define ADC_IER 		(*((volatile unsigned long *) 0x50040004)) //ADC interrupt enable register
#define ADC_CR 			(*((volatile unsigned long *) 0x50040008)) //ADC control register
#define ADC_SQR1  	(*((volatile unsigned long *) 0x50040030)) //ADC regular sequence register
#define ADC_DR 			(*((volatile unsigned long *) 0x50040040)) //ADC data register
#define ADC_CCR 		(*((volatile unsigned long *) 0x50040308)) //ADC common control register
#define ADC_CFGR 		(*((volatile unsigned long *) 0x5004000C)) //ADC configuration register
	
//TIM2 start 0x4000 0000
#define TIM2_CR1   (*((volatile unsigned long *) 0x40000000)) //TIM2 control register
#define TIM2_EGR	 (*((volatile unsigned long *) 0x40000014)) //TIM2 event generation register
#define TIM2_CCMR1 (*((volatile unsigned long *) 0x40000018)) //TIM2 capture/compare mode register
#define TIM2_PSC   (*((volatile unsigned long *) 0x40000028)) //TIM2 event generation register
#define TIM2_ARR   (*((volatile unsigned long *) 0x4000002C)) //TIM2 auto-reload register
#define TIM2_CCR2  (*((volatile unsigned long *) 0x40000038)) //TIM2 capture/compare register
#define TIM2_CCER  (*((volatile unsigned long *) 0x40000020)) //TIM2 capture/compare enable register
	

	