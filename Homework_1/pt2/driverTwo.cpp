//Kunal Mukherjee

#include <iostream>
#include <fstream>
#include "extendedNum.h"
#include <gmp.h>
#include <string>


using namespace std;

int main (int argc, char *argv[])
{
  using namespace std; 
  
  if (argc != 2) 
    {
      cout << "Usage:" << argv[0] << "inputfile" << endl;
      return -1;
    }
  
  ifstream inFile (argv[1]);

  if (!inFile) 
    {
      cout << "Error opening input file: " << argv[1] << endl;
      return -1;
    }  

  mpz_t p,rop,f_temp;
  mpz_t * factors;
  int n = 0;
  
    
  string p_temp, rop_temp, fact_temp; 
  
  inFile >> p_temp;
  inFile >> n;      

  factors = new mpz_t [n];
  mpz_init_set_str(p, p_temp.c_str() , 10);  

  for(int i = 0; i < n ; i++)
    {
      inFile >> fact_temp;      
      mpz_init_set_str(f_temp, fact_temp.c_str() , 10);      
      mpz_set(factors[i],f_temp); 
    }

  inFile >> rop_temp;
  mpz_init_set_str(rop, rop_temp.c_str() , 10);

  primitive_root(rop,p,n,factors);

  inFile.close();
  delete [] factors;
  
  mpz_clear(p);
  mpz_clear(rop);
  mpz_clear(f_temp);
   
  return 0;
}
