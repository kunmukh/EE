MainSeg SEGMENT CODE
;
;
CSEG at 0h
	ljmp Start
RSEG MainSeg
Start: inc P0
	sjmp Start
	
	END