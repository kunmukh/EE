// File: inserstionsort.h
// Template functions for sorting vectors using Insertion Sort algorithm
// Based on Dale, et al., C++ Plus Data Structures 6/e, Chapter 12

#ifndef INSERSTIONSORT_H_
#define INSERSTIONSORT_H_

#include <vector>     // std::vector<T>
#include <algorithm>  // std::swap

template<class ItemType>
void InsertItem(std::vector<ItemType>& values, int startIndex, int endIndex)
// Post: values[0]..values[endIndex] are now sorted.
{
   bool finished = false;
   int current = endIndex;
   bool moreToSearch = (current != startIndex);

   while (moreToSearch && !finished)
   {
      insertionSortComparisons++;
      if (values[current] < values[current-1])
      {
	 insertionSortExchanges++;
	 std::swap(values[current], values[current-1]);
	 current--;
	 moreToSearch = (current != startIndex);
      }
      else
	 finished = true;
   }
   if (debug)
      PrintContainer(values);
}

template<class ItemType>
void InsertionSort(std::vector<ItemType>& values)
// Post: The elements in the array values are sorted by key.
{
   int numValues = values.size();
   for (int count = 0; count < numValues; count++)
      InsertItem(values, 0, count);
}

#endif
