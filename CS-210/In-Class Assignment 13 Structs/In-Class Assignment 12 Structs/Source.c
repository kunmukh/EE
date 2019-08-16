//Kunal Mukherjee
//CS 210
//Dr.Blandford
//1st Nov, 2016

#include<stdio.h>
#include<math.h>

typedef struct
{
 double real;
 double imag;
} complex_t;
//Put your function prototype here.

complex_t MultiplyComplex(complex_t c1, complex_t c2);

int main()

{
 complex_t c1, c2, c3;
 c1.real = 4; c1.imag = 3;
 c2.real = 1; c2.imag = 2;
 c3 = MultiplyComplex(c1, c2);
 printf("(%4.1f + i%4.1f) * (%4.1f + i %4.1f) = (%4.1f + i%4.1f)\n", c1.real, c1.imag, c2.real, c2.imag, c3.real, c3.imag);
 return 0;
}

complex_t MultiplyComplex(complex_t c1, complex_t c2)
{
    complex_t x  ;
    x.real = (c1.real*c2.real - c1.imag*c2.imag);
    x.imag = (c1.real*c2.imag + c2.real*c1.imag );
    return x;
}