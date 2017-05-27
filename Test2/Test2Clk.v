module Test2Clk(Clk,clk1,CLOCK_50,LEDS);

input Clk;
input clk1;
input CLOCK_50;

wire slow_clock;

output [3:0] LEDS;
reg [3:0] LEDS;

reg clk0;

reg direction;
reg temp;

clock_divider mdiv(CLOCK_50,slow_clock);

initial 
begin
	LEDS = 4'b0001;
	direction = 1'b0;
end

always @(negedge clk1)
begin
	if(!temp)
		temp <= 1'b1;
	else
		temp <= 1'b0;
end

always @(negedge Clk /*or negedge clk1*/)
begin
	if(!temp)
		direction <= 1'b1;
	else
		direction <= 1'b0;
end


always@(posedge CLOCK_50)
begin
	if (!Clk)
		clk0 <= 1'b1;
	else
		clk0 <= 1'b0;
end

always@(posedge clk0)
begin
	if (direction)
	begin
		if (LEDS == 4'b1000)
			LEDS <= 4'b0001;
		else
			LEDS <= LEDS << 1'b1;
	end
	else
	begin
		if (LEDS == 4'b0001)
			LEDS <= 4'b1000;
		else
			LEDS <= LEDS >> 1'b1;
	end	
end
endmodule


module clock_divider(input fast_clock, output slow_clock);

parameter COUNTER_SIZE = 21;
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