//Kunal Mukherjee
//Pohlig-Hellman Algorithm
//4/1/2018

#include "stdio.h"
#include "gmp.h"
#include "string.h"
#include "factorize.c"
#include "stdbool.h"
#include "shanks.h"
#include "setjmp.h"

#define TRY do{ jmp_buf ex_buf__; if( !setjmp(ex_buf__) ){
#define CATCH } else {
#define ETRY } }while(0)
#define THROW longjmp(ex_buf__, 1static jmp_buf s_jumpBuffer;


void dlp(mpz_t rop, const mpz_t p,
	 const mpz_t g,
	 const mpz_t h);

void chineeseRemainder(mpz_t rop, const mpz_t * y,
		       const struct factors * f,
		       const mpz_t pri,
		       int len);

void multiInverse(mpz_t rop, const mpz_t a,
		  const mpz_t b);		       



void dlp(mpz_t rop, const mpz_t p,
	               const mpz_t g,
	               const mpz_t x)
{
  //show what p,g,x
  gmp_printf("Dlp was called with these values:\nThe p : %Zd g : %Zd x : %Zd \n\n", p , g , x);
  //set up factor array
  struct factors *f = malloc(sizeof(struct factors));
  bool shank = false;
  
  mpz_t n, nTemp ,h, gTemp, expTemp, hTemp, qTemp , eTemp, one, result;

  mpz_init(n);
  mpz_init(nTemp);
  mpz_init(h);
  mpz_init(gTemp);
  mpz_init(expTemp);
  mpz_init(hTemp);
  mpz_init(qTemp);
  mpz_init(eTemp);
  mpz_init(one);
  mpz_init(result);
  
  mpz_set_str(one,"1",10);
  mpz_sub(n,p,one);
  mpz_set(nTemp,n);

  mpz_t * gi;
  mpz_t * hi;
  mpz_t * yi;
 
  
  factor(nTemp,f);
 
  gi = malloc(sizeof(mpz_t)*f->nfactors);
  hi = malloc(sizeof(mpz_t)*f->nfactors);
  yi = malloc(sizeof(mpz_t)*f->nfactors);
  

  for (int i = 0; i < f->nfactors; i++)
    {     
      mpz_init(gi[i]);
      mpz_init(hi[i]);
      mpz_init(yi[i]);      
    }  
    

  for (int i = 0; i < f->nfactors; i++)
    {
      mpz_set_ui(eTemp, f->e[i]);
      mpz_powm(qTemp,f->p[i], eTemp, p);
      mpz_cdiv_q(expTemp,n,qTemp);    
      mpz_powm(gTemp,g,expTemp,p);
      mpz_powm(hTemp,x,expTemp,p);
      //create the gi and hi
      mpz_set(gi[i],gTemp);    
      mpz_set(hi[i],hTemp);           
    }  

  for (int i = 0; i < f->nfactors; i++)
    {
      mpz_set_ui(eTemp, f->e[i]);
      mpz_powm(qTemp,f->p[i], eTemp, p);
      mpz_set_ui(eTemp, f->e[i]);
      //call Shank and make the yi
      TRY
	{
	  shanks(yi[i],p,gi[i],hi[i],qTemp);
	  //gmp_printf("p = %Zd e = %Zd g = %Zd h = %Zd y = %Zd\n", f->p[i],
	  //	 eTemp, gi[i], hi[i], yi[i]);
	  shank = true;
	}
      CATCH
	{
	  printf("Shanks have FAILED.\n");
	}
       ETRY;
    }
 
    //printf("\n");   

  if(shank)
    {
      //use CRT to compute the x
      chineeseRemainder(rop, yi, f, p, f->nfactors);
    }
  else
    {
      mpz_set_str(rop,"00000000",10);
    }

    //check the x and see it it makes sense
    mpz_powm(result,g,rop,p);
    if(mpz_cmp(result,x) == 0)
       printf("\n\nThe result was checked.\n");

    gmp_printf("%Zd^%Zd (mod %Zd) = %Zd\n",g,rop,p,x);

    mpz_clear(n);
    mpz_clear(nTemp);
    mpz_clear(h);
    mpz_clear(gTemp);
    mpz_clear(expTemp);
    mpz_clear(hTemp);
    mpz_clear(qTemp);
    mpz_clear(eTemp);
    mpz_clear(one);
  
}

void chineeseRemainder(mpz_t rop, const mpz_t * y,
		       const struct factors * f,
		       const mpz_t pri,
		       int len)
{
  mpz_t p , prod, sum, pTemp, ropMul, tempMul1, tempMul2, ropTemp, qTemp, eTemp;
  int i;  
  
  mpz_init(p); 
  mpz_init(prod);
  mpz_init(sum);
  mpz_init(pTemp);
  mpz_init(ropMul);
  mpz_init(tempMul1);
  mpz_init(tempMul2);
  mpz_init(ropTemp);
  mpz_init(qTemp);
  mpz_init(eTemp);  
  
  mpz_set_str(prod, "1", 10);
  mpz_set_str(sum, "0", 10);

  for (i = 0; i < len; i++)
    {         
      mpz_set_ui(eTemp, ((struct factors *)f)->e[i]);        
      mpz_powm(qTemp,((struct factors *)f)->p[i], eTemp, pri);      
      mpz_mul(prod,prod,qTemp);      
    }
  

  for (i = 0; i < len; i++)
    {
      mpz_set_ui(eTemp, ((struct factors *)f)->e[i]);
      mpz_powm(qTemp,((struct factors *)f)->p[i], eTemp, pri);      
      mpz_cdiv_q(p, prod, qTemp);      
      multiInverse(ropMul,p, qTemp);      
      mpz_mul(tempMul1,y[i],ropMul);
      mpz_mul(tempMul2,tempMul1,p);
      mpz_add(sum,sum,tempMul2);      
    }

  
  mpz_mod(rop,sum,prod);  

  mpz_clear(p); 
  mpz_clear(prod);
  mpz_clear(sum);
  mpz_clear(pTemp);
  mpz_clear(ropMul);
  mpz_clear(tempMul1);
  mpz_clear(tempMul2);
  mpz_clear(ropTemp);
  mpz_clear(qTemp);
  mpz_clear(eTemp);
  
  
}

void multiInverse(mpz_t rop, const mpz_t aIn,
		  const mpz_t bIn)
{
  //gmp_printf("a = %Zd, b = %Zd\n", aIn,bIn);
  
  bool found = false;
  mpz_t a, b,b0, t, q, x0, x1, zero, one, mulTemp;

  mpz_init(a);
  mpz_init(b);
  mpz_init(b0);
  mpz_init(t);
  mpz_init(q);
  mpz_init(x0);
  mpz_init(x1);
  mpz_init(zero);
  mpz_init(one);
  mpz_init(mulTemp);

  mpz_set(a,aIn);
  mpz_set(b,bIn);
  
  mpz_set(b0,b);
  mpz_set_str(x0, "0", 10);
  mpz_set_str(x1, "1", 10);
  
  mpz_set_str(zero, "0", 10);
  mpz_set_str(one, "1", 10);

  if (mpz_cmp(b,one) == 0)
    {
      mpz_set(rop,one);
      found = true;      
    }
  
  if (!found)
    {

      while(mpz_cmp(a,one) > 0)
	{
	  mpz_fdiv_q(q,a,b);
	  mpz_set(t,b);
	  mpz_mod(b,a,b);
	  mpz_set(a,t);
	  mpz_set(t,x0);

	  mpz_mul(mulTemp,q,x0);
	  mpz_sub(x0,x1,mulTemp);
	  mpz_set(x1,t);
	}

      if (mpz_cmp(x1,zero) < 0)
	{
	  mpz_add(x1,x1,b0);
	}      
     
      mpz_set(rop,x1);      
      
    }

  mpz_clear(a);
  mpz_clear(b);
  mpz_clear(b0);
  mpz_clear(t);
  mpz_clear(q);
  mpz_clear(x0);
  mpz_clear(x1);
  mpz_clear(zero);
  mpz_clear(one);
  mpz_clear(mulTemp);
  

}
