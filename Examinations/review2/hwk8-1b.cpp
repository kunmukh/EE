// Homework 8
// Chapter 5 Exercise 1b
#include <iostream>
#include <stack>
using namespace std;

int main()
{
   stack<int> stk;
   int item1 = 4,
       item3 = 0,
       item2 = item1+1;       // item2 = 5
   stk.push(item2);           // 5
   stk.push(item2+1);         // 5 6
   stk.push(item1);           // 5 6 4
   item2 = stk.top();         // item2 = 4
   stk.pop();                 // 5 6
   item1 = item2+1;           // item1 = 5
   stk.push(item1);           // 5 6 5
   stk.push(item3);           // 5 6 5 0
   while (!stk.empty())
   {
      item3 = stk.top();
      cout << item3 << endl;  // last time, item3 = 5
      stk.pop();
   }
   cout << item1 << endl << item2 << endl << item3 << endl;

   return 0;
}
