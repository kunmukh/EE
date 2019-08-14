// File: bubblesort.h
// Template functions for sorting vectors using Short Bubble Sort algorithm
// Based on Dale, et al., C++ Plus Data Structures 6/e, Chapter 12

#ifndef BUBBLESORT_H_
#define BUBBLESORT_H_

#include <vector>     // std::vector<T>
#include <algorithm>  // std::swap

template<class ItemType>
void BubbleUp2(std::vector<ItemType>& values, int startIndex, int endIndex,
	       bool& sorted)
// Post: Adjacent pairs that are out of order have been 
//       switched between values[startIndex]..values[endIndex]
//       beginning at values[endIndex].
//       sorted is false if a swap was made; otherwise true.
{
   sorted = true;
   for (int index = endIndex; index > startIndex; index--)
   {
      bubbleSortComparisons++;
      if (values[index] < values[index-1])
      {
	 bubbleSortExchanges++;
	 std::swap(values[index], values[index-1]);
	 sorted = false;
      }
   }
}

template<class ItemType>
void BubbleSort(std::vector<ItemType>& values)
// Post: The elements in the array values are sorted by key.
//       The process stops as soon as values is sorted
{
   int numValues = values.size();
   int current = 0;
   bool sorted = false;

   while (current < numValues - 1 && !sorted)
   {
      BubbleUp2(values, current, numValues-1, sorted);
      current++;
      if (debug)
	 PrintContainer(values);
   }
}

#endif
