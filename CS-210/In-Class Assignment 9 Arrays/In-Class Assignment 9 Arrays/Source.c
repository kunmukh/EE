//Arrays
// Kunal Mukherjee
//CS210
//Assignment : Arrays
// The program  shifts all the values in an array argument to the left 1 place with the value at index 0 becoming the last value in the array. 

#include<stdio.h>

void Rotate(int d[], int n);
// This function takes in the array d and a argument of n

int main()
{
 int d[] = {1, 2, 3, 4, 5, 6, 7, 8, 9};
 int i;
 for(i=0;i<8;i++)
 printf("%d ", d[i]);
 printf("\n");

 Rotate(d, 8);

 for(i=0;i<8;i++)
 printf("%d ", d[i]);
 }

void Rotate(int d[], int n)
{
  int x , i ;   
   x = d[0]; 

  for (i = 0 ; i < n ;i++)  {
  d[i]= d[i+1];  
  }

  d[7]= x;

}