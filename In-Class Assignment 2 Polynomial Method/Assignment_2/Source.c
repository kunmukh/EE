//Kunal Mukherjee
//1 August, 2016
//Assignment 2
// Polynimial method- It takes a value of y and prints out its result

#include<stdio.h>
#include<math.h>

double FindY (double x);

void main()
{
    double y;
    double x;
    printf("Enter a value of x--> \t");
    scanf_s("%lf", &x);
    y = FindY(x);
    printf("The value of Y is --> %f \n ",y);    
}

double FindY(double x)
{
    double y;
    y = pow (x,4)-3*pow (x,3)+2*pow (x,2)+1;
    return y;
}

