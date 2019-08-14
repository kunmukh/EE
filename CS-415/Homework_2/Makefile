driver: driver.o
	clang -Wall -lgmpxx -lgmp driver.o -o driver

driver.o: driver.c shank.h
	clang -Wall -c driver.c
