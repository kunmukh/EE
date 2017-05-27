module uPController1 (clk, Z, ControlBus);
input clk;
input Z;
output [6:0] ControlBus;
reg [2:0] pCounter;
reg [1:0] conditionSelect;
reg [2:0] branchAddress;
reg [11:0] ControlRegister;
reg [6:0] ControlBus;
reg [5:0] ROM [11:0]; //5 postion and save 12-bit for each data

always @(posedge clk)
begin
if(clk)
begin

	ROM[0]=12'b000000000110;
	ROM[1]=12'b000001000000;
	ROM[2]=12'b000000111000;
	ROM[3]=12'b010100000000;
	ROM[4]=12'b000000000001;
	ROM[5]=12'b101010000000;

	
	ControlRegister = ROM[pCounter];
	conditionSelect = ControlRegister[11:10];
	branchAddress = ControlRegister[9:7];
	ControlBus = ControlRegister[6:0];


	case(conditionSelect)
	2'b00:pCounter = pCounter + 1'b1;
	2'b01:
	if(Z)
	pCounter = branchAddress;
	else
	pCounter = pCounter + 1'b1;
	2'b10:pCounter = branchAddress;
	endcase


end
end 
endmodule