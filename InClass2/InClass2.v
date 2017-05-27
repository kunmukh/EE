module InClass2(ain, yout, enable);
input [1:0] ain;
output [3:0] yout;
input enable;
reg[3:0] yout;

always@(ain or enable)
begin
	if(enable == 1)
	begin
		if(~ain[1] && ~ain[0])
			yout = 4'b1110;
		if(~ain[1] && ain[0])
			yout = 4'b1101;
		if(ain[1] && ~ain[0])
			yout = 4'b1011;
		if(ain[1]&& ain[0])
			yout = 4'b0111;
	end	
	else
	yout = 4'b1111;
end
endmodule