//Kunal Mukherjee
//CS 210
//29TH sEPT, 2016

#include<stdio.h>
#include<stdlib.h>
#pragma warning(disable:4996)

int WriteNumbers();
double AverageNumbers();


int main()
{
    fseek(stdin, 0, SEEK_END);
    double avg; 

    if(WriteNumbers() == 0)
        printf("File written successfully. \n");

        avg = AverageNumbers();
        printf("File written successfully. \n");
        printf("%f\n", avg);   
}

int WriteNumbers()
{
    //Writes 1000 random ints between 0 and 100
    //  to "Numbers.txt"
	int err, i, r;
    FILE *outp; //  and out files
	err = fopen_s(&outp, "Numbers.txt", "w");
    if(err != 0)
        return 1;
    srand(23);
    for(i=0;i<1000;i++)
	{
        r = rand();
        r = r % 101;  //0 to 100
        fprintf(outp, "%d\n", r);
    }
    fclose(outp);
    return 0;

}

double AverageNumbers()
{
double sum, open, avg;
int dataIn, status;
sum = 0;
avg = 0;

FILE *inp;
 
inp = fopen("Numbers.txt", "r");
status = fscanf_s(inp, "%d", &dataIn);

    while (status == 1)
    {
        sum += dataIn;
        status = fscanf_s(inp, "%d", &dataIn);
    }

    avg = sum / 1000;
    return avg;
 }

 
