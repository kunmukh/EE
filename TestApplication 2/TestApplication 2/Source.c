#include<stdio.h>
#include<stdlib.h>
#pragma warning(disable:4996)

void Birthday(int x);

int main ()
{
    int n;   
    printf("Enter your birthdate to be written on a file-->");
    scanf("%d",&n);    
    FILE *TextOutput;           
    fopen_s(&TextOutput, "Birthday.txt", "w");
    fprintf(TextOutput, "%d", n);       
    fclose(TextOutput);  

    int date;
    fopen_s(&TextOutput, "Birthday.txt", "r");
    fscanf_s(TextOutput, "%d", &date);        
    FILE *ReadBday;   
    fopen_s(&ReadBday, "Format_Bday.txt", "w");
    fprintf(ReadBday, "%d <--This is your Birthday", &date);
    fclose(TextOutput, ReadBday);

    printf("The program sucessfully ran!!!\n");

    return 0;
}