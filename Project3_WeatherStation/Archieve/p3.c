
//WORKING DONE
//Project 3: Weather Station
//Kunal Mukherjee, Mason Mobley, Darren, Micheal, Muhammad
//4/24/2019

//Adding libraries
#include "stm32l432.h"
#include "string.h"
#include "stdint.h"
#include "stdio.h"
#include "stdlib.h"

//Initialization of GPIO pins
void GPIO_Init(void);

//I2C1 function definition
void I2C1_Initialize(void);

void I2C1_Start(unsigned long SlaveAddress,
								char size, 
								char direction);
								
void I2C1_Stop(void);
								
void I2C1_WaitLineIdle(void);
								
int I2C1_SendData(char SlaveAddress,
									char size,
									char * pData);
									
int I2C1_ReceiveData(char SlaveAddress,
										char size,
										char * pData);

//vem										
void VEM6070_Init(void);

//ADC										
void ADC_Init(void);
										
//USART
void USART1_Initialize(void);		
				
void GPIO_Initialize(void);		
							
void USART1_Read(char *buffer, int bytes);

void USART1_Write(char *buffer, int bytes);
										
//Timer2
void Timer2Init(void);

void Timer1Init(void);
										
//UART TO ASCII
void ASCII(int num, char special);

//GPIO ON/OFF
void turnSensorOn(void);

void turnSensorOff(void);

//SD card ON/OFF
void turnSDOn(void);

void turnSDOff(void);
										
#define VEML6070_ADDR_H  (0x39) // High address
#define VEML6070_ADDR_L  (0x38) // Low address										
#define VEML6070_UVS     (0x38) // High address		
							
#define UVS1  (*((volatile unsigned long *) 0x70))

#define MAX_BYTE_VAL 8
#define READ_BYTE 1										
										 
int main()
{
	int i,j;
	int UVvalue = 0, Lightvalue = 0, temperatureValue = 0;		
	char Data_Receive[MAX_BYTE_VAL];
	char Data_Send[MAX_BYTE_VAL];
	
	VEM6070_Init();
	GPIO_Init();	
	
	I2C1_Initialize();
	ADC_Init();
	USART1_Initialize();
	
	Timer2Init(); //1 hour Timer
	Timer1Init(); //30 sec Timer
	
	turnSDOff();
	turnSensorOff();
	
	while(1){	
	
		//start the timer
		TIM2_CR1 |= (1 << 0);
		//wait until timer up
		//while((TIM2_CR1 & 1) != 0);
		
		//clear the data received
		for (i = 0; i < MAX_BYTE_VAL; i++)
		{
			Data_Receive[i] = 0;
			Data_Send[i] = 0;
		}	
	
		turnSensorOn();
		
		//delay
		for (i = 0; i < 10000; i++)
		{
			for (j = 0; j < 100; j++);
		}
	
	  //Get the UV Sensor Data 
		I2C1_SendData(VEML6070_UVS, READ_BYTE, Data_Send);		
		
		I2C1_ReceiveData(VEML6070_ADDR_L, READ_BYTE, Data_Receive);
		UVvalue = Data_Receive[0];
		
		I2C1_ReceiveData(VEML6070_ADDR_H, READ_BYTE, Data_Receive);
		UVvalue = (Data_Receive[0] << 8) + UVvalue ;
		
		//Get the TMPT6000 Light sensor PA4
		ADC_CR |= (1 << 2); //start adc regular conversion
		while((ADC_ISR & (1 << 2)) == 0);//look at the EOC flag
		Lightvalue = ADC_DR & 0xFFF;			
		
		for (i = 0; i < 50000; i++);
		
		ADC_CR |= (1 << 2); //start adc regular conversion PA1
		while((ADC_ISR & (1 << 2)) == 0);//look at the EOC flag
		temperatureValue = ADC_DR & 0xFFF;
		
		//start the timer
		TIM1_CR1 |= (1 << 0);
		//wait until timer up
		while((TIM1_CR1 & 1) != 0);
		
		turnSDOn();
		
		ASCII(UVvalue, ' ');
		ASCII(Lightvalue, ' ');
		ASCII(temperatureValue, '-');	
		
		//start the timer
		TIM1_CR1 |= (1 << 0);
		//wait until timer up
		while((TIM1_CR1 & 1) != 0);		
		
		turnSDOff();

		//delay
		for (i = 0; i < 10000; i++);
	
		turnSensorOff();		
		
	}		
}

void ASCII(int num, char special)
{	
	int i;
	char pData1[MAX_BYTE_VAL];
	char pData2[MAX_BYTE_VAL];
	
	for (i = 0; i < MAX_BYTE_VAL; i++)
	{
			pData1[i] = 0;
			pData2[i] = 0;			
	}
	
  sprintf(pData1, "%d", num);	
	pData2[0] = special;
	
	USART1_Write(pData1,MAX_BYTE_VAL);
	USART1_Write(pData2,MAX_BYTE_VAL);		
}

void Timer2Init()
{
	RCC_APB1ENR1 |= (1 << 0);   //TIM2 clk enbale	
	
	TIM2_CR1 &= ~(1 << 0); //turn it off
	TIM2_CR1 |= (1 << 7);// enable auto-reload
	
	//For 1 hour timing
	TIM2_PSC = 3;         //1;//1874;  //Don't use prescaling 
	TIM2_ARR = 3599999999;//2399999999;//63999; //16MHz/16000 = 1000 Hz 			
	
	//start timer
	TIM2_CR1 |= (1 << 4); //DIR set to count down
	TIM2_CR1 |= (1 << 3); //DIR set to count down
	TIM2_EGR |= (1 << 0); //UG: update event 	
	TIM2_CR1 |= (1 << 0); //CEN: counter enabled
}

//30 second timer
void Timer1Init()
{
	RCC_APB2ENR |= (1 << 11);   //TIM1 clk enbale	
	
	TIM1_CR1 &= ~(1 << 0); //turn it off
	TIM1_CR1 |= (1 << 7);// enable auto-reload

	TIM1_PSC = 2000; //Don't use prescaling
	TIM1_ARR = 60000; //16MHz/16000 = 1000 Hz
	
	//start timer
	TIM1_CR1 |= ( 1 << 4); //DIR set to count down
	TIM1_CR1 |= ( 1 << 3); //DIR set to count down
	TIM1_EGR |= (1 << 0); //UG: update event 	
	TIM1_CR1 |= (1 << 0); //CEN: counter enabled
}

void GPIO_Init(void)
{
	RCC_AHB2ENR  |= (1 << 0);    //GPIOA clk enable
	RCC_AHB2ENR  |= (1 << 1);    //GPIOB clock enable bit
//	RCC_APB1ENR1 |= 1 << 19; // UART4 ENABLE 
	
	//enable pb3 for debug
	GPIOB_MODER &= ~(3 << (2 * 3)); 	
	GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output
	
	//set up PA9 as alternate func I2C1_SCL
	//set up PA10 as alternate func I2C1_SDA
	
	GPIOA_MODER   &= ~(3 << (2 * 9)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 9)); //set port PA9 is alternate 10	
	
	GPIOA_MODER   &= ~(3 << (2 * 10)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 10)); //set port PA10 is alternate 10
	
	GPIOA_AFRH    |=  (4 << (4 * 1)); //alt func 1, port pin 1, 
	                                //control bit are 4 bit wide
	GPIOA_AFRH    |=  (4 << (4 * 2)); //alt func 1, port pin 1, 
	                                //control bit are 4 bit wide
	
	GPIOA_OTYPER |= (1 << 9);  //pa9 as open-drain
	GPIOA_OTYPER |= (1 << 10); //pa10 as open-drain	
	
	//GPIOB_MODER &= ~(3 << (2 * 3)); 	
	//GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output
	
	// enable GPIO pin and configure the TX pin and the Rx pin as:
	// ALternate function, high speed, push-pull, pull-up 
	// USART1  PB6 = TX and PB.7 = RX
	GPIOB_MODER   &= ~(0xF <<(2*6)); // clr PB6 AND 7
	GPIOB_MODER   |= 0xA<< (2*6); // Altfunc PB6 AND 7 
	
	// Alternate function 7 = Usart1 
	// Appendix I shows all alternate functions 
	GPIOB_AFRL    |= 0x77 << (4*6); // set pB6 and 7 to AF 7 
	GPIOB_OSPEEDR |= 0xF<<(2*6); // HIGH SPEED ON PB6 AND 7 
	GPIOB_PUPDR   &= ~((0xF)<<(2*6));
	GPIOB_PUPDR   |= 0x5 << (2*6); // pull up on PB6 and 7 
	GPIOB_OTYPER  &= ~(0x3<<6); //PB6 and 7 open drain	
	RCC_APB2ENR |= 1 << 14;      // USART1 Enable 
	
	//setting up MOSFET
	//for Sensor
	GPIOA_MODER &= ~(3 << (2 * 5));//clear
	GPIOA_MODER |=  (1 << (2 * 5)); //PA5 Output
	//GPIOA_OTYPER |= (1 << 5);  //pA5 as open-drain
	
	//for SD
	GPIOA_MODER &= ~(3 << (2 * 6));//clear
	GPIOA_MODER |=  (1 << (2 * 6)); //PA6 Output
	//GPIOA_OTYPER |= (1 << 6); //pa6 as open-drain	

//THE COMMENTED OUT CODE ABOVE CAUSES THE SENSOR DATA NOT TO WRITE TO THE DATA LOGGER....
//LOOK INTO THIS. 
}

void turnSensorOff(void)
{	
	GPIOA_MODER &= ~(3 << (2 * 5));//input
	GPIOA_PUPDR &= ~(3 << (2 * 5));//input
	GPIOA_PUPDR |=  (2 << (2 * 5));//pull down
}

void turnSensorOn(void)
{
	GPIOA_MODER  &= ~(3 << (2 * 5));//clear
	GPIOA_MODER  |=  (1 << (2 * 5)); //PA6 Output
	GPIOA_OTYPER |=  (1 << 5); //pa6 as open-drain
	
	GPIOA_ODR |= (1 << 5);	
}

void turnSDOff(void)
{			
	GPIOA_MODER &= ~(3 << (2 * 6));//input
	GPIOA_PUPDR &= ~(3 << (2 * 6));//input
	GPIOA_PUPDR |=  (2 << (2 * 6));//pull down
	
}

void turnSDOn(void)
{	
	GPIOA_MODER  &= ~(3 << (2 * 6));//clear
	GPIOA_MODER  |=  (1 << (2 * 6)); //PA6 Output
	GPIOA_OTYPER |= (1 << 6); //pa6 as open-drain
	
	GPIOA_ODR |= (1 << 6);
}

void I2C1_Initialize(void)
{
	RCC_APB1ENR1 |= 1 << 21; //I2C1 clock enable
	
	I2C1_CR1 &= ~I2C_CR1_PE;
	
	//using the STM32F0-F3_AN4235_V1.0.1 to get the timing information
	//Device Mode- Master
	//I2C Speed- Fast Mode
	//I2C Speed frequency 400 Hz
	//Rise Time = 100 ns
	//Fall Time = 10 ns
	I2C1_TIMINGR = (uint32_t) 0x0010020A;
	
	I2C1_CR1 |= I2C_CR1_PE; //peripheral enbale
}


void VEM6070_Init(void)
{
	//Initializing VEM6070
	UVS1 = 0x06;
}

void I2C1_Start(unsigned long SlaveAddress,
								char size, 
								char direction)
{
	
	unsigned long temp = I2C1_CR2; 
	
	temp &= 0xFC009800; //clear SADD, NBYTES, RELOAD, AUTOEND, RD_WRN, START, STOP
	
	if(direction == MASTER_READ){
		temp |= I2C_CR2_WRN; //Transfer direction (master mode)		
	}
	else{ //master write
			temp &= ~(unsigned long)I2C_CR2_WRN; //0: Master requests a write transfer
	}
	
	temp |= SlaveAddress << 1; //SADD[7:1]: Slave address bit 7:1 (master mode)
	temp |= (unsigned long) size << 16; //NBYTES[7:0] num of bytes
	temp |= I2C_CR2_START; //Bit 13 START: Start generation
	                       //cleared by software by writing ‘1’ 
	                       //to the ADDRCF bit in the I2C_ICR register
	I2C1_CR2 = temp;	
}

void I2C1_Stop(void)
{
	//generate the STOP bit after the current byte has been transferred 
	I2C1_CR2 |= I2C_CR2_STOP;
	//wait while stopf flag is reset
	while ((I2C1_ISR & I2C_ISR_STOPF) == 0);
	
	I2C1_ICR |= I2C_ICR_STOPCF; //write 1 to clear STOPF flag	
}

void I2C1_WaitLineIdle(void)
{
	while ((I2C1_ISR & I2C_ISR_BUSY) == I2C_ISR_BUSY); //if busy wait
}

int I2C1_SendData(char SlaveAddress,
				  char size,
				  char * pData)
{
	int i;
	
	if (size <= 0 || pData == NULL) return -1;
	
	//wait until the line is idle
	I2C1_WaitLineIdle();
	
	I2C1_Start(SlaveAddress, size, MASTER_WRITE);
	
	for (i = 0; i < (int)size; i++)
	{
		//wait for Transmit register empty to be empty
		while((I2C1_ISR & I2C_ISR_TXIS) == 0);
		//TXIS is cleared by writing to the TXDR reg
		I2C1_TXDR = pData[i];		
	}	
	
	//wait intil TC transfer complete flag is set
	while ((I2C1_ISR & I2C_ISR_TC) == 0 &&
					(I2C1_ISR & I2C_ISR_NACKF) == 0);
					
	if (( I2C1_ISR & I2C_ISR_NACKF) != 0)
		return -1;
	
	//send the stop signal
	I2C1_Stop();
	
	return 0;
}

int I2C1_ReceiveData(char SlaveAddress,
					 char size,
					 char * pData)
{
	int i;
	
	if (size <= 0 || pData == NULL) return -1;
	
	//wait until the line is idle
	I2C1_WaitLineIdle();
	
	I2C1_Start(SlaveAddress, size, MASTER_READ);
	
	for(i = 0; i < size; i++)
	{
		//wait until receive data register not empty flag set
		while((I2C1_ISR & I2C_ISR_RXNE) == 0);
		pData[i] = I2C1_RXDR;
	}
	
	//wait until TCR flag is set
	while ((I2C1_ISR & I2C_ISR_TC) == 0);
	
	//send the stop signal
	I2C1_Stop();
	
	return 0;		
}

void ADC_Init(void)
{
	int i;	
	
	RCC_AHB2ENR |= (1 << 13); //set ADC clk	
	
	//mode default to analog
	GPIOA_PUPDR &= ~(1  << 4); //PA4 to no pull or down
	GPIOA_PUPDR &= ~(1  << 1); //PA4 to no pull or down
	
	ADC_CR &= ~(1 << 29); //deep power mode cleared
	ADC_CR |= (1 << 28); //set voltage reg
	
	for (i=0; i <10000; i++);//wait for .5us
	
	ADC_CCR |= (1 << 22); //VREF ENAB
	ADC_CCR |= (1 << 16); //HCLK/1 (Synchronous clock mode) enb
	ADC_ISR |= (1 << 0); //ADC ready
	
	ADC_CR |= (1 << 0); //ENB ADC
	
	while ((ADC_ISR & (1 << 0)) == 0); //wait till ADC is ready
	
	ADC_SQR1 |= (1 << 0);//Channel length 2	
	ADC_SQR1 |= (9 << 6);//CH9 
	ADC_SQR1 |= (6 << 12);//CH6
	
	ADC_CFGR |= (1 << 16); //DISCEN: Discontinuous mode for regular channels
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
		for(j = 0;j<500000;j++);
	}
	
	// WAIT UNTIL TC BIT IS SET. TC IS SET BY 
	//HARDWARE AND CLEARED BY SOFTWARE 
	// TC:TRANSMISSION COMPLETE FLAG 
	while(!(USART1_ISR & USART_ISR_TC));	
	
	// WRITING 1 TO THE TCCF BIT IN ICR CLEARS THE TC BIT IN ISR 
	USART1_ISR &= ~(USART_ISR_TC);
	
	// TCCF:TRANSMISSION COMPLETE CLEAR FLAG 
	USART1_ICR |= USART_ICR_TCCF; 
}
