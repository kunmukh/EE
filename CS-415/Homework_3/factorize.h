
struct factors
{ 
  mpz_t         *p;
  unsigned long *e;
  long nfactors;
};

void factor (mpz_t, struct factors *);

