; Kunal Mukherjee
; 9/17/2018
; Assignment 2
; EE354

; if the button is pushed one time it will output
;an A-note (440Hz) for about 1 second. 
;When the button is pushed a second time 
;it will output a Cnote (261Hz) for about 1 second. 
;A third push goes back to an A-note, etc. 

;TODO: Put in the 1sec loop  

MainSeg SEGMENT CODE	
	;Macro to set P1.0 to 1
	SetBit MACRO 
				setb P1.0
				ENDM
	;Macro to set P1.0 to 0			
	ClrBit MACRO 
				clr P1.0
				ENDM
				
	CSEG at 0h
		ljmp Start
	RSEG MainSeg
		mov a, #00h
		Start: 	cjne a, #0, ReadA ;if R2 is 0, then choose A
				cjne a, #1, ReadC ;if R2 is 1, then choose C 			
				jmp Start ; jump to start
		
		ReadA: 	mov c, P3.4 ; Read button state
				jnc CloseA ; jump to Close, if pressed			
				jmp Start  ; jump to Start, if not pressed
			
		CloseA: inc a ; if A is selected, incr R2, so that C is selected next time
				mov R1, #2
		LP:		mov R2, #250
		LP1:	Call PlayA ; calls play A in a loop for 500 times
				djnz R2, LP1
				djnz R1, LP				
				jmp Start
				
		;subroutine for play	
		PlayA: 	ClrBit ;turn p1.0 on
				call DelayA ;calls delay
				SetBit ;turn p1.0 off			
				RET
				
		;subroutine to delay A	
		DelayA: mov R3, #170 ;delays it for (1/440) /2 = 0.0013 sec
		LP4:	mov R4, #8
		LP3:	djnz R4, LP3
				djnz R3, LP4			
				RET			
					
		ReadC: 	mov c, P3.4 ;Read button state
				jnc CloseC ; jump to Close, if pressed
				jmp Start  ; jump to Start, if not pressed
			
		CloseC: dec a ;if C is selected, dec R2, so that A is selected next time
				mov R1, #2
		LP6:	mov R2, #250
		LP5:	Call PlayC
				djnz R2, LP5
				djnz R1, LP6
				setb c
				jmp Start
				
		;subroutine to playC	
		PlayC: 	ClrBit ;turn p1.0 on
				call DelayC
				SetBit ;turn p1.0 off			
				RET; once finished return to start		
				
		;subroutine to delau C		
		DelayC: mov R3, #170;delays it for (1/261) /2 = 0.0019 sec
		LP8:	mov R4, #12
		LP7:	djnz R4, LP7
				djnz R3, LP8				
				RET			
		
		END
			