// Homework 8
// Chapter 5 Exercise 1a
#include <iostream>
#include <stack>
using namespace std;

int main()
{
   stack<int> stk;
   int item1 = 1,
       item2 = 0,
       item3 = 4;             // stk contents
   stk.push(item2);           // 0
   stk.push(item1);           // 0 1
   stk.push(item1+item3);     // 0 1 5
   item2 = stk.top();         // item2 = 5
   stk.pop();                 // 0 1
   stk.push(item3*item3);     // 0 1 16
   stk.push(item2);           // 0 1 16 5
   stk.push(3);               // 0 1 16 5 3
   item1 = stk.top();         // item1 = 3
   stk.pop();                 // 0 1 16 5
   cout << item1 << endl << item2 << endl << item3 << endl;
   while (!stk.empty())
   {
      item1 = stk.top();
      stk.pop();
      cout << item1 << endl;
   }
   return 0;
}


