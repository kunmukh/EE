module ALU(L, R, Z, Sel, clk);
input [3:0] L;
input [3:0] R;
output [3:0] Z;
input Sel;
input clk;
reg [3:0] Z;

always @(posedge clk)
begin
	if(clk)
	begin
		if(Sel)
		begin
			Z <= R+L;
		end
	end
end
endmodule