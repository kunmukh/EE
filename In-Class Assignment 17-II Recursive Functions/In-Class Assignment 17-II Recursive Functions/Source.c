//Kunal Mukherjee
//CS210
//Recursive Function

#include<stdio.h>
void WriteVertical(int n);

int main (void)
{
    int n;
    int sum;
    printf("Enter an integer-->\t");
    scanf_s("%d", &n);
    WriteVertical(n);    
}

void WriteVertical(int n)
{
    if (n < 10)
         {
            printf("%d\n", n);
         }         
    else if (n > 10)
    {        
        WriteVertical(n/10);
        printf("%d\n", n % 10);
    }    
}