#ifndef _CS220LIB_H
#define _CS220LIB_H

// The main assembly routine is _asm_main.  g++ on Windows automatically
// prepends an underscore on all C++ function names, so we need to
// call asm_main from C++.

#if defined(__linux__)
#define ASM_MAIN _asm_main
#else
#define ASM_MAIN asm_main
#endif

extern "C" int ASM_MAIN();

// stdin is a macro under Cygwin, so we can't access it directly from assembly.
// This forces evaluation of the macro and stores the result in a variable that
// we can access from assembly.  This is used as an argument to fgets().

FILE *asm_stdin = stdin;
#endif
