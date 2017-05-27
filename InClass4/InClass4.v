module myDFF(CLK, D, Q, Qn);
input D, CLK;
output Q, Qn;
reg Q, Qn;

always @(posedge CLK)
if(CLK)
	begin
		Q <= D;
		Qn <= ~D;
	end
endmodule

module InClass4(X, Y, CLK);
input X, CLK;
output Y;

wire Da, A, An, Xn;

not (Xn, X);
and (Da, An, Xn);
and (Y, A, Xn);

myDFF myD(CLK, Da, A, An);

endmodule 