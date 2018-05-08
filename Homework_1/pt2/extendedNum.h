//Kunal Mukherjee
//cs415
//2/4/18

#include <gmp.h>
#include <cmath>
using namespace std;
#include <iostream>

void eea(mpz_t x, 
		      mpz_t y,
		      mpz_t d,
		      mpz_t a,
		      mpz_t b)
{
  mpz_t u,v,q,r,m,n,temp1,temp2,zero,temp_a,temp_b,result_1,neg_1;
   
 
  gmp_printf("The a: %Zd and b: %Zd\n", a , b);  
  
  mpz_init_set_str(u, "1", 10);
  mpz_init_set_str(v, "0", 10);
  mpz_init_set_str(neg_1, "-1", 10);
  mpz_init(q);
  mpz_init(r);
  mpz_init(m);
  mpz_init(n);
  mpz_init(temp_a);
  mpz_init(temp_b);
  mpz_init(result_1);
  
  mpz_init(temp1);
  mpz_init(temp2);   
  mpz_init_set_str(zero, "0", 10);

  mpz_set(temp_a,a);
  mpz_set(temp_b,b);

 
  while (mpz_cmp(a,zero) != 0){
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

  //ax + by = result
  mpz_mul(temp1,temp_a,x);
  mpz_mul(temp2,temp_b,y);
  //cout << "kunal" << endl;
  mpz_add(result_1,temp1,temp2);
  //mpz_mul(result_1,result_1,neg_1);

  //mpz_set(result,result_1);

  

  gmp_printf("The u: %Zd and v: %Zd and GCD is %Zd\n", x , y, result_1);
  gmp_printf("GCD checker used from gmp gave the result %Zd\n", d);

  if(mpz_cmp(result_1,d) == 0)
    {
      gmp_printf("The gmp_gcd: %Zd and algorithm: %Zd gave the correct answer.\n", d, result_1);
    }
  
  mpz_clear(u);
  mpz_clear(v);
  mpz_clear(q);
  mpz_clear(r);
  mpz_clear(m);
  mpz_clear(n);  
  mpz_clear(temp1);
  mpz_clear(temp2);
  mpz_clear(zero);  
  
}

void primitive_root( mpz_t rop,
				  const mpz_t p,
				  const int n,
				  const  mpz_t* factors)
{
  mpz_t answer, temp_sub, temp_div,temp_rop,one, temp_subp,r;
  bool flag = false;
  mpz_init(answer);
  mpz_init(temp_sub);
  mpz_init(temp_div);
  mpz_init(temp_rop);
  mpz_init(temp_subp);
  mpz_init_set_str(r, "2", 10);
  mpz_init_set_str(one, "1", 10);
  mpz_sub(temp_subp,p,one);
  //cout << "kunal" << endl;

  while (mpz_cmp(r,temp_subp) < 0)
    { 
      flag = false;
      
      for (int i = 1; i < n ; i++)
	{	  
	  mpz_cdiv_q(temp_div,temp_subp,factors[i]);
	  mpz_powm(temp_rop, r, temp_div,p);

	  if (mpz_cmp(temp_rop,one) == 0)
	    {
	      flag = true;
	      break;
	    }	  
	}

      if(!flag)
	{
	  mpz_set(answer,r);
	  break;
	}
    
      mpz_add(r,r,one);
    }

  //cout << "kunal" << endl;
  gmp_printf("The primiitve root that my algorithm found  %Zd\n", answer);
  
   if(mpz_cmp(rop,answer) == 0)
    {
      gmp_printf("The primitve root: %Zd and algorithm: %Zd gave the correct answer.\n", rop,answer);
    }
  
  mpz_clear(r);
  mpz_clear(answer);
  mpz_clear(temp_sub);
  mpz_clear(temp_div);
  mpz_clear(temp_rop);
  mpz_clear(one);
  mpz_clear(temp_subp);
 
}

  #include <gmp.h>

#ifndef EXTENDEDNUM
#define EXTENDEDNUM

class ExtendedNum
{
 public:
  ExtendedNum();
  
  void eea(mpz_t x, 
	   mpz_t y,
	   mpz_t d,
	   mpz_t a,
	   mpz_t b);

  void primitive_root( mpz_t rop,
		       const mpz_t p,
		       const int n,
		       const  mpz_t* factors);

 private:
  mpz_t result;  
};

#endif
