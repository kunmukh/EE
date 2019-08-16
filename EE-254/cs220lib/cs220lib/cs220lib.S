# A collection of routines for simplified I/O from assembly.
# These routines preserve ALL registers

# On some architectures (notably Windows) gcc/g++ prepends an
# underscore to function names, on others (notably Linux) nothing
# is prepended.  The following macros try to sort this out in
# a portable way.

# The Mac requires that the stack be aligned on a 16 byte boundary
# before a function call.  You will see several "andl $0xfffffff0, %esp"
# instructions used in the code below to ensure that the stack is
# aligned on a 16 byte boundary.  Windows and Linux have no such
# requirement, but it doesn't hurt to do the alignment on those
# machines either (well, it does waste a few bytes of stack space).

#define ADDPREF(x,y)  XADDPREF(x,y)
#define XADDPREF(x,y)  x ## y

#define GETCHAR   ADDPREF(__USER_LABEL_PREFIX__,getchar)
#define PUTCHAR   ADDPREF(__USER_LABEL_PREFIX__,putchar)
#define PRINTF    ADDPREF(__USER_LABEL_PREFIX__,printf)
#define SCANF     ADDPREF(__USER_LABEL_PREFIX__,scanf)
#define FGETS     ADDPREF(__USER_LABEL_PREFIX__,fgets)
#define FDOPEN    ADDPREF(__USER_LABEL_PREFIX__,fdopen)
#define FFLUSH    ADDPREF(__USER_LABEL_PREFIX__,fflush)
#define STRLEN    ADDPREF(__USER_LABEL_PREFIX__,strlen)
#define ASM_STDIN ADDPREF(__USER_LABEL_PREFIX__,asm_stdin)

.section .data, ""
# The leading space in the format specifiers causes any white space to
# be automatically skipped when using scanf (see the scanf man page).
# We will not want to include this leading space when using printf.

f_str:	.asciz " %s"
f_int:	.asciz " %d"
f_uint:	.asciz " %u"
f_sing:	.asciz " %g"
f_doub:	.asciz " %le"

.globl	cslib_version
cslib_version:
	.asciz "CS220LIB V01.23"

.section .bss, ""
FPUSAVE: .fill	108, 1, 0

.section .text, ""
.globl	print_str, print_int, print_uint, print_nl
.globl	read_str,  read_int,  read_uint,  read_line
.globl	print_char, read_char
.globl	flush, read_nl, str_length
.globl  print_single, print_double, read_single, read_double
.globl  print_singleptr, print_doubleptr, read_singleptr, read_doubleptr

# str_length finds the length of a string.
# The length does not include the null-terminator byte
# pre:  address of string in eax
# post:	the string length is stored in eax
str_length:
	pushl	%ecx
	pushl	%edx

	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl    $12, %esp		# Mac - Keep stack aligned at call
	pushl	%eax
	call	STRLEN

	leave
	popl	%edx
	popl	%ecx
	ret

# read_char reads a single character from standard input
# pre:	none
# post:	char is stored in %eax
read_char:
	pushl	%ecx		# These three may be destroyed by getchar,
	pushl	%edx		# so save them here for later restoration
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

	call	GETCHAR

	leave
	popl	%edx
	popl	%ecx
	ret

# print_char displays a character to standard output
# pre:	char is in %eax
# post:	none
print_char:
	pushl	%ecx		# These three may be destroyed by putchar,
	pushl	%edx		# so save them here for later restoration
	pushl	%eax
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

	subl	$12, %esp	# added to keep SP 16 byte aligned
	pushl	%eax
	call	PUTCHAR
	call	flush

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_str displays a null-terminated string to standard output
# pre:  address of string in eax
# post: none
print_str:
	pushl	%ecx		# These three may be destroyed by printf,
	pushl	%edx		# so save them here for later restoration
	pushl	%eax

	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp	# added to keep SP 16 byte aligned

	pushl	%eax		# Push address of string
	movl	$f_str, %eax 	# Load address of format (%s)
	inc	%eax		# Skip space in format string
	pushl	%eax		# Push address of format string
	call	PRINTF
	call	flush

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_nl displays a newline to standard output
# pre:	 none
# post:	 none
print_nl:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$12, %esp		# added to keep SP 16 byte aligned
	pushl	$10
	call	PUTCHAR
	call	flush
	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_int displays an integer to standard output
# pre:	int in eax
# post:	none
print_int:
	pushl	%ecx
	pushl	%edx
	pushl	%eax

	pushl   %ebp
	movl    %esp, %ebp
	andl    $0xfffffff0, %esp	# Mac - Align stack

	subl	$8, %esp		# added to keep SP 16 byte aligned
	pushl	%eax			# Push int value
	movl	$f_int, %eax 		# Load address of format (%d)
	inc	%eax			# Skip space in format string
	pushl	%eax			# Push address of format string
	call	PRINTF
	call	flush

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_uint displays an unsigned integer to standard output
# pre:	uint in eax
# post:	none
print_uint:
	pushl	%ecx
	pushl	%edx
	pushl	%eax

	pushl   %ebp
	movl    %esp, %ebp
	andl    $0xfffffff0, %esp	# Mac - Align stack

	subl	$8, %esp		# added to keep SP 16 byte aligned
	pushl	%eax			# Push uint value
	movl	$f_uint, %eax 		# Load address of format (%u)
	inc	%eax			# Skip space in format string
	pushl	%eax			# Push address of format string
	call	PRINTF
	call	flush

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# read_int reads an integer from standard input
# pre:	none
# post:	integer is stored in eax
read_int:
	pushl	%ecx
	pushl	%edx
	pushl	%ebp			# Set up stack frame
	movl	%esp, %ebp
	subl	$4, %esp		# Create room for local variable

	lea	-4(%ebp), %eax		# Find address of local variable

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp		# added to keep SP 16 byte aligned
	pushl	%eax			# Store address on stack
	pushl	$f_int			# Push address of format (%d)
	call	SCANF

	movl	-4(%ebp), %eax		# Move value into %eax

	leave
	popl	%edx
	popl	%ecx

	ret

# read_uint reads an integer from standard input
# pre:	none
# post:	unsigned integer is stored in eax
read_uint:
	pushl	%ecx
	pushl	%edx
	pushl	%ebp			# Set up stack frame
	movl	%esp, %ebp
	subl	$4, %esp		# Create room for local variable

	lea	-4(%ebp), %eax		# Find address of local variable

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp		# added to keep SP 16 byte aligned
	pushl	%eax			# Store address on stack
	pushl	$f_uint			# Push address of format (%d)
	call	SCANF

	movl	-4(%ebp), %eax		# Move value into %eax

	leave
	popl	%edx
	popl	%ecx
	ret

# read_str reads a white-space delimited string into a buffer
# pre:	address of buffer is in eax
# post:	none
read_str:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

	subl	$8, %esp		# added to keep SP 16 byte aligned
	pushl	%eax			# Push address of buffer
	pushl	$f_str			# Push address of format (%s)
	call	SCANF

	leave
	popl	%eax
	popl	%edx
	popl	%ecx

	ret

# read_line reads a newline terminated line into a buffer
# pre:	address of buffer is in eax
# post:	none
read_line:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebx
	pushl   %ebp
	movl    %esp, %ebp

	# Use fgets instead of gets, to prevent warning.
	# Misuse fgets actually, since we are using it with a fixed
	# size buffer.

	movl	%eax, %ebx		# Move buffer address into EBX

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl    $4, %esp		# Mac - Keep stack aligned at call
	pushl	ASM_STDIN		# Push stdin FILE* pointer
	pushl	$256			# Push max size of buffer
	pushl	%ebx			# Push address of buffer
	# fgets will read up to 255 characters (including the newline)
	# and place a null after the last character read
	call	FGETS

	# Chomp (overwrite with null) newline at end of line (if present)
	addl    $4, %esp        	# Keep 16 byte alignment of ESP for Mac
	pushl	%ebx			# get length of line
	call	STRLEN

	decl	%eax
	cmpb	$10, (%ebx,%eax,1)	# check for newline
	jnz     0f
	movb	$0, (%ebx,%eax,1)	# chomp newline

0:	leave
	popl	%ebx
	popl	%eax
	popl	%edx
	popl	%ecx

	ret

# read_nl reads and discards characters up to a newline
# pre:	none
# post:	none
read_nl:
	pushl	%ecx			# These 3 may be destroyed by printf,
	pushl	%edx			# save them for later restoration
	pushl	%eax
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

0:	call	GETCHAR
	cmp	$10, %eax		# Have we read a newline char yet?
	jnz	0b

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# flush flushes all pending output
# pre:	none
# post:	none
flush:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl   %ebp
	movl    %esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

	subl	$12, %esp		# added to keep SP 16 byte aligned
	pushl	$0			# If arg is 0, flush all files
	call	FFLUSH

	leave
	pop	%eax
	pop	%edx
	pop	%ecx
	ret

# print_single/print_double displays a floating-point number to standard ouput
# pre:	floating point number in st(0)
# post:	FPU stack is unaltered
#       All CPU regs are left unaltered
print_single:
print_double:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebp
	movl	%esp, %ebp

	andl    $0xfffffff0, %esp	# Mac - Align stack

	# Push st(0) as first arg
	subl	$12, %esp		# Mac - Keep stack aligned at call
	fstl	(%esp)
	# Must save all FPU regs, they could be changed by PRINTF
	fsave	FPUSAVE			# Save (and clear) all FPU registers
	pushl	$f_sing			# Push address of format (%e)
	incl	(%esp)			# Skip space in format string
	call	PRINTF
	call	flush

	frstor	FPUSAVE
	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_singleptr displays a floating-point number to standard ouput
# pre:	address of floating point number in EAX
# post:	FPU stack is unaltered
#       All CPU regs are left unaltered
print_singleptr:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebp
	movl	%esp, %ebp

	# Must save all FPU regs, they could be changed by PRINTF
	fsave	FPUSAVE			# Save (and clear) all FPU registers

	# Convert float to double and push on stack
	andl    $0xfffffff0, %esp	# Mac - Align stack
	flds    (%eax)
	subl	$12, %esp		# Mac - Keep stack aligned at call
	fstpl	(%esp)
	pushl	$f_sing			# Push address of format (%e)
	incl	(%esp)			# Skip space in format string
	call	PRINTF
	call	flush

	frstor	FPUSAVE
	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# print_doubleptr displays a floating-point number to standard ouput
# pre:	address of floating point number in EAX
# post:	FPU stack is unaltered
#       All CPU regs are left unaltered
print_doubleptr:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebp
	movl	%esp, %ebp

	# Must save all FPU regs, they could be changed by PRINTF
	fsave	FPUSAVE			# Save (and clear) all FPU registers

	# Push double on stack
	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl    $4, %esp		# Mac - Keep stack aligned at call
	pushl   4(%eax)
	pushl   (%eax)
	pushl	$f_sing			# Push address of format (%e)
	incl	(%esp)			# Skip space in format string
	call	PRINTF
	call	flush

	frstor	FPUSAVE

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# read_single/read_double reads a floating-point number from standard input
# pre:	none
# post:	floating point number in st(0), rest of FPU stack is unaltered
#       All CPU regs are left unaltered
read_single:
read_double:
	pushl	%ecx
	pushl	%edx
	pushl	%eax

	pushl	%ebp
	movl	%esp, %ebp

	# Save space on stack for number (double)
	subl	$8, %esp
	# Must save all, they could be changed by PRINTF
	fsave	FPUSAVE

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp		# Mac - Keep stack aligned at call
	lea	    -8(%ebp),%eax
	pushl	%eax
	pushl	$f_doub
	call	SCANF

	# Restore FPU registers
	frstor	FPUSAVE

	# Pop top number off and push new number on to stack
	# to maintain stack contents (except for %st(0))
	fstp	%st(0)
	fldl	-8(%ebp)          	# Copy value onto FPU stack

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# read_singleptr reads a floating-point number from standard input
# pre:	Address where number is stored in EAX
# post:	Number stored at address in EAX
read_singleptr:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebp
	movl	%esp, %ebp

	# Must save all, they could be changed by scanf
	fsave	FPUSAVE

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp		# Mac - Keep stack aligned at call
	pushl	%eax
	pushl	$f_sing
	call	SCANF

	# Restore FPU registers
	frstor	FPUSAVE

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

# read_doubleptr reads a floating-point number from standard input
# pre:	Address where number is stored in EAX
# post:	Number stored at address in EAX
read_doubleptr:
	pushl	%ecx
	pushl	%edx
	pushl	%eax
	pushl	%ebp
	movl	%esp, %ebp

	# Must save all, they could be changed by scanf
	fsave	FPUSAVE

	andl    $0xfffffff0, %esp	# Mac - Align stack
	subl	$8, %esp		# Mac - Keep stack aligned at call
	pushl	%eax
	pushl	$f_doub
	call	SCANF

	# Restore FPU registers
	frstor	FPUSAVE

	leave
	popl	%eax
	popl	%edx
	popl	%ecx
	ret

