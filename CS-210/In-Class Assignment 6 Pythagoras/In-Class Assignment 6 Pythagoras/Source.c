//Kunal Mukherjee
//Assignment Pythagoras
//cs 210
//15th Sept, 2016

#include<stdio.h>
#include<math.h>
 

 int main ()
 {
     
     int a, b, c;
    
 for (a = 1; a < 999; a++)
 
 for (b = 1; b < 999; b++)
 
 for (c = 1 ; c < 999; c++)
 {
 if (a + b + c == 1000 && pow(a,2) + pow(b,2) == pow(c,2))
 printf("a = %d;\n b = %d;\n c = %d\n", a,b,c);
 }
 return 0; 
 }
