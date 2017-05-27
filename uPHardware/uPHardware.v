//The uPHardware

module uPHardware(InBus, Control, clk, OutBus, Z);
input [3:0] InBus;
input [6:0] Control;
input clk;

output [3:0] OutBus;
output Z;

wire [3:0] Rout;
wire [3:0] Mout;
wire [3:0] Qout;
wire [3:0] Aout;
wire [3:0] n_Qout;

GPRegister R (Control[5], 1'b0, Control[2], clk, Aout, Rout);
GPRegister M (Control[1], 1'b0, 1'b0, clk, InBus, Mout);
GPRegister Q (Control[6], Control[4], 1'b0, clk, InBus, Qout); 

ALU AOL (Mout, Rout, Aout, Control[3], clk);

assign n_Qout = ~Qout;
nand (z, n_Qout[0], n_Qout[1], n_Qout[2], n_Qout[3]);

assign OutBut = Control[0] ? Rout : 4'bzzzz; //if Control[0]= 1,  outbus = Rout, 
                                              //else rout is high impedence mode

endmodule

//GPRegister Module

module GPRegister(L, D, C, clk, BusIn, BusOut);
input L, D, C;
input clk;
input [3:0] BusIn;
output [3:0] BusOut;
reg [3:0] BusOut;

always@ (posedge clk)
begin
	if(clk)
		begin
			if(L)
			begin
				BusOut <= BusIn;
			end
			if(C)
			begin
				BusOut <= 0;
			end
			if (D)
			begin
				BusOut <= BusOut - 4'b0001;
			end
		end
end
endmodule

//ALU Module


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
