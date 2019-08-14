//Kunal Mukherjee
//Shanks Algorithm
//3/1/2018

#include "stdio.h"
#include "gmp.h"
#include "string.h"

void shanks(mpz_t rop, const mpz_t p,
	               const mpz_t g,
	               const mpz_t h);

int comparator(const void *p, const void *q);


struct list {
  mpz_t value;
  int index;
};

void shanks(mpz_t rop, const mpz_t p,
	               const mpz_t g,
	               const mpz_t h)
{ 
  gmp_printf("Shank was called with these values:\nThe p : %Zd g : %Zd h : %Zd \n", p , g , h);
  mpz_t n, N, one;
  
  //initialize
  mpz_init(n);
  mpz_init(N);
  mpz_init(one);
  
   
  //set up N
  mpz_set_str(one , "1", 10);
  mpz_sub(N,p,one);
  mpz_sqrt(n,N);
  mpz_add(n,n,one);

  
  struct list * list1;  

  //create list 1 and indexList
  list1 = malloc ((mpz_get_ui (n)+1) * sizeof (struct list));
  
  
  mpz_init_set_ui (list1[0].value, 1);
  list1[0].index = 0;
  

  for (unsigned long int i = 1; i <= mpz_get_ui (n); i++)
    {
      list1[i].index = i;      
      mpz_init (list1[i].value);
      mpz_powm_ui(list1[i].value,g,i,p);          
    }

  //printf("Power of g is found\n");
  
  //quicksort
  qsort(list1, mpz_get_ui (n)+1, sizeof (struct list), comparator);

  //printf("Qsort done\n");
 
  //Giant step
  // h* g^(-n*q) ==> h*u^n
  mpz_t hGqn; 
  mpz_t u;
  mpz_t u_contain;  
  mpz_init_set (u, g);
  //u = g^n
  mpz_powm (u, u, n, p);
  // u = g^-n
  mpz_invert (u, u, p);
  mpz_init_set (u_contain,u);
  //hGqn = h
  mpz_init_set (hGqn, h); 
  
  int j = 0;
  struct list * jTemp = 0;
  struct list temp;
  mpz_init(temp.value);
  mpz_t final;
  mpz_init(final); 
  
  
  //searching the element
  for (int i = 0; i <= mpz_get_ui (n); i++)
    {          
      
      mpz_set(temp.value,hGqn);     
      //find h*u^n in list1 by doing binary search
      jTemp = (struct list*) bsearch (&temp, list1, mpz_get_ui (n)+1,
				      sizeof (struct list),  comparator);   
      
      
      if (jTemp > 0)
	{
	  j = jTemp->index;
	  printf("An element of list1 matched h*u.\n");	 
	  //rop = i + j*n
	  mpz_mul_ui (n,n,i); //i*n
	  mpz_add_ui (n,n,j); //j + i*n
	  //set rop
	  mpz_set (rop, n);

	  mpz_powm(final,g,rop,p);
	  if(mpz_cmp(final,h) == 0)
	    printf("The g^rop = h. The answer was checked.\n");

	  free(list1);
	  mpz_clear(n);
	  mpz_clear(N);
	  mpz_clear(one);
	  mpz_clear(hGqn);
	  mpz_clear(final);
	  mpz_clear(u);
	  break;
	}
      
      //h = h*u^i
      mpz_mul (hGqn, hGqn, u); 
      mpz_mod(hGqn, hGqn, p);
      
    }
 
  
}

int comparator(const void *p, const void *q)
{ 
  mpz_t l,r;
  mpz_init_set(l,((struct list *)p)->value);
  mpz_init_set(r,((struct list *)q)->value);
  int result = mpz_cmp(l,r);
  mpz_clear(l);
  mpz_clear(r);
 
 
  return result;
  
}

