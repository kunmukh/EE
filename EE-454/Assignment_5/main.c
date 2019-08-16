//Kunal Mukherjee;
//3/7/2019
//Assignment 5: I2C Master Out

#include "stm32l432.h"
#include "string.h"

void GPIO_Init(void);

void I2C1_Initialize(void);
void I2C1_Start(unsigned long SlaveAddress,
								char size, 
								char direction);
								
void I2C1_Stop(void);
void I2C1_WaitLineIdle(void);
								
int I2C1_SendData(char SlaveAddress,
									char size,
									char * pData);

int main()
{
	int num = 8;
	char data[8] = "5";	
	
	unsigned char slaveAddress = 0x53;
	int i;
	
	GPIO_Init();
	I2C1_Initialize();
	
	while(1)
	{
		I2C1_SendData(slaveAddress, num, data);		
		
		for (i = 0; i < 1000; i++);
	}	
}

void GPIO_Init(void)
{
	RCC_AHB2ENR  |= (1 << 0);    //GPIOA clk enable
	
	//set up PA9 as alternate func I2C1_SCL
	//set up PA10 as alternate func I2C1_SDA
	
	GPIOA_MODER   &= ~(3 << (2 * 9)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 9)); //set port PA9 is alternate 10	
	
	GPIOA_MODER   &= ~(3 << (2 * 10)); //clear the GPIOA mode bits
	GPIOA_MODER   |=  (2 << (2 * 10)); //set port PA10 is alternate 10
	
	GPIOA_AFRH    |=  (4 << (4 * 1)); //alt func 4, port pin 1, 
	                                //control bit are 4 bit wide
	
	GPIOA_AFRH    |=  (4 << (4 * 2)); //alt func 4, port pin 1, 
	                                //control bit are 4 bit wide
	
	GPIOA_OTYPER |= (1 << 9);  //pa9 as open-drain
	GPIOA_OTYPER |= (1 << 10); //pa10 as open-drain	
	
	GPIOA_PUPDR  &= ~(3 << (2 * 9)); //no pull-up/pull-dn
	GPIOA_PUPDR  &= ~(3 << (2 * 10)); //no pull-up/pull-dn
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
	
	I2C1_CR1 |= I2C_CR1_PE; //peripheral enbale	
}

void I2C1_Start(unsigned long SlaveAddress,
								char size, 
								char direction)
{
	unsigned long temp = I2C1_CR2; 

	temp &= 0xFC009800; //clear SADD, NBYTES, RELOAD, AUTOEND, RD_WRN, START, STOP
	
	if(direction == READ)
	{
		temp |= I2C_CR2_WRN;		
	}
	else
	{
			temp &= ~(unsigned long)I2C_CR2_WRN;
	}
	
	temp |= SlaveAddress << 1; //why the shift of one
	temp |= (unsigned long) size << 16;
	temp |= I2C_CR2_START;
	
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
	
	I2C1_Start(SlaveAddress, size, WRITE);
	
	for (i = 0; i < (int)size; i++)
	{
		//wait for Tx to be empty
		while((I2C1_ISR & I2C_ISR_TXE) == 0);
		//TXIS is cleared by writing to the TXDR reg
		I2C1_TXDR = pData[i];		
	}
	
	//wait intil TC flag is set
	while ((I2C1_ISR & I2C_ISR_TC) == 0 &&
					(I2C1_ISR & I2C_ISR_NACKF) == 0);
	
	if (( I2C1_ISR & I2C_ISR_NACKF) != 0)
		return -1;
	
	I2C1_Stop();
	
	return 0;
}

