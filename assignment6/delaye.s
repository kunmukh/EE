		AREA MyProg, CODE, READONLY
		EXPORT delaye
GPIOC_ODR EQU 0x40020814

delaye PROC		
		push {r0-r5};
		ldr r3, =GPIOC_ODR;
		mov r1, #0;
		str r1, [r3];
		mov r4, #0;
		mov r5, #1;
		mov r2, #10150;		
Lp2 	add r4, #1;
		cmp r4, r2;
		BNE Lp2;
		str r5, [r3];
		pop {r0-r5};
		bx lr;
		ENDP
	END