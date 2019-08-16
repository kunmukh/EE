# file: example.s
# This is an example file that demonstrates the uses of the CS220 library.

.globl _asm_main

# Initialized data goes in the data segment
.section .data, ""
msg1: .asciz "Hello world!"
msg2: .asciz "Enter an integer: "
msg3: .asciz "Enter a line of text"
msg4: .asciz "Enter a float: "
buff: .fill 80, 1, 0
xval: .long 12345
fval: .float 23.85
dval: .double 23.85

# Uninitialized data goes in the bss segment
.section .bss, ""
yval: .fill 4, 1, 0

# Code goes in the text segment
.section .text, ""
_asm_main:
    enter	$0, $0		# set up stack frame
    pusha			# save registers

    # Display string at msg1 location
    movl $msg1, %eax
    call print_str
    call print_nl
    # Display the value of the xval variable
    movl xval, %eax
    call print_int
    call print_nl

    movl $msg4, %eax
    call print_str
    call read_single
    call print_single
    ffree %st(0)
    call print_nl

    movl $msg4, %eax
    call print_str
    movl $fval, %eax
    call read_singleptr
    movl $fval, %eax
    call print_singleptr
    call print_nl

    movl $msg4, %eax
    call print_str
    movl $dval, %eax
    call read_doubleptr
    movl $dval, %eax
    call print_doubleptr
    call print_nl

    # Prompt for an integer, store in yval variable
    movl $msg2, %eax
    call print_str
    call read_int
    movl %eax, yval
    call read_nl
    # Display the value of the yval variable
    movl yval, %eax
    call print_int
    call print_nl
    # Prompt for a line of text, store at location buff
    # Display the line of text.
    movl $msg3, %eax
    call print_str
    call print_nl
    movl $buff, %eax
    call read_line
    movl $buff, %eax
    call print_str
    call print_nl

    # End of user written code
    popa			# restore registers
    movl	$0, %eax	# return program status in eax
    leave			# restore stack frame
    ret
