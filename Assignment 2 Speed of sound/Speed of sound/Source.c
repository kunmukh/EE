//Kunal Mukherjee
// September 9, 2016
// Assignment 2- Speed of sound
// CS 210- Dr. D. Blandford

#include<stdio.h>
#include<math.h>
#define Conversion_Factor 0.681818 

Help();
GetTemperature();
double PrintFeetPerSecond(double a);
double PrintMilesPerHour(double a);

void main()
{
    printf("\nKUNAL MUKHERJEE'S ASSIGNMENT 2 -- SPEED OF SOUND IN AIR AT SEA LEVEL\n\n");
    double b;
    double c;
    Help();
    b = GetTemperature();
    c = PrintFeetPerSecond(b);
    PrintMilesPerHour(c);
    return 0;

}

Help()
{
    printf("This program computes and display the speed of sound in air at sea level using a user input value for temperature in Farenheit.\n ");
}

GetTemperature()
{
    double a;
    printf("\nEnter the temprature in Farenhite-->\t");
    scanf_s("%lf", &a);
    printf("\nThe temperature you entered--> %f F\n",a);
    return a;
}

 double PrintFeetPerSecond(double a)
 {
    a = 21.92 * ( sqrt ( 5.0 * a + 2297.9));
    printf("\nThe Speed of Sound in feet per second is--> %0.3f ft/sec.\n", a);
    return a;
}

 double PrintMilesPerHour(double a)
 {
     double MilesPerHour;
     MilesPerHour = Conversion_Factor * a;
     printf("\nThe Speed of Sound in miles per hour is--> %0.3f miles/hr.\n\n\n", MilesPerHour);
     return MilesPerHour;  
 }

