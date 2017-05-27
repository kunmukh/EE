The CS220 library is used to teach x86 assembly using the GNU gas assembler.
The routines are fairly simple wrappers around the C stdio I/O routines.
The library allows students to write "interesting" programs (programs
that do simple I/O) very quickly.

There are two problems with trying to use the stdio I/O routines
directly:

1) The students coming into the class are not familiar with the
   stdio I/O routines.  They have taken a course in C++ and learned
   how to do I/O using iostreams.

2) In order to use the stdio routines at the assembly language level,
   students must understand the cdecl calling convention.  The cdecl 
   calling convention is a stack-based convention.  Students must then
   understand how the stack works before they can use the stdio routines. 
   The CS220 library uses a register based calling convention that
   does not require the user to have an understanding of the stack.
   Also, the CS220 routines preserve all registers.  The cdecl calling
   convention allows certain registers (EAX, ECX, EDX) to be used
   as scratch registers by a subprogram.  If they are being used in the
   main program they must be saved before the subprogram call and
   then restored afterward.

The stack and the cdecl calling convention are very important topics
and are covered in detail later in the course.  (A major course
objective is that students have an understanding of what happens at
the assembly language level during a subprogram call.)

Using the CS220 library allows students to write assembly language
programs that do I/O on the first day that we start discussing
assembly instead of much later.

====================================================================
64 Bit Issues

This library uses only 32 bit assembly, when compiling on a
64 bit architecture make sure that the -m32 flag is passed
to the GNU compiler.

====================================================================
Mac Only Notes

The Mac requires that the stack pointer be aligned on a 16 byte
boundary before all calls to OS routines.  The CS220 library routines
make this adjustment, so that the programmer can use the CS220
library routines without having to worry about aligning the stack
pointer.

If you want to call a routine outside the CS220 library, you will need
to align the stack before making an OS call.  Here is the standard
way to do this for a routine that takes a single argument:

	andl	$0xfffffff0, %esp	# align stack on 16 bytes
	subl	$12, %esp
	pushl	argone
	call	routine			# stack still aligned at call

For a two argument routine this becomes

	andl	$0xfffffff0, %esp	# align stack on 16 bytes
	subl	$8, %esp
	pushl	argtwo
	pushl	argone
	call	routine			# stack still aligned at call

and so on.

====================================================================

The library has been built and tested on the following platforms:
Linux, Cygwin, MingW, and DJGPP.  It has been partially tested on
a Mac (Intel).

====================================================================

Tony Richardson
richardson@evansville.edu
