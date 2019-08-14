//Kunal Mukherjee
//3/15/2019

#define READ 1
#define WRITE 0

#define I2C_CR1_PE     (1 << 0)
#define I2C_CR2_WRN    (1 << 10)
#define I2C_CR2_START  (1 << 13)
#define I2C_CR2_STOP   (1 << 14)
#define I2C_CR2_NACK   (1 << 15)
#define I2C_ISR_STOPF  (1 << 05)
#define I2C_ICR_STOPCF (1 << 05)
#define I2C_ISR_BUSY   (1 << 15)
#define I2C_ISR_TXIS   (1 << 01)
#define I2C_ISR_TXE    (1 << 00)
#define I2C_ISR_TC     (1 << 06)
#define I2C_ISR_NACKF  (1 << 04)
#define I2C_ISR_RXNE   (1 << 02)
#define I2C_ISR_ADDCODE (127 << 17)
#define I2C_ISR_DIR    (1 << 16)
#define I2C_ISR_ADDR   (1 << 3)

#define RCC_AHB1ENR_DMA1EN (1 << 0)
#define DMA_CCR_MEM2MEM (1 << 14)
#define DMA_CCR_PL      (3 << 12)
#define DMA_CCR_PL_1    (3 << 12)
#define DMA_CCR_PSIZE   (3 << 8)
#define DMA_CCR_PSIZE_0 (1 << 8)
#define DMA_CCR_MSIZE   (3 << 10)
#define DMA_CCR_MSIZE_0 (1 << 10)
#define DMA_CCR_PINC    (1 << 6)
#define DMA_CCR_MINC    (1 << 7)
#define DMA_CCR_CIRC    (1 << 5)
#define DMA_CCR_DIR     (1 << 4)
#define DMA_CCR_EN      (1 << 0)
#define DMA_CSELR_C1S   (15 << 0)


//RCC starts at 0x4002 1000
#define RCC_AHB2ENR  (*((volatile unsigned long *) 0x4002104C)) //AHB2 peripheral clock enable register
#define RCC_APB1ENR1 (*((volatile unsigned long *) 0x40021058)) //APB1 peripheral clock enable register 1
#define RCC_AHB1ENR  (*((volatile unsigned long *) 0x40021048)) //AHB1 peripheral clock enable register
	
//GPIOA start 0x4800 0000
#define GPIOA_MODER   (*((volatile unsigned long *) 0x48000000)) //GPIO A Mode register
#define GPIOA_OTYPER  (*((volatile unsigned long *) 0x48000004)) //GPIO A Output type reg
#define GPIOA_OSPEEDR (*((volatile unsigned long *) 0x48000008)) //GPIO A Output speed register
#define GPIOA_PUPDR   (*((volatile unsigned long *) 0x4800000C)) //GPIO A Pudr register
#define GPIOA_AFRL    (*((volatile unsigned long *) 0x48000020)) //GPIO A Alternate func register low
#define GPIOA_AFRH    (*((volatile unsigned long *) 0x48000024)) //GPIO A Alternate func register high
#define GPIOA_ODR			(*((volatile unsigned long *) 0x48000014)) //GPIO A Output data reg

//GPIOB start 0x4800 0400
#define GPIOB_MODER (*((volatile unsigned long *) 0x48000400)) //GPIO B Mode register
#define GPIOB_PUPDR (*((volatile unsigned long *) 0x4800040C)) //GPIO B Pudr register
#define GPIOB_BSRR  (*((volatile unsigned long *) 0x48000418)) //GPIO B Output Bit set/reset register
#define GPIOB_AFRL  (*((volatile unsigned long *) 0x48000420)) //GPIO B Alternate func register
#define GPIOB_ODR		(*((volatile unsigned long *) 0x48000414)) //GPIO B Output data reg
	
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
#define TIM2_CCMR2 (*((volatile unsigned long *) 0x4000001C)) //TIM2 capture/compare mode register
#define TIM2_PSC   (*((volatile unsigned long *) 0x40000028)) //TIM2 event generation register
#define TIM2_ARR   (*((volatile unsigned long *) 0x4000002C)) //TIM2 auto-reload register
#define TIM2_CCR1  (*((volatile unsigned long *) 0x40000034)) //TIM2 capture/compare register
#define TIM2_CCR2  (*((volatile unsigned long *) 0x40000038)) //TIM2 capture/compare register
#define TIM2_CCR3  (*((volatile unsigned long *) 0x4000003C)) //TIM2 capture/compare register
#define TIM2_CCR4  (*((volatile unsigned long *) 0x40000040)) //TIM2 capture/compare register
#define TIM2_CCER  (*((volatile unsigned long *) 0x40000020)) //TIM2 capture/compare enable register
#define TIM2_DIER  (*((volatile unsigned long *) 0x4000000C)) //TIM2 interrupt enable register		
#define TIM2_SR    (*((volatile unsigned long *) 0x40000010)) //TIM2 status register
	
//I2C1 starts 0x4000 5400
#define I2C1_CR1     (*((volatile unsigned long *) 0x40005400)) //I2C1 status register
#define I2C1_CR2     (*((volatile unsigned long *) 0x40005404)) //I2C1 status register
#define I2C1_TIMINGR (*((volatile unsigned long *) 0x40005410)) //I2C1 timing register
#define I2C1_ISR     (*((volatile unsigned long *) 0x40005418)) //I2C1 interrupt and starts register
#define I2C1_ICR     (*((volatile unsigned long *) 0x4000541C)) //I2C1 interrupt control register
#define I2C1_TXDR    (*((volatile unsigned long *) 0x40005428)) //I2C1 tranfer data register
#define I2C1_RXDR    (*((volatile unsigned long *) 0x40005424)) //I2C1 receive data register
#define I2C1_OAR1    (*((volatile unsigned long *) 0x40005408)) //I2C1 own address 1 register
#define I2C1_OAR2    (*((volatile unsigned long *) 0x4000540C)) //I2C1 own address 2 register

//I2C3 starts 0x4000 5C00
#define I2C3_CR1     (*((volatile unsigned long *) 0x40005C00)) //I2C3 status register
#define I2C3_CR2     (*((volatile unsigned long *) 0x40005C04)) //I2C3 status register
#define I2C3_TIMINGR (*((volatile unsigned long *) 0x40005C10)) //I2C3 timing register
#define I2C3_ISR     (*((volatile unsigned long *) 0x40005C18)) //I2C3 interrupt and starts register
#define I2C3_ICR     (*((volatile unsigned long *) 0x40005C1C)) //I2C3 interrupt control register
#define I2C3_TXDR    (*((volatile unsigned long *) 0x40005C28)) //I2C3 tranfer data register
#define I2C3_RXDR    (*((volatile unsigned long *) 0x40005C24)) //I2C3 receive data register
#define I2C3_OAR1    (*((volatile unsigned long *) 0x40005C08)) //I2C3 own address 1 register
#define I2C3_OAR2    (*((volatile unsigned long *) 0x40005C0C)) //I2C3 own address 2 register
	
//NVIC 0xE000 E100 programmer maunal
#define NVIC_ISER0   (*((volatile unsigned long *) 0xE000E100)) //Interrupt set enbale register 31-0
#define NVIC_ISER1   (*((volatile unsigned long *) 0xE000E104)) //Interrupt set enbale register 63-32
#define NVIC_ISER2   (*((volatile unsigned long *) 0xE000E108)) //Interrupt set enbale register 80-64
	
//SYSCFG 0x4001 0000
#define SYSCFG_EXTICR1    (*((volatile unsigned long *) 0x40010008)) //SYSCFG
	
//EXTI	0x4001 0400
#define EXTI_IMR1         (*((volatile unsigned long*) 0x40010400))	//EXTI_IMR1
#define EXTI_RTSR1        (*((volatile unsigned long*) 0x40010408))	//EXTI_RTSR1
#define EXTI_PR1          (*((volatile unsigned long*) 0x40010414))	//EXTI_PR1
	
//DMA 0x4002 0000
#define DMA1_CSELR          (*((volatile unsigned long*) 0x400200A8))
	
#define DMA1_Channel1_CCR   (*((volatile unsigned long*) 0x40020008))
#define DMA1_Channel1_CNDTR (*((volatile unsigned long*) 0x4002000C))
#define DMA1_Channel1_CPAR  (*((volatile unsigned long*) 0x40020010))
#define DMA1_Channel1_CMAR  (*((volatile unsigned long*) 0x40020014))
	
#define DMA1_Channel2_CCR   (*((volatile unsigned long*) 0x4002001C))
#define DMA1_Channel2_CNDTR (*((volatile unsigned long*) 0x40020020))
#define DMA1_Channel2_CPAR  (*((volatile unsigned long*) 0x40020024))
#define DMA1_Channel2_CMAR  (*((volatile unsigned long*) 0x40020028))



	
