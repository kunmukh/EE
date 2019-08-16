// File: sorting2.cpp
// Various sorting algorithms for vectors.
// Assumes that type T has defined operator<

#include <cstdlib>   // exit
#include <fstream>
#include <iostream>
#include <iomanip>
#include <vector>

// Global counters
int selectionSortComparisons = 0,
   selectionSortExchanges = 0;
int insertionSortComparisons = 0,
   insertionSortExchanges = 0;
int bubbleSortComparisons = 0,
   bubbleSortExchanges = 0;
int heapSortComparisons = 0,
   heapSortExchanges = 0;

// Debugging flags
bool debug = true;

template <class T>
void PrintContainer (const T & container) {
   using namespace std;
   for (auto e: container)
      cout << " " << e;
   cout << endl;
}  // end PrintContainer

// Sorting functions - includes put here so global vars and PrintContainer
// are visible to sorting functions.
#include "selectionsort.h"
#include "insertionsort.h"
#include "bubblesort.h"
#include "heapsort.h"

template <class T>
void ReadVectorFromFile (std::vector<T> & v, std::istream &in)
{
   T value;

   // make vector empty
   v.clear();

   // read in values
   while (in >> value)
      v.push_back(value);
}  // end ReadVectorFromFile


int main (int argc, char *argv[])
{
   using namespace std;

   if (argc != 2)
   {
      cout << "Usage: " << argv[0] << " input_file" << endl;
      exit(1);
   }

   ifstream in (argv[1]);
   if (!in)
   {
      cout << "Unable to open input file: " << argv[1] << endl;
      exit(1);
   }

   vector<int> v;
   ReadVectorFromFile(v, in);
   vector<int> copyToSort (v);

/*
   cout << "Selection Sort" << endl;
   SelectionSort(copyToSort);

   copyToSort = v;
   cout << "Bubble Sort" << endl;
   BubbleSort(copyToSort);

   copyToSort = v;
   cout << "Insertion Sort" << endl;
   InsertionSort(copyToSort);

   copyToSort = v;
*/
   cout << "Heap Sort" << endl;
   HeapSort(copyToSort);

/*   
   cout << "\n\nSorting vector of " << v.size() << " elements" << endl << endl;

   cout << "Sorting algorithm       Comparisons        Exchanges/Copies"
	<< endl;
   cout << "-----------------       -----------        ----------------"
	<< endl;
   cout << left << setw(20) << "Selection sort" 
	<< right << setw(15) << selectionSortComparisons 
	<< setw(17) << selectionSortExchanges << endl;
   cout << left << setw(20) << "Bubble sort" 
	<< right << setw(15) << bubbleSortComparisons 
	<< setw(17) << bubbleSortExchanges << endl;
   cout << left << setw(20) << "Insertion sort" 
	<< right << setw(15) << insertionSortComparisons 
	<< setw(17) << insertionSortExchanges << endl;
   cout << left << setw(20) << "Heap sort" 
	<< right << setw(15) << heapSortComparisons 
	<< setw(17) << heapSortExchanges << endl;
*/
   return 0;
}
