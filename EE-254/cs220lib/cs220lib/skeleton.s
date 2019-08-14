# file: skeleton.s
# This is a template file that illustrates the general form of an assembly
# language program that is to be linked with the CS220 library.

.globl _asm_main

# Initialized data goes in the data segment
.section .data, ""

# Uninitialized data goes in the bss segment
.section .bss, ""

# Code goes in the text segment
.section .text, ""
_asm_main:
    enter	$0, $0		# set up stack frame
    pusha			# save registers

    # User-written code goes here.
    # Do not modify the code before or after this comment block.

    popa			# restore registers
    movl	$0, %eax	# return program status in eax
    leave			# restore stack frame
    ret
