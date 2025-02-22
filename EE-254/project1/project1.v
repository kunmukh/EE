
module project1(Clk,/*Clk2,*/LEDS);

input Clk;
//input Clk2;

output [3:0] LEDS;

reg [3:0] LEDS;
reg [7:0] Condition;
reg [7:0] Cont;

initial 
begin
	LEDS = 1;//1,2,4,8
	Condition = 2'b01;
end

always@(posedge Clk/*,posedge Clk2*/)
begin	

	if(Clk)
	begin
		if (Condition == 2'b00)
			begin
			LEDS <= 1;
			Condition <= Condition + 1'b1;			
			end		
			
		if (Condition == 2'b01)
			begin
			LEDS <= 2;
			Condition <= Condition + 1'b1;			
			end
			
		if (Condition == 2'b10)
			begin
			LEDS <= 4;
			Condition <= Condition + 1'b1;			
			end
			
		if (Condition == 2'b11)
			begin
			LEDS <= 8;
			Condition <= Condition - 2'b11;			
			end
	end

	/*if(Clk2)
	//begin	
	if (Condition == 2'b11)		
		Condition <= 3'b110)
	else
		Condition <= Condition + 3'b110;
		
	begin
		if (Condition == 3'b100)
			begin
			LEDS <= 1;
			Condition <= Condition + 2'b11;
			end		
			
		if (Condition == 3'b101)
			begin
			LEDS <= 2;
			Condition <= Condition - 1'b1;
			end
			
		if (Condition == 3'b110)
			begin
			LEDS <= 4;
			Condition <= Condition - 1'b1;
			end
			
		if (Condition == 3'b111)
			begin
			LEDS <= 8;
			Condition <= Condition - 1'b1;
			end
		end
		
	//end*/
	
end




endmodule