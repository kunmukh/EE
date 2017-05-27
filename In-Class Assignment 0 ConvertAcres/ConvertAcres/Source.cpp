//Kunal Mukherjee//
//30 Aug, 2016
//In Class Assignment 1
// This pprogram converts acers to equivalent square miles //
#include<stdio.h>
#define ACER_PER_SQUAREMILE 640

int main(void)
{
double acer;
double squaremile;
printf("Enter the number of Acer -->\t");
scanf_s("%lf",&acer);
squaremile = acer / ACER_PER_SQUAREMILE;
printf(" The required square mile is --> %f \n", squaremile);
return (0);
}
