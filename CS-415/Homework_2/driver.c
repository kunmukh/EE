//Kunal Mukherjee
//Shanks Algorithm
//3/1/2018

#include "stdio.h"
#include "gmp.h"
#include "string.h"
#include "stdlib.h"
#include "shank.h"

int main (int argc, char **argv){  
  
  FILE *file;
  file = fopen(argv[1],"r"); 
  
  
  //Initialize values
  mpz_t p, g, h, rop, numTimes;
  mpz_init(p);
  mpz_init(g);
  mpz_init(h);
  mpz_init(rop);
  mpz_init(numTimes);
  
  mpz_inp_str(numTimes, file, 10);

  double numT =  mpz_get_d(numTimes);
  
  
  for (double i = 0; i < numT; i++)
    {     
      
      //Assign Values
      mpz_inp_str(p, file, 10);
      mpz_inp_str(g, file, 10);
      mpz_inp_str(h, file, 10);

      shanks(rop, p, g, h);
      gmp_printf("The is rop : %Zd\n\n\n", rop);
    }

  mpz_clear(p);
  mpz_clear(g);
  mpz_clear(h);
  mpz_clear(rop);
  mpz_clear(numTimes);
  fclose(file);
  return 0;
}
