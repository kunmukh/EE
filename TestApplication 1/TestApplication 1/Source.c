#include<stdio.h>
int main()
{
int m = 25;
int *mPointer;
mPointer = &m;
printf("m = %d\n", m);
printf("&m = %d\n", &m);
printf("mPointer = %d\n", mPointer);
}