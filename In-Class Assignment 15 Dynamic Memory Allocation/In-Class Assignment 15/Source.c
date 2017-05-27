//Kunal Mukherjee
//CS 210
//Dr.Blandford
// 8th Nov, 2016

#include<stdio.h>
#include<stdlib.h>

int main()
{
        int status, dataIn;
        
        FILE *inp;      
        fopen_s(&inp, "MyData.txt", "r");
        status = fscanf_s(inp, "%d", &dataIn);

        int *a;
        a = (int *)calloc(dataIn, sizeof(int));         
        
        int i = 0;
        while(status == 1)
            {            
                status = fscanf_s(inp, "%d", &a[i]);
                i++;
            }

            for(i=0;i<dataIn;i++)
                printf("%d, %d\n", a[i], a[dataIn-i-1]);  

        
 fclose(inp);
}