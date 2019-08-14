#include <stdio.h>
#include <stdlib.h>

void printChar(unsigned char);
void makeHrGlass(void);
void printHRglass();
void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state);
unsigned char GetLedStatus(unsigned char row,
                           unsigned char column);
void DoLED(unsigned char r, unsigned char c);
unsigned char getUpDn (void);
void makeInitialGenUp(void);
void makeInitialGenDown(void);
unsigned char checkStatus(unsigned char row,
                        unsigned char col);

//global variables
unsigned char LED[16];
unsigned char down = 0;
unsigned char up = 1;
unsigned char flat = 2;

int main()
{
    int i, user, ledRow, ledCol;

    for (i = 0; i <16; i++)
	{
		LED[i] = 0x00;
	}


	if (getUpDn() == down)
        makeInitialGenUp();
    else
        makeInitialGenDown();


    //SetLed(0,1,1);
	makeHrGlass();

    printf("\n");

	printHRglass();

    scanf("%d",&user);

	while(user != 100)
    {
        if(getUpDn() == down) //Start at top and move down
		{
            for (ledRow = 15; ledRow >= 0; ledRow--)
			{
				for (ledCol = 0; ledCol <8; ledCol++)
				{
					DoLED(ledRow,ledCol);
				}
			}
		}
		else //STart at bottom and move up
		{
			for (ledRow = 0; ledRow < 16; ledRow++)
			{
				for (ledCol = 0; ledCol < 8; ledCol++)
				{
					DoLED(ledRow, ledCol);
				}
			}
		}
		while(user == 200) //if stopped wait here
        {
            scanf("%d",&user);
        }

        printHRglass();

        //scanf("%d",&user);
        if(user < 25)
            user++;
        else
            user = 100;
    }

    return 0;
}

void DoLED(unsigned char r, unsigned char c)
	{

		//printf("r: %i c: %i status:%i\n", r,c, GetLedStatus(r,c));
		if(getUpDn() != flat)
		{
			if (getUpDn()== down)
			{
				//look at the bottom LED
				//Move LED at (r,c) accordign to algirithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r+1 < 16) &&
                    (GetLedStatus(r+1,c) == 0) &&
                    (checkStatus(r+1,c) != 0))
				{
					SetLed(r+1, c, 1);
					SetLed(r  , c, 0);
				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r+1 < 16) &&
                        (c-1 >= 0) &&
                        (GetLedStatus(r+1,c - 1) == 0) &&
                        (checkStatus(r+1,c - 1) != 0))
					{
						SetLed(r+1, c - 1, 1);
						SetLed(r  , c, 0);
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r+1 < 16)&&
                            (c+1 <= 7)&&
                            (GetLedStatus(r+1,c + 1) == 0) &&
                            (checkStatus(r+1,c + 1) != 0))
					{
						SetLed(r+1, c + 1, 1);
						SetLed(r  , c, 0);
					}
				}
			}
			else
			{
				//look at the top LED
				//Move LED at (r,c) accordign to algirithm
				//if not fill, move led
				if((GetLedStatus(r,c) != 0) &&
                    (r-1 >= 0) &&
                    (GetLedStatus(r-1,c) == 0) &&
                    (checkStatus(r-1,c) != 0))
				{
					SetLed(r-1, c, 1);
					SetLed(r  , c, 0);
				}
				else
				{
					//look at bottom left
					//if not fill, move led
					if((GetLedStatus(r,c) != 0) &&
                        (r-1 >= 0) &&
                        (c >= 0) &&
                        (GetLedStatus(r-1,c - 1) == 0) &&
                        (checkStatus(r-1,c - 1) != 0))
					{
						SetLed(r-1, c - 1, 1);
						SetLed(r  , c, 0);
					}
				//look at bottom right
				//if not fill, move led
					else if((GetLedStatus(r,c) != 0) &&
                            (r-1 >= 0) &&
                            (c <= 7) &&
                            (GetLedStatus(r-1,c + 1) == 0) &&
                            (checkStatus(r-1,c+1) != 0))
					{
						SetLed(r-1, c + 1, 1);
						SetLed(r  , c, 0);
					}
				}
			}
		}
	}

unsigned char checkStatus(unsigned char row,
                        unsigned char col)
{
   if ((row == 5) || (row == 6) || (row == 7) ||
       (row == 8) || (row == 9) || (row == 10))
   {
       if((col == 0) || (col == 7))
       {
           return 0;
       }
   }

   if  ((row == 6) || (row == 7) ||
        (row == 8) || (row == 9))
    {
        if((col == 6) || (col == 1))
       {
           return 0;
       }
    }

    if((row == 7) ||
        (row == 8))
    {
        if((col == 5) || (col == 2))
       {
           return 0;
       }
    }

    return 1;

}

unsigned char GetLedStatus(unsigned char row,
                           unsigned char column)
{
	unsigned char temp = LED[row];

	return temp & (1 << column);
}

unsigned char getUpDn (void)
{
    return down;
}

void SetLed(unsigned char row,
            unsigned char column,
            unsigned char state)
{
	if(state)
		LED[row] |= (1 << column);
	else
		LED[row] &= ~(1 << column);
}

void makeInitialGenUp(void)
{
    int i , j;
    for (i = 0; i < 8; i++)
    {
        for (j = 0; j < 7; j++)
        {
            SetLed(j,i,1);
        }
    }
}

void makeInitialGenDown(void)
{
    int i , j;
    for (i = 0; i < 8; i++)
    {
        for (j = 15; j > 8; j--)
        {
            SetLed(j,i,1);
        }
    }
}

void makeHrGlass(void)
	{

		SetLed(5,7, 0);
		SetLed(6,7, 0);
		SetLed(7,7, 0);
		SetLed(8,7, 0);
		SetLed(9,7, 0);
		SetLed(10,7, 0);

		SetLed(5,0, 0);
		SetLed(6,0, 0);
		SetLed(7,0, 0);
		SetLed(8,0, 0);
		SetLed(9,0, 0);
		SetLed(10,0, 0);

		SetLed(6,6, 0);
		SetLed(7,6, 0);
		SetLed(8,6, 0);
		SetLed(9,6, 0);

		SetLed(6,1, 0);
		SetLed(7,1, 0);
		SetLed(8,1, 0);
		SetLed(9,1, 0);

		SetLed(7,5, 0);
		SetLed(8,5, 0);

		SetLed(7,2, 0);
		SetLed(8,2, 0);
	}

void printHRglass()
{
    int i;
    for (i = 0; i <16; i++)
	{
		printChar(LED[i]);
		printf("\n");
	}
	printf("\n");
}
void printChar(unsigned char a)
{
    int i;
    for (i = 0; i < 8; i++)
    {
      printf("%d", !!((a << i) & 0x80));
    }

}
