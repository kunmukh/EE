//Kunal Mukherjee;
//3/26/2019
//Project 2: HI2C

#include "stm32l432.h"
#include "string.h"
#include "stdint.h"

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
										
//I2C3 function definition
void I2C3_Initialize(unsigned long slaveAddress);
								
void I2C3_Start(unsigned long SlaveAddress,
								char size, 
								char direction);
								
void I2C3_Stop(void);
								
void I2C3_WaitLineIdle(void);
								
int I2C3_SendData(char SlaveAddress,
									char size,
									char * pData);
									
int I2C3_ReceiveData(char SlaveAddress,
										 char size,
										 char * pData);

//LED Blink 
void LEDBlink (int number);
										 
int main()
{
	char Data_Receive[8];
	char Data_Send[8];	
	char num = 1;
	
	unsigned long slaveAddress = 0x53;
	int i;	
	
	Data_Send[0] = 0x03; 
	GPIO_Init();	
	I2C1_Initialize();
	I2C3_Initialize(slaveAddress);
	
	//setup the global interrupt
	NVIC_ISER2 |= (1 << 8); //I2C3_EV event interrupt 72,73
	NVIC_ISER2 |= (1 << 9); //I2C3_ER error interrupt
	
	while(1)
	{
		I2C1_ReceiveData(slaveAddress, num, Data_Send);			
		//or
		//I2C1_SendData(slaveAddress, num, Data_Send);	
		
		//delay
		for (i = 0; i < 1000; i++);		
	}	
	
	return 0;
}

void I2C3_EV_IRQHandler()
{
	//variables to hold the receive address and the direction
	char direction, ReceiveAddress;
	char data[6];
	//the data the slave sends out if the Master wants to receive
	data[0] = 0x05;
	
	//the index of the array that contains the data
	int num = 1;	
	
	//receive data register not empty
	if ((I2C3_ISR & I2C_ISR_RXNE) == I2C_ISR_RXNE) //master write, slave read
	{					
		I2C3_ReceiveData(ReceiveAddress, num, data);			
	}
	
	//check to see if slave was called		
	if ((I2C3_ISR & I2C_ISR_ADDR) == I2C_ISR_ADDR)
	{
		ReceiveAddress = (I2C3_ISR & I2C_ISR_ADDCODE) >> 17;
		direction = (I2C3_ISR & I2C_ISR_DIR) >> 16;	
		
		I2C3_SendData(ReceiveAddress, num, data);	
		
	}
	
		
	//disable the ADDR flag
	I2C3_ICR |= (1 << 3);
	return;
}

void GPIO_Init(void)
{
	RCC_AHB2ENR  |= (1 << 0);    //GPIOA clk enable
	RCC_AHB2ENR  |= (1 << 1);    //GPIOB clock enable bit
	
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
	
	GPIOB_MODER &= ~(3 << (2 * 3)); 	
	GPIOB_MODER |=  (1 << (2 * 3)); //PB3 Output
	
	//set up PA7 as alternate func I2C3_SCL
	//set up PB4 as alternate func I2C3_SDA
	
	GPIOA_MODER   &= ~(3 << (2 * 7)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 7)); //set port PA7 is alternate 10	
	
	GPIOB_MODER   &= ~(3 << (2 * 4)); //clear the GPIOB mode bits
	GPIOB_MODER   |=  (2 << (2 * 4)); //set port PB4 is alternate 10
	
	GPIOA_AFRL    &=  ~((unsigned long)(15 << (4 * 7)));
	GPIOA_AFRL    |=   (4  << (4 * 7)); //alt func 1, port pin 1, 
	                                //control bit are 4 bit wide
	
	GPIOB_AFRL    &=  ~(15 << (4 * 4));
	GPIOB_AFRL    |=   (4  << (4 * 4)); //alt func 1, port pin 1, 
	                                //control bit are 4 bit wide
	
	GPIOA_OTYPER |= (1 << 7);  //pa7 as open-drain
	GPIOB_OTYPER |= (1 << 4);  //pb4 as open-drain	
}

void I2C1_Initialize(void)
{
	RCC_APB1ENR1 |= 1 << 21; //I2C1 clock enable
	I2C1_CR1 &= ~I2C_CR1_PE;
	
	I2C1_TIMINGR |= 0 << 28; //Fpresc = Fi2c/(0+1) = 4 MHz
	I2C1_TIMINGR |= 5 << 20; //SCLDEL = 5 -> 1.5 us
	I2C1_TIMINGR |= 5 << 16; //SDADEL = 5 -> 1.5 us
	I2C1_TIMINGR |= 19 << 8; //SCLH = 19 -> Thigh = 5 us
	I2C1_TIMINGR |= 19; //SCLL = 19 -> Tlow = 5 us
	
	I2C1_CR1 |= (1 << 01); //transmit interrupt
	I2C1_CR1 |= (1 << 02); //receive interrupt	
	I2C1_CR1 |= (1 << 03); //address match interrupt
	I2C1_CR1 |= (1 << 05); //STOP detection interrupt
	I2C1_CR1 |= (1 << 06); //transmit complete interrupt
	
	I2C1_CR1 |= I2C_CR1_PE; //peripheral enbale
}

void I2C1_Start(unsigned long SlaveAddress,
								char size, 
								char direction)
{
	unsigned long temp = I2C1_CR2; 

		temp &= 0xFC009800; //clear SADD, NBYTES, RELOAD, AUTOEND, RD_WRN, START, STOP
	
	if(direction == MASTER_READ)
	{
		temp |= I2C_CR2_WRN; //Transfer direction (master mode)		
	}
	else
	{
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

void I2C3_Initialize(unsigned long slaveAddress)
{
	RCC_APB1ENR1 |= 1 << 23; //I2C3 clock enable	
	I2C3_CR1 &= ~I2C_CR1_PE;	
	
	//enabling the interrupt
	I2C3_CR1 |= (1 << 01); //transmit interrupt
	I2C3_CR1 |= (1 << 02); //receive interrupt	
	I2C3_CR1 |= (1 << 03); //address match interrupt
	I2C3_CR1 |= (1 << 05); //STOP detection interrupt
	I2C3_CR1 |= (1 << 06); //transmit complete interrupt	
	I2C3_CR1 |= (1 << 16); //slave byte control
	I2C3_CR1 |= (1 << 17); //clock stret disable	
	
	I2C3_OAR1 |= (slaveAddress << 1); //ERROR [7:1] own address
	
	I2C3_OAR1 |= (1 << 15); //own slave address enabled 
	                        //receive slave address ACKed
	
	I2C3_CR2 |= I2C_CR2_NACK;
	
	I2C3_CR1 |= I2C_CR1_PE; //peripheral enbale
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
	
	//blink LED
	LEDBlink(pData[0]);
	
	//wait until TCR flag is set
	while ((I2C1_ISR & I2C_ISR_TC) == 0);
	
	I2C1_Stop();
	
	return 0;		
}

/*void I2C3_Start(unsigned long SlaveAddress,
								char size, 
								char direction)
{
	unsigned long temp = I2C3_CR2; 

		temp &= 0xFC009800; //clear SADD, NBYTES, RELOAD, AUTOEND, RD_WRN, START, STOP
	
	if(direction == MASTER_READ)
	{
		temp |= I2C_CR2_WRN;		
	}
	else
	{
			temp &= ~(unsigned long)I2C_CR2_WRN;
	}
	
	temp |= SlaveAddress << 1; 
	temp |= (unsigned long) size << 16;
	temp |= I2C_CR2_START;
	
	I2C3_CR2 = temp;
}*/

/*void I2C3_Stop(void)
{
	//generate the STOP bit after the current byte has been transferred 
	I2C3_CR2 |= I2C_CR2_STOP;
	
	//wait while stopf flag is reset
	//while ((I2C3_ISR & I2C_ISR_STOPF) == 0);
	
	I2C3_ICR |= I2C_ICR_STOPCF; //write 1 to clear STOPF flag	
}*/

/*void I2C3_WaitLineIdle(void)
{	
	while ((I2C3_ISR & I2C_ISR_BUSY) == I2C_ISR_BUSY); //if busy wait
}*/


int I2C3_SendData(char SlaveAddress,
									char size,
									char * pData)
{
	int i;
	if (size <= 0 || pData == NULL) return -1;
	
	for (i = 0; i < (int)size; i++)
	{
		
		while((I2C3_ISR & I2C_ISR_TXIS) == 0);
		//TXIS is cleared by writing to the TXDR reg
		I2C3_TXDR = pData[i];		
	}	
	
	return 0;
}		

int I2C3_ReceiveData(char SlaveAddress,
										char size,
										char * pData)
{
	int i;
	if (size <= 0 || pData == NULL) return -1;	
	
	for(i = 0; i < size; i++)
	{				
		pData[i] = I2C3_RXDR;
	}
	
	//blink LED
	LEDBlink(pData[0]);	
	
	return 0;		
}

void LEDBlink (int number)
{
	int i, j, k;
	
	for (i = 0; i < number; i++)
	{
		GPIOB_ODR |= (1 << 3);
		for (j = 0; j < 10000; j++){for (k = 0; k < 20; k++);}; //delay
		GPIOB_ODR &= ~(1 << 3);
		for (j = 0; j < 10000; j++){for (k = 0; k < 20; k++);}; //delay		
	}
}