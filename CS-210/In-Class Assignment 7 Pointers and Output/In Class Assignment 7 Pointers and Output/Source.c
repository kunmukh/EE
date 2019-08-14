//Kunal Mukherjee
//CS 210
//27th Sept, 2016
// Pointer and output parameters

#include<stdio.h>
#include<math.h>
#define pi 3.141592653589793

 ConvertToXY(double r, double theta, double *x,double *y);

int main()
{
    double r, theta, x, y;
    printf("Enter a number for r-->  \t");
    scanf_s("%lf", &r);
    printf("Enter a number for theta--> \t");
    scanf_s("%lf", &theta);
    theta = theta * (pi/180);
    ConvertToXY(r, theta, &x, &y);       
    printf(" %f, %f \n", x, y);
        }

    ConvertToXY(double r, double theta,double  *x,double *y)
    {
     
        *x = r * cos(theta);
        *y = r * sin(theta);
    }

