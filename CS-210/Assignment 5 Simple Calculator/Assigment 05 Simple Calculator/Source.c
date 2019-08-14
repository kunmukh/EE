// File: Asn05KXM
//Kunal Mukherjee
//CS 210
//27th Sept , 2016
//Assignment 5 - Simple Calculator
//The program will perform the operation indicated, show the result, and continue until the you enter(q) to exit. The initial result is 0.

#include<stdio.h>
#include<math.h>
#include<stdlib.h>

//void Instructions();
// This function has no parameters and returns a void but prints instructions. 
//This function has no inputs.

//void GetInput(double *number, char *operator_c);
//This function returns a void and has two output parameters for the number and operator. This function is used to recceive the operator and the number used for the computaion.
//(double *number, char *operator_c) The two input/ output parameter are pointers. They are pointers because I need the function two return two values simulatneously. 

//void DoOperation(double number, char operator_c, double *accumulator);
//This function has two input parameters,to receieve the number and the operation, and one input/output parameter, the accumulated result. It returns a void
//(double number, char operator_c, double *accumulator) The two input parameter is cumber and character_c, which will bring operator and number from the main program for computation. One input/output parameter the accumulator is a pointer which stores the result.

void Instructions();
void GetInput(double *number, char *operator_c);
void DoOperation(double number, char operator_c, double *accumulator);


int main()
{
    double number, accumulator;   //creating two variable to hold the number entered by the user and the accumulator to hold the result
    char operator_c;              // Char operator_c is defined to help store the operator entered by the user 

    Instructions();               // Calling Instructions , will print the instructions of the program
     
    accumulator = 0;             // Initializing the accumulator to zero.   

    printf("\nThe intial result is --> %f\n", accumulator);  // To let the user know that the value of the accumulator has been set to zero by default.

   GetInput( &operator_c, &number);    // Calling the GetInput function with two arguments which are pointers. Pointers are used to get two different answers simultaneously 

     while (operator_c != 'q') // A interrupt has been places , so that the DoOperation function is only ran when the operator is not 'q'
   {
        DoOperation(number, operator_c, &accumulator); // If the user entered a symbol and a number, then the main function calls DoOperatation with three arguments. Two of the arguments are input arguments but the accumualtor is a input/output arguemnt. 
        fseek(stdin,0,SEEK_END);    //This instructions clears the buffer before the input from the user is asked again
        GetInput( &operator_c, &number); //alling the GetInput function with two arguments which are pointers. Pointers are used to get two different answers simultaneously 
   }

   printf("\nThe Final calculated result is -->%f\n\n", accumulator);  //Once the user inputs 'q' and the main program falls out of the loop and prints the value of the accumulator

  return 0;  // Return zero to end the main program
}

void Instructions()    //Function that prints instruction
{
     printf("Enter a operator along with a number on a single line.\nThe program will perform the operation indicated, show the result,\n and continue until the you enter(q) to exit. The initial result is 0\n"); 
}

void GetInput(char *operator_c, double *number) // This function returns a void and has two output parameters for the number and operator. This function is used to recceive the operator and the number used for the computaion.
{ 
    printf("\n\n\n Enter a Arithmetic Operator and Number--> \t");  //Prints to ask the user to enter a arithmetic operator and a number
    scanf_s("%c ", &*operator_c); //Scans for the operator and places inside the pointer *operator_c, which changes the operator_C value in the main program also.
    scanf_s("%lf", &*number); //Scans for the number and places inside the pointer *number, which changes the number value in the main program also.   
    return 0;  //Return zero to end the function
}

void DoOperation(double number, char operator_c, double *accumulator)  //This function has two input parameters,to receieve the number and the operation, and one input/output parameter, the accumulated result. It returns a void.
{
       if (operator_c == '+')  // if the operator_c is a'+' then it enters this loop
    {
        *accumulator += number;  // adds the number to the accumualtor
        printf("The result is --> %f \n", *accumulator);// prints the accumualtor as result
    }

        else if (operator_c == '-')  // if the operator_c is a'-' then it enters this loop
    {
        *accumulator -= number; // adds the number from the accumualtor
        printf("The result is --> %f \n", *accumulator);// prints the accumualtor as result
    }

        else if (operator_c == '*')  // if the operator_c is a'*' then it enters this loop
    {
        *accumulator *= number;  // multiplies the number with the accumualtor
        printf("The result is --> %f \n", *accumulator); // prints the accumualtor as result
    }

        else if (operator_c == '/')  // if the operator_c is a'/' then it enters this loop
    {
        *accumulator /= number;  // divides the number with the accumualtor
        printf("The result is --> %f \n", *accumulator); // prints the accumualtor as result
    }   
        
        else if (operator_c == '^')  // if the operator_c is a'^' then it enters this loop
    {
        *accumulator = pow(*accumulator,number);    // raises to the accumualtor to the power of the number entered   
        printf("The result is --> %f \n", *accumulator);// prints the accumualtor as result
    }   

         
    }
 
  
  





    

