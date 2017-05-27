
//Kunal Mukherjee
//Assignment 4 Series for Trig Function
//10th Sept, 2016
//CS 210 -Dr. Blandford

#include<stdio.h>
#include<math.h>
#include<stdlib.h>

#define pi 3.141592653589793

instruction();

double x_input();
double accuracy_input();

double factorial(double x);

print_sin(double x, double y);
print_cos(double x, double y);


 int main()
{
    double x, x_rad;
    double actual_sin, actual_cos; 
    double accuracy, new_accuracy;
    double calculated_sin, calculated_cos;
    int n;

    instruction();

    x = x_input();
    x_rad = x * (pi / 180);

    actual_sin = sin (x_rad);
    actual_cos = cos (x_rad);

    accuracy = accuracy_input(); 

    if (accuracy == 0 && x == 0)
    {
        printf("\nThe program has been instructed to terminate.\n\n\n");
        return 0;
    }

    if (accuracy < pow(10, -9))
    {
        new_accuracy = pow(10, -9);
        accuracy = new_accuracy;
        printf("\nThe Programmar has set the accuracy valuye to 10^(-9) as the input accuracy value was less that 10^ (-9).\n");
    }   
    
    calculated_sin = 0;
    for (n = 0; accuracy <= fabs(actual_sin - calculated_sin); n++)
    {    
        double nth_frac;
        double frac;
        nth_frac = (2 * n) + 1;
        frac = factorial(nth_frac);

        if (n % 2 == 0)
        {
            calculated_sin = calculated_sin + ((pow(x_rad, nth_frac))/frac);
            
        }
        else
        {
            calculated_sin = calculated_sin - ((pow(x_rad, nth_frac))/frac);
            
        }            
    }   
   print_sin(x, calculated_sin);

   calculated_cos = 0;
    for (n = 0; accuracy <= fabs(actual_cos - calculated_cos); n++)
    {    
        double nth_frac;
        double frac;
        nth_frac = (2 * n);
        frac = factorial(nth_frac);

        if (n % 2 == 0)
        {
            calculated_cos = calculated_cos + ((pow(x_rad, nth_frac))/frac);
            
        }
        else
        {
            calculated_cos = calculated_cos - ((pow(x_rad, nth_frac))/frac);
            
        }            
    }   
   print_cos(x, calculated_cos);

    while (accuracy != 0 && x != 0)
    {

    x = x_input();
    x_rad = x * (pi / 180);

    actual_sin = sin (x_rad);
    actual_cos = cos (x_rad);

    accuracy = accuracy_input(); 

    if (accuracy == 0 && x == 0)
    {
        printf("\nThe program has been instructed to terminate.\n\n\n");
        return 0;
    }

    if (accuracy < pow(10, -9))
    {
        new_accuracy = pow(10, -9);
        accuracy = new_accuracy;
        printf("\nThe Programmar has set the accuracy valuye to 10^(-9) as the input accuracy value was less that 10^ (-9).\n");
    }   
    
    calculated_sin = 0;
    for (n = 0; accuracy <= fabs(actual_sin - calculated_sin); n++)
    {    
        double nth_frac;
        double frac;
        nth_frac = (2 * n) + 1;
        frac = factorial(nth_frac);

        if (n % 2 == 0)
        {
            calculated_sin = calculated_sin + ((pow(x_rad, nth_frac))/frac);
            
        }
        else
        {
            calculated_sin = calculated_sin - ((pow(x_rad, nth_frac))/frac);
            
        }            
    }   
   print_sin(x, calculated_sin);

   calculated_cos = 0;
    for (n = 0; accuracy <= fabs(actual_cos - calculated_cos); n++)
    {    
        double nth_frac;
        double frac;
        nth_frac = (2 * n);
        frac = factorial(nth_frac);

        if (n % 2 == 0)
        {
            calculated_cos = calculated_cos + ((pow(x_rad, nth_frac))/frac);
            
        }
        else
        {
            calculated_cos = calculated_cos - ((pow(x_rad, nth_frac))/frac);
            
        }            
    }   
   print_cos(x, calculated_cos);
}
}


instruction()
{
    printf("\nThis program will prompt you to enter value of x in degree and a accuracy number. \nThe program will return the value of sin x and cos x to the required accuracy.\n");
}

double x_input()
{
    double x;
    printf("\nEnter the value of x in degrees-->\t");
    scanf_s("%lf", &x);
    return x;
}

double accuracy_input()
{
    double accuracy;
    printf("\nEnter the accuracy number-->\t");
    scanf_s("%lf", &accuracy);
    return accuracy;
}

double factorial(double frac)
{
    
    double i = 1, f = 1;          
  while ( i <= frac )
      {
      f = f * i;      
      i++;
      }
      return f;
}

print_sin(double x, double y)
{
     printf("\nSin of %f is %.10f\n", x, y);
}

print_cos(double x, double y)
{
    printf("Cos of %f is %.10f\n\n\n", x, y);
}


