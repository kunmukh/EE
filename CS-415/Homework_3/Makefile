driver: driver.o
	clang -Wall -lgmpxx -lgmp driver.o -o driver

driver.o: driver.c pohlig-hellman.h shanks.h
	clang -Wall -c driver.c

clean:
	rm -f *.o *~ driver
