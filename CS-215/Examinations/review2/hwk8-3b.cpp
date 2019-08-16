// Homework 8
// Chapter 5 Exercise 3b
#include <iostream>
#include <queue>
using namespace std;

int main()
{
   queue<int> que;
   int item1 = 4,
       item3 = 0,
       item2 = item1+1;       // item2 = 5
   que.push(item2);           // 5
   que.push(item2+1);         // 5 6
   que.push(item1);           // 5 6 4
   item2 = que.front();       // item2 = 5
   que.pop();                 // 6 4
   item1 = item2+1;           // item1 = 6
   que.push(item1);           // 6 4 6
   que.push(item3);           // 6 4 6 0
   while (!que.empty())
   {
      item3 = que.front();
      cout << item3 << endl;  // last time, item3 = 0
      que.pop();
   }
   cout << item1 << endl << item2 << endl << item3 << endl;

   return 0;
}
