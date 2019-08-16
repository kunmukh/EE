//Kunal Mukherjee
//cs 210
//Multidimensional Array
//27th Oct, 2016

#include<stdio.h>
 

int main()
{
    int d3[][3][5] = {{{1, -2, 3, -4, 5}, {6, -7, 8, 9, 0}}, 
                      {{10, -9, 8, 7, 6}, {5, 4, -3, 2, 1}}, 
                      {{6, -9, 3, -2, 6}, {1, 8, -6, 1, 5}} 
                     };
                     int r, c, p;
 
     for (p = 0; p < 2; p++)
     {
         for(r=0;r<3;r++)
         {
             for(c=0;c<5;c++)
             {
                 if (d3[p][r][c] < 0)
                 
                     d3[p][r][c] = 0; 
                     printf("%d, ",d3[p][r][c]);
                                 
             }   
             printf("\n");
         }
      }
       
 }       
             
    


