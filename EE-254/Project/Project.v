module Project(Clk,Clk2,CLOCK_50,LEDS);

input Clk;
input Clk2;
input CLOCK_50;

output [3:0] LEDS;

reg [3:0] LEDS;
reg [7:0] Condition;
reg [7:0] Conditions;
reg [7:0] Cont;
reg Clk0;
reg Right;
reg temp;

wire slow_clock;

clock_divider mdiv(CLOCK_50,slow_clock);

initial 
begin
	LEDS = 1;//1,2,4,8
	Condition = 2'b01;
	temp = 1'b0;
end

always @(negedge Clk2)
begin
	if(!temp)
		temp <= 1'b1;
	else
		temp <= 1'b0;
end

always @(negedge Clk)
begin
	if(!temp)
		Right <= 1'b1;
	else
		Right <= 1'b0;
end



always@(posedge slow_clock)
begin	
	if(Right)	
	begin		
		if (Condition == 2'b00 )
			begin
			LEDS <= 1;
			Condition <= Condition + 1'b1;
			Conditions <= 3'b111;	
			end		
			
		if (Condition == 2'b01)
			begin
			LEDS <= 2;
			Condition <= Condition + 1'b1;
			Conditions <= 3'b100;	
			end
			
		if (Condition == 2'b10)
			begin
			LEDS <= 4;
			Condition <= Condition + 1'b1;
			Conditions <= 3'b101;
			end
			
		if (Condition == 2'b11)
			begin
			LEDS <= 8;
			Condition <= Condition - 2'b11;
			Conditions <= 3'b110;
			end		
	end	
	
	else
	begin			
		if (Conditions == 3'b100)
			begin
			LEDS <= 1;
			Conditions <= Conditions + 2'b11;
			Condition <= 2'b01;
			end		
			
		if (Conditions == 3'b101)
			begin
			LEDS <= 2;
			Conditions <= Conditions - 1'b1;
			Condition <= 2'b10;			
			end
			
		if (Conditions == 3'b110)
			begin
			LEDS <= 4;
			Conditions <= Conditions - 1'b1;
			Condition <= 2'b11;
			end
			
		if (Conditions == 3'b111 )
			begin
			LEDS <= 8;
			Conditions <= Conditions - 1'b1;
			Condition <= 2'b00;
			end
	end
	
end
endmodule


module clock_divider(input fast_clock, output slow_clock);
parameter COUNTER_SIZE = 25;
parameter COUNTER_MAX_COUNT = (2 ** COUNTER_SIZE) - 1;
reg [COUNTER_SIZE-1:0] count;
always @(posedge fast_clock)
begin
	if(count ==COUNTER_MAX_COUNT)
	count <= 0;
	else
	count <= count+1;
end
assign slow_clock = count[COUNTER_SIZE-1];
endmodule