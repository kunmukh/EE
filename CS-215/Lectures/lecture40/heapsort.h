// File: heapsort.h
// Template functions for sorting vectors using Heap Sort algorithm
// Based on Dale, et al., C++ Plus Data Structures 6/e, Chapter 9

#ifndef HEAPSORT_H_
#define HEAPSORT_H_

#include <vector>     // std::vector<T>
#include <algorithm>  // std::swap

template<class ItemType>
void ReheapDown(std::vector<ItemType>& elements, int root, int bottom)
// Post: Heap property is restored.
{
   int maxChild;
   int rightChild;
   int leftChild;

   leftChild = root*2+1;
   rightChild = root*2+2;
   if (leftChild <= bottom)
   {
      if (leftChild == bottom)
	 maxChild = leftChild;
      else
      {
	 if (elements[leftChild] <= elements[rightChild])
	    maxChild = rightChild;
	 else
	    maxChild = leftChild;
      }
      heapSortComparisons++;
      if (elements[root] < elements[maxChild])
      {
	 heapSortExchanges++;
	 std::swap(elements[root], elements[maxChild]);
	 ReheapDown(elements, maxChild, bottom);
      }
   }
}

template<class ItemType>
void HeapSort(std::vector<ItemType>& values)
// Post: The elements in the array values are sorted by key.
{
   int index;
   int numValues = values.size();

   // Convert the array of values into a heap.
   for (index = numValues/2 - 1; index >= 0; index--)
   {
      ReheapDown(values, index, numValues-1);
      if (debug)
	 PrintContainer(values);
   }
   if (debug)
      std::cout << std::endl;
   // Sort the array.
   for (index = numValues-1; index >=1; index--)
   {
      heapSortExchanges++;
      std::swap(values[0], values[index]);
      ReheapDown(values, 0, index-1);
      if (debug)
	 PrintContainer(values);
   }
}

#endif
