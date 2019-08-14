# File: Makefile
# Makefile for sorting test program
# Class: CS 215

sorting: sorting.o
	clang++ -g -Wall -o sorting sorting.o

sorting.o: sorting.cpp selectionsort.h insertionsort.h bubblesort.h heapsort.h 
	clang++ -g -Wall -std=c++11 -c sorting.cpp

clean:
	rm -f *.o sorting *~
