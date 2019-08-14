dummy: libcs220.a example

libcs220.a: libcs220.a(cs220lib.o) libcs220.a(main.o)

libcs220.a(main.o): main.cpp
	$(CXX) -g -c -m32 main.cpp
	$(AR) cr libcs220.a main.o
	rm -f main.o

libcs220.a(cs220lib.o): cs220lib.S
	$(CXX) -g -m32 -c -o cs220lib.o cs220lib.S
	$(AR) cr libcs220.a cs220lib.o
	rm -f cs220lib.o

example: example.s libcs220.a
	$(CXX) -g -m32 -o example example.s libcs220.a

clean:
	rm -f example example.exe

realclean: clean
	rm -f libcs220.a
