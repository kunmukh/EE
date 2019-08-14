//Kunal Mukherjee
//IF/ELSE
//6th sept, 2016

#include<stdio.h>
#include<math.h>

int main()
{
    double x;
    double y;

    printf("Enter the x value -->");
    scanf_s("%lf", &x);
       
    if (x < 0)
    {
        y = abs(x);
        printf("%f \n", y);
    }

   else if (x >= 0 && x < 12)
    {
        y = pow(x,2);
        printf("%f \n", y);
    }

    else if (x >= 12 && x <= 50)
    {
        y = pow( x - 12, 2);
        printf("%f \n", y);
    }

    else if (x > 50)
    {
        y = sqrt(x);
        printf("%f \n", y);
    }
    

    return 0;

    
}