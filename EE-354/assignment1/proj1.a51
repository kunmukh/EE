;Kunal Mukherjee
;Assignment 1

MainSeg SEGMENT CODE
	;
	CSEG at 0h
		ljmp Start
	RSEG MainSeg
		Start:  
				mov a, p3   ;read p3 into accumulator
				mov r3, p3	;move p3 in r3				
				anl a, #80h ;see if p3.7 is 1 or 0	
				jz L1       ;if 3.7 = 0 jump to L1
				jnz L2      ;if 3.7 = 1 jump to L2
				
		L1: 	mov a, r3
				anl a, #40h ;see if p3.6 is 1 or 0				
				jz L3       ;if 3.6 is 0, then L3
				jnz L4      ;if 3.6 is 1, then L4
				
		L2:		mov a, r3
				anl a, #40h ;see if p3.6 is 1 or 0
				jz L5       ;if 3.6 is 0, then L3
				jnz L6      ;if 3.6 is 1, then L4	
				
		L3:		clr P1.0    ;turn P1.0 LED on
				clr P1.1    ;turn P1.1 LED on
				sjmp Lo1     ;jump to Lo1
				
		L4:		setb P1.0    ;turn P1.0 LED on
				setb P1.1    ;turn P1.1 LED on		
				jmp Start   ;jmp to start 
				
		L5:		clr P1.0    ;turn P1.0 LED off
				clr P1.1    ;turn P1.1 LED off		
				jmp Start   ;jmp to start 
				
		L6:		clr P1.0    ;turn P1.0 LED on
				clr P1.1    ;turn P1.1 LED on	
				sjmp Lo4

		Lo1:	mov r2, #128
				jmp Lo2
		Lo4:	mov r2, #255
				jmp Lo2
				
		Lo2:	dec r2
			    mov r1, #255
		Lo3:	dec r1
				mov a, r1
				jnz Lo3
				mov a, r2
				jnz Lo2			
				setb P1.0    ;turn P1.0 LED off
				setb P1.1    ;turn p1.1 LED off
				mov r2, #128
		Lo6:	dec r2
				mov r1, #255
		Lo5:	dec r1
				mov a, r1
				jnz Lo5
				mov a, r2
				jnz Lo6
				jmp Start
				
		
					
		
END