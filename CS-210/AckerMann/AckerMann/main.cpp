#include <fstream>
#include <cstdlib>

using namespace std;

int Ackermann(int m, int n);

#include <iostream>

int  main ()
{
  cout << "Enter the m value" << endl;
  cout << "Enter the n value" << endl;
   int m;
   int n;
   cin >> m >> n ;
   int ackermann;

ackermann = Ackermann(m,n);

cout << "Ackermann(" << m << "," << n <<") is " << ackermann << endl;

   return 0;
}

int Ackermann(int m, int n)
{
  if (m == 0)
    {
      return n + 1;
    }
  if ( m > 0 && n == 0)
    {
      return Ackermann(m-1, 1);
    }
  if ( m > 0 && n > 0)
    {
      return Ackermann(m-1, Ackermann(m, n-1));
    }
  else
    return 0;
}
