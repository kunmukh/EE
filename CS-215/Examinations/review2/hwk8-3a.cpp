// Homework 8
// Chapter 5 Exercise 3b
#include <iostream>
#include <queue>
using namespace std;

int main()
{
   queue<int> que;
   int item1 = 1,
       item2 = 0,
       item3 = 4;             // que contents
   que.push(item2);           // 0
   que.push(item1);           // 0 1
   que.push(item1+item3);     // 0 1 5
   item2 = que.front();       // item2 = 0
   que.pop();                 // 1 5
   que.push(item3*item3);     // 1 5 16
   que.push(item2);           // 1 5 16 0
   que.push(3);               // 1 5 16 0 3
   item1 = que.front();       // item1 = 1
   que.pop();                 // 5 16 0 3
   cout << item1 << endl << item2 << endl << item3 << endl;
   while (!que.empty())
   {
      item1 = que.front();
      que.pop();
      cout << item1 << endl;
   }
   return 0;
}


