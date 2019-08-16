MainSeg SEGMENT CODE	
	CSEG at 0h
		ljmp Start
	RSEG MainSeg
	Start:		mov R3, #170
		LP4:	mov R4, #12
		LP3:	djnz R4, LP3
				djnz R3, LP4
			
			jmp Start
	
	END