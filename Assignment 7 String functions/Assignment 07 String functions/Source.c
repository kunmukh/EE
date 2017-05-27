//File: Asn07KXM
//Kunal Mukherjee
//cs 210
//Assignment: Assignment 07 String Functions
//For this assignment you will write three "bullet-proof" string function: Str2Double,Int2Bin, and Bin2Int. These functions will notify the user after parsecing the string if it had accurately convert a string to double or an Integer to Binary or a Binary number to an integer

//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

#include<stdio.h>
#include<string.h>
#include<conio.h>
#pragma warning(disable:4996)


double Str2Double(char s[], double *d); //this function accepts a string less than 80 characters long, parses the string, and converts the result to a double which is returned in an output parameter. 
int Bin2Int(char bin[]); // this function takes in a string of binary and returns the appropiate integer
int power(int c, int d); //this function is used to calculate power of 2, while converting binary to integer
void Int2Bin(char s_bin[], int n); //thsi functions takes a interger , n, as a argument and converts it into binary , which is placed into the srting s_bin

int main()
{
    char s[80]; // The string to contain digit, number or decimal
    char bin[16];     // the string that contains the binary that will be  converted to integer
    char s_bin[17];  // the string that contains the integer that will be  converted to binary. An extra bit space is left to symbolize if the integer is negative or positive, if it is negative a '0' is being placed infront of the binary , if negative a '1' is placed infront of the binary

    double d;
    double status;  
    int b; // b is the integer converted from binary
    int n; // n is the integer that is going to be converted to binary

    {
            strncpy(s, "0", 80);
            status = Str2Double(s, &d);
            printf("The status of 0 is :%f\n\n", status); 

            strncpy(s, "123", 80);
            status = Str2Double(s, &d);
            printf("The status of 123 is :%f\n\n", status);

            strncpy(s, "1.2345", 80);
            status = Str2Double(s, &d);
            printf("The status of 1.2345 is :%f\n\n", status);

            strncpy(s, "ABC", 80);
            status = Str2Double(s, &d);
            printf("The status of ABC is :%f\n\n", status);

            strncpy(s, "A1.B23", 80);
            status = Str2Double(s, &d);
            printf("The status of A1.B23 is :%f\n\n", status); 
    }       
    
    {
            strncpy(bin, "00101", 16);
            b = Bin2Int(bin);       
            printf("\nThe Integer value is :%d\n\n", b);

            strncpy(bin, "11011", 16);
            b = Bin2Int(bin);       
            printf("\nThe Integer value is :%d\n\n", b);

            strncpy(bin, "00000000000000011", 16);
            b = Bin2Int(bin);       
            printf("\nThe Integer value is :%d\n\n", b);
    }
    
    {
                n = 5;
                Int2Bin(s_bin, n);
                printf("\nThe binary number is (The first digit is 0 for negative integer and 1 for positive integer):");
               for (int i = 0 ; i <= 16; i++)
                {
                    printf("%c", s_bin[i]);
                }    
                printf("\n");

                n = (-5);
                Int2Bin(s_bin, n);
                printf("\nThe binary number is (The first digit is 0 for negative integer and 1 for positive integer):");
               for (int i = 0 ; i <= 16; i++)
                {
                    printf("%c", s_bin[i]);
                }    
                printf("\n");

                n = 32768;
                Int2Bin(s_bin, n);
                printf("\nThe binary number is (The first digit is 0 for negative integer and 1 for positive integer):");
               for (int i = 0 ; i <= 16; i++)
                {
                    printf("%c", s_bin[i]);
                }    
                printf("\n");
    }
         
}

double Str2Double(char s[], double *d)
{
    int cnt_decimal = 0; // counter for decimal number
    int cnt_digit = 0; // counter for digits
    int cnt_non_digit = 0; // counter for non-digits

    for (int i = 0; i < 80; i++) // this loop is used to classify the elemsnts in the string, into decimals, diigts and non-digits
    {
        if (s[i] == 46)
        {
            cnt_decimal++;
        }

        else  if (s[i] >= 48 && s[i] <= 57)
        {
            cnt_digit++;
        }
        
        else if ((s[i] > 0 && s[i] < 48)||(s[i] > 57 && s[i] <= 255))
        {
            cnt_non_digit++;
        }
    }

    if ((cnt_decimal > 1) || ((cnt_digit == 0) && (cnt_non_digit > 1))) // String has more than one decimal point or there are no digits in the string.
    {
        *d = 0;
        return 0;
    }

    else if (cnt_decimal == 1 || cnt_decimal == 0 )
    {
         if (cnt_digit > 0 && cnt_non_digit == 0) //String has 0 or 1 decimal point and all other characters are digits
        {
             *d = 1;
             return 1;
        }

        else if (cnt_digit > 0 && cnt_non_digit > 0) //String has 0 or 1 decimal points, at least one digit, and there are additional non-digit characters which are ignored.
        {
             *d = -1;
             return -1;
        }
    }
 }

int Bin2Int(char bin[])
{      
   int cnt_bin = 0 ;
   
    for (int i = 0; i < 16; i++) // this loop is to count how many 1 and 0 are on the string
    {
        if (bin[i] == 48 || bin[i] == 49)
        {
            cnt_bin++;
        }
        
    }

    if (cnt_bin == 0) // if there are string is empty, it should return 0
    {
        return 0;
    }

    int dec=0; 
    int j=0,f;    
     
    
    for (int i = 0; i < cnt_bin ; i++)  // this loop is used to print the bianry
    {
        printf("%c",(char)bin[i]);
    }   
    
    for(int i= cnt_bin - 1; i>=0 ; i--)  //this loop is used to calculate the decimal number 
    {
        dec=((bin[i]-48)*power(2,j))+dec;
        j++;
    }

    return dec;

}

int power(int c, int d)
{
    int pow=1;
    int i=1;
    while(i<=d) 
    {
        pow=pow*c;
        i++;
    }
    return pow;
}

void Int2Bin(char s_bin[] , int n)
{
   int cnt_bin = 0;
   int n_init = n;  
   n = abs(n);

   for (int i = 0 ; i <= 16; i++) // if there are more than 16 binary digits it should return all 0.
   {
          if (cnt_bin > 16)
       {
           for (int k = 0; k < 16; k++)
           {
               s_bin[k] = '0';
           }
          return; 
       }
        
       else if (cnt_bin <= 16)  // this loop is used to calculate binary number
        {
            if ((n != 0 && n % 2 == 0) && (n != 1  && n % 2 == 0 ))
            { 
                s_bin[16-i]= '0';
                n = n/2 ; 
                cnt_bin++;
              }
         
            else if ((n != 0 && n % 2 == 1) && (n != 1  && n % 2 == 1 ))
            {
                 s_bin[16-i]= '1';
                 n = n/2; 
                 cnt_bin++; 
             }    
             
             else if (cnt_bin < 16 && n == 1)
            {
                s_bin[16-i]= '1';
                                
                n_init;
                if (n_init <= 0) // this loop decides wheater the integer was a negative or positive and it adds zero or one accordingly
                {
                    s_bin[(16-i)-1] = '0';
                }
                else 
                {
                    s_bin[(16-i)-1] = '1';
                }

                for (int j = (16-i)- 2 ; j >= 0; j--)
                {
                    s_bin[j] = 0; // the rest of the emptly spaces in the string should be filled with space.
                }
                return;
            }
            
             else if (cnt_bin < 1 && n == 0)
            {
                s_bin[16-i]= '0'; 

                 n_init;
                 if (n_init <= 0) // this loop decides wheater the integer was a negative or positive and it adds zero or one accordingly
                {
                    s_bin[(16-i)-1] = '0';
                }
                else
                {
                    s_bin[(16-i)-1] = '1';
                } 

                for (int j = (16 - i) - 2; j >= 0; j--)
                {
                    s_bin[j] = 0;
                }                             
                return;                
         }         
 }
 }
 }

 


