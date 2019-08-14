//Kunal Mukherjee
//CS 210
//Recursive Function
//22nd Nov, 2016

#include<stdio.h>
#include<math.h>

int SumOfDigits(int s);
int PowerOfTwo(int n);

int main ()
{
    int a;
    printf("Enter the integer:");    
    scanf_s("%d", &a);    
    int sum;
    sum = SumOfDigits(a); 
    printf("%d\n", sum);

    int power;
    printf("Enter the power of two:\t");
    scanf_s("%d", &power);
    int power_ans;
    power_ans= PowerOfTwo(power);
    printf("%d\n", power_ans);

}

int SumOfDigits(int s)
{
    if (s == 0)
    return 0;
    
    else 
    return (s % 10) + (SumOfDigits(s/10));
}

int PowerOfTwo(int n)
{
    if (n == 0)
        return 1;

    else 
    return 2 * PowerOfTwo(n-1);
     
}