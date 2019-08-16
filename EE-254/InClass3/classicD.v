module classicD(D, clk, Q, Qn);
input clk, D;
output Q, Qn;
reg Q, Qn;

always@(clk or D)
	if(clk)
		begin
			Q <= D;
			Qn <= ~D;
		end
endmodule