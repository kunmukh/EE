//Kunal Mukherjee
//CS-415 Cryptography
//1/24/2018
//EEC- Extended Eucledian Algorithm

#include "stdio.h"
#include "gmp.h"

int main (int argc, char **argv){

  //printf("The x: %s and y : %s\n",argv[1], argv[2]);
  
  mpz_t a,b,x,y,u,v,q,r,m,n,temp1,temp2,gcd,result,zero;
  
  mpz_init_set_str(a, argv[1] , 10);
  mpz_init_set_str(b, argv[2], 10);

  gmp_printf("The a: %Zd and b: %Zd\n", a , b);

   
  
  mpz_init_set_str(x, "0", 10);
  mpz_init_set_str(y, "1", 10);
  mpz_init_set_str(u, "1", 10);
  mpz_init_set_str(v, "0", 10);
  mpz_init_set_str(zero, "0", 10);
 
  mpz_init(q);
  mpz_init(r);
  mpz_init(m);
  mpz_init(n);
  mpz_init(gcd);
  mpz_init(temp1);
  mpz_init(temp2);

  mpz_gcd(temp1, a,b);
  gmp_printf("GCD checker used from gmp is %Zd\n", temp1);
 


  while (mpz_cmp(zero,a) != 0){
    //q = b / a;
    mpz_cdiv_q(q,b,a);   
    //r = b % a;
    mpz_cdiv_r(r,b,a);
    //m = x - u * q;
    mpz_mul(temp1,u,q);
    mpz_sub(m,x,temp1);
    //n = y - v * q;
    mpz_mul(temp2,v,q);
    mpz_sub(n,y,temp2);
    
   
    //b = a;
    mpz_set(b,a);
    //a = r;
    mpz_set(a,r);
    //x = u;
    mpz_set(x,u);
    //y = v;
    mpz_set(y,v);
    //u = m;
    mpz_set(u,m);
    //v = n;
    mpz_set(v,n);
  }

  //gcd = b;
  mpz_set(gcd,b); 

  gmp_printf("The x: %Zd and y : %Zd and GCD is %Zd\n", x , y, b);
  

  

  mpz_clear(a);
  mpz_clear(b);
  mpz_clear(x);
  mpz_clear(y);
  mpz_clear(u);
  mpz_clear(v);
  mpz_clear(q);
  mpz_clear(r);
  mpz_clear(m);
  mpz_clear(n);
  mpz_clear(gcd);   


  return 0;
 
}
