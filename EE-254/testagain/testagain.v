module testagain(Clk,LEDS);

input Clk;
output [3:0] LEDS;
reg [3:0] LEDS;

initial 
begin
	LEDS = 0;
end

always@(posedge Clk)
begin
	LEDS = LEDS + 1'b1;
end
endmodule