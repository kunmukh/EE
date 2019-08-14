//Kunal Mukherjee
//cs 210
//Lopp equation evalution
//8th Septmember, 2016

#include<stdio.h>
#include<math.h>

double FindY(double x);

int main (void)
{
    double x;
    double y;
    double xIncr;

    printf("\nEnter the x-value--> \t");
    scanf_s("%lf", &xIncr);

    
    for (x = 0; x < 10; x = x + xIncr)
    {
        
        y = FindY(x);
        printf("\nThe required values %lf \n", y);
    }
}

double FindY(double x)
{
    double y;
    y = pow (x,4) + 3 * pow(x,3) + 2 * pow(x,2) + 1;
    return y;

}