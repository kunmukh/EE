//File:Asn08KXM
//Kunal Mukherjee
//CS 210
//Assignment: Fourier series expansion
// This program that will generate a file to plot the Fourier expansion of either a square wave or a sawtooth wave using either a fixed number of terms or a prescribed accuracy.
//---------------------------------------------------------------------------------------------------------------------------

#include<stdio.h>
#include<stdlib.h>
#include<math.h>
#include<string.h>
#define PI 3.141592653589793
#pragma warning(disable:4996)

void Square(double terms, double points, double c[][2] );  //This function produces the points for a square graph. It accepts the terms and points and the two dimensional points array which is hold the t valule and the f(t) value 
void Sawtooth(double terms, double points, double c[][2]);  //This function produces the points for a sawtooth graph. It accepts the terms and points and the two dimensional points array which is hold the t valule and the f(t) value 


int main()
{
    
    double terms, points;    
    int choice;
    char file_name[100];

    
    printf("Enter Choose The following wave form: \n1> Square\n2> Sawtooth\n");
    scanf_s("%d", &choice);
    printf("Enter the number of terms( 1 to 100 ): ");
    scanf_s("%lf", &terms);
    printf("Enter the number of point you want to be plotted ( 100 to 1000 ): ");
    scanf_s("%lf", &points);              

   if (terms < 1 || terms > 100)  // if the term is not in the range of 1 to 100, I set it to 5
    {
        terms = 5;
   }

   if (points < 100 || points > 1000)   //// if the term is not in the range of 100 to 1000, I set it to 200
    {
        points = 200;
   }

   double **array;
    array = calloc(points, sizeof (double));  //using the number of points entered by the user, to produce a 1D array of that number
    for (int i = 0; i < points; i++)
    {
         array[i] = calloc(points, sizeof (double));  //using calloc that 1D array is chaged to 2D so that the coordinate poinst cna be stored
    } 

   if (choice == 1 || choice == 2)  // The choice number decides which loop to trigger
   {
       if (choice == 1)
       {
            Square(terms,points, &array[0][0]);  //The terms number entered by the user and the points is plasses along with the first element of the array as a way to pass the whole array
       }

       else if (choice == 2)
       {
            Sawtooth(terms,points, &array[0][0]);
       }
   }         
}

void Square(double terms, double points, double array[][2])
{
   double t, tincr;
   double f_t;
   int k;
   tincr = (8*PI)/points;  //the x range is from -4pi to +4pi. so the t-increment is 8pi/points
   t = (-4) * PI;  // the initaial t is -4pi

    for (int i = 0; i < points; i++) // this f_t is calculated for all the points
    {
        f_t = 0;
        for (k = 1; k <= 2*terms; k += 2)
        {
           double new_f_t = (4/(k*PI))* sin(k*t);  // a new f_t is calculated for each k and added to the f_t as a way to figure out the sum of fuction for the value of K
           f_t = f_t + new_f_t;
        }              
        array[i][0] = t;  //the i-th row is filled with the t value and f_t value
        array[i][1] = f_t;        
        t += tincr;  // t in incremented
    }    
   
    fseek(stdin,0,SEEK_END);
    char file_name[100] = {"test.txt"};  //the computer asks for the file name that it will use to save in the computer
    printf("Enter the file name that with store the points(place .txt after the name): \t ");
    gets(file_name);
    
    FILE *name; //a file is opened
    name = fopen (file_name, "w");  //initialized it for writing

   for (int i = 0; i < points; i++)  //the points in the array is printed into the text file . The points are separated by a comma and a new coordinate is separated bya new line
    {
        fprintf(name,"%lf,%lf\n", array[i][0], array[i][1] );
    }     
   fclose(name);

   for (int i = 0; i < points; i++)  // the array is printed in the terminal for the user to see
    {
        printf("%f ,%f\n", array[i][0], array[i][1]);
    }
    
}

void Sawtooth(double terms, double points, double array[][2])
{
   double t, tincr;
   double f_t;
   int k;
   tincr = (8*PI)/points;
   t = (-4) * PI;

    for (int i = 0; i < points; i++)
    {
        f_t = 0;
        for (k = 1; k <= 2*terms; k++)
        {
           double new_f_t = ((pow(-1, k+1))/k)* sin(k*t);
           f_t = f_t + new_f_t;
        }
        array[i][0] = t; 
        array[i][1] = 2 * f_t;
        t += tincr;
    }

    fseek(stdin,0,SEEK_END);
    char file_name[100] = {"test.txt"};
    printf("Enter the file name that with store the points: ");
    gets(file_name);
    
    FILE *name;
    name = fopen (file_name, "w");

   for (int i = 0; i < points; i++)
    {
        fprintf(name,"%lf,%lf\n", array[i][0], array[i][1] );
    }     
   fclose(name);

   for (int i = 0; i < points; i++)
    {
        printf("%f ,%f\n", array[i][0], array[i][1]);
    }
}