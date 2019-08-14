public _Assem
	;Define a relocatable code segment named Assemb
	Assemb SEGMENT CODE
		;Select Assemb as the active segment
		RSEG Assemb
			;entry point of the C langauge call
		_Assem: mov R6, #0; ;return 0 as int
				mov R7, #0;
		ret
		end
			