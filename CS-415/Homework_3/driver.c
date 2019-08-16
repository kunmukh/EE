//Kunal Mukherjee
//Pohlig-Hellman Algorithm
//4/1/2018

#include "stdio.h"
#include "gmp.h"
#include "string.h"
#include "stdlib.h"
#include "pohlig-hellman.h"
#include "stdbool.h"

int main (int argc, char **argv)
{
  //Initialize values
  mpz_t p, g, x, rop, numTimes;
  mpz_init(p);
  mpz_init(g);
  mpz_init(x);
  mpz_init(rop);
  mpz_init(numTimes);
  
  bool file = false;

  if(argc == 2)
    {
      file = true;
    }
  else
    {
      file = false;
    }

  if(file)
    {  
      FILE *file;
      file = fopen(argv[1],"r");  
      mpz_inp_str(numTimes, file, 10);

      double numT =  mpz_get_d(numTimes);
  
  
      for (double i = 0; i < numT; i++)
	{     
      
	  //Assign Values
	  mpz_inp_str(p, file, 10);
	  mpz_inp_str(g, file, 10);
	  mpz_inp_str(x, file, 10);

	  dlp(rop, p, g, x);
	  gmp_printf("The is rop : %Zd\n\n\n", rop);
	}

       fclose(file);
    }
  else
    {
      //Assign Values
      mpz_set_str(p, argv[1] , 10);
      mpz_set_str(g, argv[2] , 10);
      mpz_set_str(x, argv[3] , 10);

      dlp(rop, p, g, x);
      gmp_printf("The is rop : %Zd\n\n\n", rop);
    }

  mpz_clear(p);
  mpz_clear(g);
  mpz_clear(x);
  mpz_clear(rop);
  mpz_clear(numTimes);
 
  return 0;
}
