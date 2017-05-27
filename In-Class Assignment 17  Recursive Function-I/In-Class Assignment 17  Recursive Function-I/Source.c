#include<stdio.h>
#include<string.h>

void PrintRow(int n, char c);
void PrintTriangle(int n, char c);
void PrintTriangle2(int n_tri2, char c);
void PrintTree(int n_tri3, char c, int lines);
void ReverseString(char c);


int main()
{
    int n;
    int n_tri;
    int n_tri2;
    int n_tri3;
    int lines;
    char c;
    char c1;
    printf("Enter a number for number of stars printed-->\t");
    scanf_s("%d", &n);
    printf("Enter a number for number of star triangles printed-->\t");
    scanf_s("%d", &n_tri);
    printf("Enter a number for number of triangles printed-->\t");
    scanf_s("%d", &n_tri2);
    printf("Enter a number for number of tree printed-->\t");
    scanf_s("%d", &n_tri3);
    printf("Enter a number for number lines-->\t");
    scanf_s("%d", &lines);
    c = '*';

    PrintRow(n, c);
    printf("\n\n");
    PrintTriangle(n_tri,c);
    printf("\n");
    PrintTriangle2(n_tri2,c);
    printf("\n");
    PrintTree(n_tri3,c,lines);
    printf("\n");

    printf("Enter a string..\n");
    scanf_s("%c",&c1);
    ReverseString(c1);
    printf("\n");

}      


void PrintRow(int n, char c)
{
    if (n > 0)
    {
        printf("%c",c);
        PrintRow(n-1, c);
    }    
}

void PrintTriangle(int n_tri, char c)
{
    if (n_tri == 1)
    { 
        printf("%c\n",c);
        return 0;
    }
        PrintRow(n_tri, c);
        printf("\n");
        PrintTriangle(n_tri-1,c);
}  

void PrintTriangle2(int n_tri2, char c)
{
    if (n_tri2 == 1)
    { 
        printf("%c\n",c);
        return;
    }
        PrintTriangle2(n_tri2-1,c);
        PrintRow(n_tri2, c);
        printf("\n");        
        
}

void PrintTree(int n_tri3, char c, int lines)
{
    if (n_tri3> 0)
    {
       PrintRow(n_tri3,0);      
       PrintRow((lines - n_tri3)*2 + 1 , c);  
       printf("\n");
       PrintTree(n_tri3-1,c,lines); 
       }       
}

void ReverseString(char c1)
{
    scanf_s("%c", &c1);
    if(c1 != '\n')
        ReverseString(c1);
    printf("%c", c1);
    return;
}