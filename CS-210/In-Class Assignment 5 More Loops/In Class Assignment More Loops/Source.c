//Kunal Mukherjee
//CS 210
//More Loops
//13th Sept, 2016

#include<stdio.h>

int Findz(int x, int y);

int main()
{
    int x, y, z;
    for (x = 0; x < 5; x++)
    {
        for (y = 0; y < 3; y++)
        {
            z = Findz(x, y);
            printf("%d \n", z);
        }
   }
}

int Findz(int x, int y)
{
    if (x >= 0 && y >= 0)    
        return x*x +y*y;

    return 0;
}