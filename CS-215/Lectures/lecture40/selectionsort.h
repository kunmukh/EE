// File: selectionsort.h
// Template functions for sorting vectors using Selection Sort algorithm
// Based on Dale, et al., C++ Plus Data Structures 6/e, Chapter 12

#ifndef SELECTIONSORT_H_
#define SELECTIONSORT_H_

#include <vector>     // std::vector<T>
#include <algorithm>  // std::swap

template<class ItemType>
int MinIndex(const std::vector<ItemType>& values, int startIndex, int endIndex)
// Post: Returns the index of the smallest value in
//       values[startIndex]..values[endIndex].
{
   int indexOfMin = startIndex;
   for (int index = startIndex + 1; index <= endIndex; index++)
   {
      selectionSortComparisons++;
      if (values[index] < values[indexOfMin])
	 indexOfMin = index;
   }
   return indexOfMin;
}


template<class ItemType>
void SelectionSort(std::vector<ItemType>& values)
// Post: The elements in the array values are sorted by key.
{
   int endIndex = values.size()-1;
   for (int current = 0; current < endIndex; current++)
   {
      std::swap(values[current], 
		values[MinIndex(values, current, endIndex)]);
      selectionSortExchanges++;
      if (debug)
	 PrintContainer(values);
   }
} 

#endif
