// Kunal Mukhw=erjee
// CS 210
// August 28, 2016
// Dr.Blandford
// Assignment 1- Triangles

#include <stdio.h>
#include <stdlib.h>
#include <math.h> /* Include Math library*/
#define pi 3.141592653589793 /* Define pi*/

int main(void)
{

    double x1, x2, x3, y1, y2, y3;
    double A, B, C;
    double rA, rB, rC;
    double a ,b, c;
    double P;
    double s;
    double K;
    double R;
    double r;
    printf("KUNAL MUKHERJEE'S Assignment-- 1 Triangles\n");
    printf("\nPLEASE ENTER the following VERTICES so that:\nSIDE LENGTH,PERIMETER and SEMI-PERIMETER of TRIANGLE;\nRADIUS of CIRCUMSCRIBED CIRCLE, RADIUS of INSCRIBED CIRCLE;\nANGLES of the TRIANGLE CORRESPONDING to the LENGTH OF TRIANGLE can be *CALCULATED*");
    printf("\n\nEnter the first x1, or first x vertex--> ");
    scanf_s("%lf", &x1);
    printf("Enter the first y1, or first y vertex--> ");
    scanf_s("%lf", &y1);
    printf("Enter the second x2, or second x vertex--> ");
    scanf_s("%lf", &x2);
    printf("Enter the second y2, or second y vertex-->");
    scanf_s("%lf", &y2);
    printf("Enter the third x3, or third x vertex--> ");
    scanf_s("%lf", &x3);
    printf("Enter the third y3, or third y vertex--> ");
    scanf_s("%lf", &y3);
    printf("\nThe vertices you have entered are (%0.3f,%0.3f),(%0.3f,%0.3f),(%0.3f,%0.3f) \n", x1, y1, x2, y2, x3, y3);
    printf("\n*****************THE CALCULATED RESULTS******************\n");
    a = (sqrt((pow ((x2-x3),2))+(pow((y2-y3),2))));
    printf("\nDimension of side 'a'= %0.3f \n", a);
    b = (sqrt((pow ((x1-x3),2))+(pow((y1-y3),2))));
    printf("Dimension of side 'b'= %0.3f \n", b);
    c = (sqrt((pow ((x2-x1),2))+(pow((y2-y1),2))));
    printf("Dimension of side 'c'= %0.3f \n" , c);
    P = a+b+c;
    printf("\nDimension of Perimeter = %0.3f \n" , P);
    s = P/2;
    printf("\nDimension of Semi-perimeter = %0.3f\n", s);
    K = sqrt(s*(s-a)*(s-b)*(s-c));
    printf("\nArea = %0.3f \n" , K);
    R = a*b*c/(4*K);
    printf("\nThe measure of the circumscribed circle of radius R= %.3f\n" , R);
    r = sqrt(((s-a)*(s-b)*(s-c))/s);
    printf("\nThe measure of the inscribed circle of radius r= %.3f\n" , r);
    A = acos(((pow (b,2))+(pow (c,2))-(pow (a,2)))/(2*b*c));
    rA = A * (180/pi);
    printf("\nThe measure of angle A in degrees = %.3f \n", rA);
    B = acos(((pow (c,2))+(pow (a,2))-(pow (b,2)))/(2*c*a));
    rB = B * (180/pi);
    printf("The measure of angle B in degrees = %.3f \n", rB);
    C = acos(((pow (a,2)) +(pow (b,2)) - (pow (c,2)))/(2*a*b));
    rC = C * (180/pi);
    printf("The measure of angle C in degrees = %.3f \n", rC);
    printf("\n*****************THE REQUIRED RESULTS ARE DISPLAYED ABOVE*****************\n");

   return (0);
}