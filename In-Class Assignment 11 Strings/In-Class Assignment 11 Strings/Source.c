//Kunal Mukherjee
//cs 210
//strings
//Oct 25, 2016

#include<stdio.h>
#include<string.h>

int main()
{
    char string1[20];
    int i, length;
    int check = 0;
    
    printf("Enter a string:");
    gets(string1);
        
    length = strlen(string1);
        
     for(i=0;i < 20 ;i++)
     {
             string1[i]= toupper(string1[i]);
    }
    
    for(i=0;i < length ;i++)
    {
        if(string1[i] != string1[length-i-1])
        {
            check = 1;            
	   }     
    }
    
    if (check)
    {
        printf("%s is not a palindrome\n", string1);
    }  
      
    else 
    {
        printf("%s is a palindrome\n", string1);
    }

    return 0;
}