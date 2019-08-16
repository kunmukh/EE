#include "stdio.h"
#include "gmp.h"

int main (int argc, char **argv)
{
  mpz_t a ,b;
  mpz_init(a);
  mpz_init(b);

  mpz_set_str(a, "2348732794923794988329849872398749832749873298479832798", 10);
  mpz_set_str(b, "10101010101010101010", 2);
  
  gmp_printf("This us a : %Zd\n", a);
  gmp_printf("This us a : %Zi\n", b, 2);

  return 0;

  
}
