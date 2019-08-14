/* Kunal Mukherjee*/
// August 30, 2016
//First Program

#include<stdio.h>
#define KMS_PER_MILE 1.609

int main(void)
{
    double miles, kms;
    printf("Enter the distance in miles -->");
    scanf_s("%lf" , &miles);
    kms = KMS_PER_MILE * miles;
    printf("The required kilometers is %f. \n", kms);
    return(0);
    

}