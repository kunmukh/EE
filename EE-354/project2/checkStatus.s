		AREA MyProg, CODE, READONLY
		EXPORT delaye
GPIOC_ODR EQU 0x40020814

checkStatus PROC		
		push {r0-r5};
		ldr r3, =GPIOC_ODR;
		mov r1, #0;
		str r1, [r3];		
		cmp r3, #0;
		BNE Lp2;
		mov r5, #1
Lp2		mov r5, #0;
		pop {r0-r5};
		bx lr;
		ENDP
	END