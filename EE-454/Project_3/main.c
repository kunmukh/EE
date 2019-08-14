//Kunal Mukherjee;
//4/24/2019
//Project 3: UTPIDPWMOC

#include "stm32l432.h"
#include "string.h"
#include "stdint.h"
#include "stdio.h"
#include "stdlib.h"
#include "time.h"

//GPIO Initialize
void GPIO_Init(void);

//USART
void USART1_Initialize(void);	
							
void USART1_Read(char *buffer, int bytes);

void USART1_Write(char *buffer, int bytes);

//USART functions
void USART1_ClearScreen(void);

void USART1_WriteValue(float Kp,float Ki,float Kd);

//ADC
void ADC_Init(void);

//PWM Timer
void PWM_Init(void);
	
#define MAX_BYTE_VAL 32
#define WAIT_TIME 500000

#define TARGET 1577
#define MAX_ADC_VALUE 4096
#define MAX_ARR_VALUE 4000

#define KP 1.25  //2.4//2.3//1.2
#define KI 0
#define KD -0.175 //0.123 //0.69 //0.5
#define T 10

#define DELAY 5000 
#define DELAY2 100
#define DELAY3 5000

int main()

{
	int i,j;
	float Kp = 0, Kd = 0, Ki = 0;
	float derivative = 0,  integral = 0;
	int position = 0, target = 0, error = 0, error_old = 0;
	float output = 0, tmr2 = 0;	
	int adcCaptureFlag = 0, kValueChangeFlag = 0;
	int Kselection;
	
	target = TARGET;
	Kp = KP;
	Ki = KI;
	Kd = KD;
	
	GPIO_Init();
	ADC_Init();
	PWM_Init();
	USART1_Initialize();	
	
	USART1_WriteValue(Kp, Ki, Kd);

	while(!adcCaptureFlag)
	{
		if ((GPIOA_IDR & (1 << 8)) == 0)
		{
			ADC_CR |= (1 << 2); //start adc regular conversion
			while((ADC_ISR & (1 << 2)) == 0);//look at the EOC flag
			target = ADC_DR & 0xFFF;
			
			adcCaptureFlag = 1;
		}
		
		for (i = 0; i < DELAY3; i++);
	}
	
	while(1)
	{

		ADC_CR |= (1 << 2); //start adc regular conversion
		while((ADC_ISR & (1 << 2)) == 0);//look at the EOC flag
		position = ADC_DR & 0xFFF;	
		
		error_old = error;
		error = target - position;		
		integral += error;
		
		derivative = error - error_old;
		
		output = output + (Kp * error) + (Ki * ( T * integral)) + ((Kd/T) * derivative);		
		//output =  output + Kp * error + (Kd/T) * derivative;			
		
		tmr2 = ((float) ((float) output / (float) MAX_ADC_VALUE) * (float) MAX_ARR_VALUE);	

		//tmr2 = MAX_ARR_VALUE;
		
		if (tmr2 < 0)
		{
			tmr2 = 0;
		}	
		if (tmr2 > MAX_ARR_VALUE)
		{
			tmr2 = MAX_ARR_VALUE;
		}

		TIM2_CCR2 = tmr2;
		
		//if constant value is change write it to the screen
		if (kValueChangeFlag == 1)
		{
			USART1_WriteValue(Kp, Ki, Kd);
			kValueChangeFlag = 0;
		}

		//use PA# and PA# to control which constant to select
		if ((GPIOA_IDR & (1 << 4)) == 0)
		{			
			Kselection++;
			
			if(Kselection > 2)
			{
				Kselection = 2;
			}
			for (i = 0; i < DELAY3; i++);
		}
		if ((GPIOA_IDR & (1 << 5)) == 0)
		{			
			Kselection--;
			
			if(Kselection < 0)
			{
				Kselection = 0;
			}
			for (i = 0; i < DELAY3; i++);
		}
		
		//use PA# and PA# to increase or decr a const
		if ((GPIOA_IDR & (1 << 3)) == 0)
		{			
			if(Kselection == 0)
			{
				Kp += .10;
			}
			if(Kselection == 1)
			{
				Ki += .10;
			}
			if(Kselection == 2)
			{
				Kd += .10;
			}
			
			kValueChangeFlag = 1;
				
			for (i = 0; i < DELAY3; i++);
		}
		
		if ((GPIOA_IDR & (1 << 7)) == 0)
		{			
			if(Kselection == 0)
			{
				Kp -= .10;
			}
			if(Kselection == 1)
			{
				Ki -= .10;
			}
			if(Kselection == 2)
			{
				Kd -= .10;
			}
				
			kValueChangeFlag = 1;
			
			for (i = 0; i < DELAY3; i++);
		}
			
		
		for (i =0; i < DELAY; i++)
		{
			for (j =0; j < DELAY2; j++);
		}
	}
	
}

void GPIO_Init(void)
{
	RCC_AHB2ENR  |= (1 << 0);    //GPIOA clk enable
	RCC_AHB2ENR  |= (1 << 1);    //GPIOB clock enable bit	
	RCC_APB2ENR |= 1 << 14;      // USART1 Enable	
	
	//enable pb3 for debug
	//GPIOB_MODER &= ~(3 << (2 * 3)); 	
	//GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output

	
	// enable GPIO pin and configure the TX pin and the Rx pin as:
	// ALternate function, high speed, push-pull, pull-up 
	// USART1  PB6 = TX and PB.7 = RX
	GPIOB_MODER   &= ~(0xF << (2*6)); // clr PB6 AND 7
	GPIOB_MODER   |=  (0xA << (2*6)); // Altfunc PB6 AND 7 
	
	// Alternate function 7 = Usart1 
	// Appendix I shows all alternate functions 
	GPIOB_AFRL    |=   (0x77 << (4*6)); // set pB6 and 7 to AF 7 
	GPIOB_OSPEEDR |=   (0xF  << (2*6)); // HIGH SPEED ON PB6 AND 7 
	GPIOB_PUPDR   &= ~((0xF) << (2*6));
	GPIOB_PUPDR   |=   (0x5  << (2*6)); // pull up on PB6 and 7 
	GPIOB_OTYPER  &=  ~(0x3  <<  6); //PB6 and 7 open drain 
	
	//PA8 ADC_CAPTURE
	GPIOA_MODER &= ~(3 << (2 * 8)); 	
	GPIOA_PUPDR |=  (2 << (2 * 8));
	
	GPIOA_MODER &= ~(3 << (2 * 4)); 	
	GPIOA_PUPDR |=  (2 << (2 * 4));
	
	GPIOA_MODER &= ~(3 << (2 * 5)); 	
	GPIOA_PUPDR |=  (2 << (2 * 5));
	
	GPIOA_MODER &= ~(3 << (2 * 3)); 	
	GPIOA_PUPDR |=  (2 << (2 * 3));
	
	GPIOA_MODER &= ~(3 << (2 * 7)); 	
	GPIOA_PUPDR |=  (2 << (2 * 7));
}

void USART1_ClearScreen()
{
	char pData[2] = "|-";
		
	USART1_Write(pData,2);
}

void USART1_WriteValue(float Kp,float Ki,float Kd)
{
	USART1_ClearScreen();
	
	int i;	
	
	char pDataKp[5], pDataKi[5], pDataKd[5];
	
	sprintf(pDataKp, "%.2f", Kp);
	sprintf(pDataKi, "%.2f", Ki);
	sprintf(pDataKd, "%.2f", Kd);
		
	USART1_Write(pDataKp,5);
	USART1_Write(pDataKi,5);
	USART1_Write(pDataKd,5);
}

// USART1 initialize
void USART1_Initialize()
{
	USART1_CR1 &= ~(1<<0); // DISABLE USART 
	
	// SET DATA LENGTH TO 8 BITS 
	// 00 = 8 DATA BITS, 01 = 9 DATA BITS ,10 = 7 DATA BITS 
	USART1_CR1 &= ~ (1 << 12); //M1 
	USART1_CR1 &= ~(1 << 28);	// M0
	
	// SELECT1 STOP BIT 
	// 00 = 1 STOPBIT 01 = 0.5 STOP BIT
	// 10 = 2 STOPBITS 11 = 1.5 STOP BIT 
	USART1_CR2 &= ~(0x3 << 12);
	
	// SET PARITY CONTROL AS NO PARITY 
	//0 = NO PARITY 
	// 1 = PARITY ENABLE (THEN PROGRAM PS BIT TO SELECT EVEN OR ODD PARITY)
	USART1_CR1 &= ~(1 << 10);
	
	// OVERSAMPLING BY 16 
	// 0 = OVERSAMPLING BY 16, 1 = OVERSAMPLING BY 8 
	USART1_CR1 &= ~(1 << 15);
	
	// SET BAUD RATE TO 9600 USING APB FREQUENCY (80 MHZ)
	// SEE EXAMPLE 1 IN SECTION 22.1.2
	USART1_BRR = 0X1A1; 
	
	// ENABLE USART
	USART1_CR1 |= (USART_CR1_TE | USART_CR1_RE); //  transmitter and reciever
	
	//  ENABLE USART 
	USART1_CR1 |= 1<<0; 
	
	// VERIFY THAT USART IS READY FOR TRANSMISSION 
	// TEACK: TRANSMIT ENABLE ACKNOWLEDGE FLAG. HARDWARE SETS OR RESETS IT. 
	while ((USART1_ISR & USART_ISR_TEACK) == 0);
	
	// VERIFY THAT USART IS READY FOR RECEPTION 
	//REACK : RECIEVE ENABLE ACKNOWLEDGE FLAG. HARDWARE SETS OR RESETS IT. 
	while((USART1_ISR & USART_ISR_REACK) == 0);
}

// USART1 READ FUNCTION 
void USART1_Read(char *buffer, int bytes)
{
	int i; 
	
	for(i = 0; i < bytes; i++)
	{	
		// WAIT UNTIL HARDWARE SETS RXNE
		while(!(USART1_ISR & USART_ISR_RXNE)); 
		buffer[i] = USART1_RDR;
	}
}

// USART1 WRITE FUNCTION 
void USART1_Write(char *buffer, int bytes)
{
	int i,j; 
	
	for(i = 0; i < bytes; i++)
	{
		// WRITING TO TDR CLEARS TXE FLAG
		USART1_TDR = buffer[i] & 0XFF;
		
		// WAIT UNTIL HARDWARE SETS TXE
		while(!(USART1_ISR & USART_ISR_TXE)); 
		
		//delay
		for(j = 0;j<WAIT_TIME;j++);
	}
	
	// WAIT UNTIL TC BIT IS SET. TC IS SET BY HARDWARE AND CLEARED BY SOFTWARE 
	// TC:TRANSMISSION COMPLETE FLAG 
	while(!(USART1_ISR & USART_ISR_TC));	
	
	// WRITING 1 TO THE TCCF BIT IN ICR CLEARS THE TC BIT IN ISR 
	USART1_ISR &= ~(USART_ISR_TC);
	
	// TCCF:TRANSMISSION COMPLETE CLEAR FLAG 
	USART1_ICR |= USART_ICR_TCCF; 
}

void ADC_Init(void)
{
	int i;	
	
	RCC_AHB2ENR |= (1 << 13); //set ADC clk	
	
	//mode default to analog
	GPIOA_PUPDR &= ~(1  << 1); //PA1 to no pull or down
	
	ADC_CR &= ~(1 << 29); //deep power mode cleared
	ADC_CR |=  (1 << 28); //set voltage reg
	
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

void PWM_Init(void)
{
	RCC_AHB2ENR  |= (1 << 1); //set the GPIOB clk
	RCC_APB1ENR1 |= (1 << 0); //TIM2 enb
	
	GPIOB_MODER &= ~(3 << (2 * 3)); //clear the GPIOB mode bits
	GPIOB_MODER |=  (2 << (2 * 3));  //set port b3 is alternate 10
	GPIOB_AFRL  |=  (1 << (4 * 3));   //alt func 1, port pin 3, 
	                                //control bit are 4 bit wide
	
	TIM2_CR1 |= (1 << 7); //ARPE: Auto-reload preload enable	
	TIM2_PSC = 0;         //PSC set to 0
	TIM2_ARR = MAX_ARR_VALUE;     //4MHz/4000= 1000 Hz = 1 ms
	
	TIM2_CCMR1 |= 0x6800; //Channel 2; 
	                      //bit 11: OC2PE: Output compare 2 preload enable
	                      //0110: PWM mode 1 - In upcounting, 
                        //channel 1 is active as long as 
	                       //TIMx_CNT<TIMx_CCR1else inactive.
	
	TIM2_CCER |= (1 << 4); //CC2E: Capture/Compare 2 output enable.
	
	TIM2_CCR2 |= 0;     //CCR2 is the value to be loaded in the actual 
	                       //capture/compare 2 register (preload value).
	
	TIM2_EGR |= (1 << 0); //UG: update event 
	
	TIM2_CR1 |= (1 << 0); //CEN: counter enabled
	
}

