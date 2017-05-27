//Kunal Mukherjee
//EE 254
//4/21/2017
//Mr. Mark Randall
//uProcessor implementation and design to build a 4-bit
//multiplier which use successive addition M = R*Q


//Driver Module of the microprocessor
module BCD7Seg(btn0, btn1, SW, CLOCK_50, Seg1, Seg4, Bout);

//the two input button ; btn0 - clk0 and btn1- reset; input CLOCK_50
// for debounching
input btn0, btn1, CLOCK_50;
//sw is the 4-bit input to be multiplied
input [3:0] SW;
//7-bit seg1 output for display bcd to 7-seg display
output [6:0] Seg1;
//7-bit seg4 output for the display of the control signal
output [6:0] Seg4;
//10-bit Bout to display the control unit LEDS and the 4-bit input
output [9:0] Bout; 

//a variable clk0 that is trigged when btn0 is pressed
reg clk0;
//a variable reset that is trigged when btn1 is pressed
reg reset;

//wire Zin to connect the uPController to the uPHardware
wire Zin;
//4-bit output bus to have the result
wire [3:0] OutBus;
//7-bit control bus to have the control signal for the uPHardware
wire [6:0] ControlBus;

//intializing the clk0 to be 0 and reset to be 0
initial
begin
	clk0 <= 0;
	reset <= 0;
end

//Debouncing btn0 using a D-type FF and CLOCK_50 as the clock
always@(posedge CLOCK_50)
begin
	if(!btn0) //if button is not pressed load 1 into clk0
		clk0 <= 1'b1;
	else //else load 0
		clk0 <= 1'b0;
end

////Debouncing btn0 using a D-type FF and CLOCK_50 as the clock
always@(posedge CLOCK_50)
begin
	if(!btn1) //if button is not pressed load 1 into reset
		reset <= 1'b1;
	else //else load 0
		reset <= 1'b0;
end

//uPController1 (clk, reset, Zin, ControlBus, seg)
uPController1 MC (clk0, reset, Zin, ControlBus, Seg4);
//uPHardware(InBus, Control, clk, OutBus, Zout)
uPHardware MH (SW, ControlBus, clk0, OutBus, Zin);
//block for converting bcd digit of outBus into 7 segment format
BCDSeg dig1 (OutBus, Seg1);

//it assigns the controlbus output to the Bout for LED display
assign Bout[9:4] = ControlBus;
//it assigns the input display to the Bout for LED display
assign Bout [3:0] = SW;

endmodule


//***********************************************************//
//The uProcessor Hardware
module uPHardware(InBus, Control, clk, OutBus, Zout);
//The inputs 4-bit inBus and 7-bit Control instruction to run
//the uProcessor and the clk input
input [3:0] InBus;
input [6:0] Control;
input clk;
//The 4-bit output and the Zout, which is used in the control
//unit circuitry
output [3:0] OutBus;
output Zout;

//the 4-bit wires to connect to the Rout and Mout with the ALU
//ALUout is the 4-bit wire connected to the output of ALU
//4 bit Qout and n_Qout connected to the output of Q register
// used for the control unit circuitry
wire [3:0] Rout;
wire [3:0] Mout;
wire [3:0] Qout;
wire [3:0] ALUout;
wire [3:0] n_Qout;

//The R , M, Q registers are intitialized as per the design of the 
//uProcessor hardware
//for R: (L= control instruction 5, D = 0, C= control inst. 2, 
//                          clk, BusIn = ALUout, BusOut = Rout)
GPRegister R (Control[5], 1'b0, Control[2], clk, ALUout, Rout);
//for R: (L= control instruction 1, D = 0, C= 0, 
//                          clk, BusIn = inBus, BusOut = Mout)
GPRegister M (Control[1], 1'b0, 1'b0, clk, InBus, Mout);
//for R: (L= control instruction 6, D = Contro. inst 4, C= 0, 
//                          clk, BusIn = inBus, BusOut = Qout)
GPRegister Q (Control[6], Control[4], 1'b0, clk, InBus, Qout); 

//Arithmetic Logic Unit to sucessive add the right and left side
ALU AOL (Mout, Rout, ALUout, Control[3]);
// assigning the bit-wise not to the Qout, a inverter was needed
assign n_Qout = ~Qout;
//using a nand gate to get the Zout , which will be used in
//control unit circuitry
nand (Zout, n_Qout[0], n_Qout[1], n_Qout[2], n_Qout[3]);
//asssiging the ALUout to the OutBus to be displayed														 
assign OutBus = ALUout;

endmodule

//*************************************************//
//ALU Module
module ALU(L, R, Result, Sel);
//Declaration of the input of the ALU
//The 4-bit left  and right input
input [3:0] L;
input [3:0] R;
//The 4 bit result
output [3:0] Result;
//The selection bit from the control hardware
input Sel;
//4-bit register declared for the result
reg [3:0] Result;

//At the posedge of the sel bit
//The right and left value would be added
always @(posedge Sel)
begin	
	Result <= R+L;	
end
//end module
endmodule

//*******************************************************//
//GPRegister Module
module GPRegister(L, D, C, clk, BusIn, BusOut);
//input of the register
//l-load ; d-decrement; c-control select
input L, D, C;
//clk input
input clk;
//defining the 4-bit input bus
input [3:0] BusIn;
//defining the 4-bit output
output [3:0] BusOut;
// reg Count to store the value after the register operation
reg [3:0] Count;

//always block to trigger when the positive edge of the clock
always@ (posedge clk)
begin
			if(L) //if Load is selected
			begin
				Count <= BusIn; //load the input bus into the Count
			end
			if(C) //if Clear is selected
			begin
				Count <= 0; //load 0 into the Count
			end
			if (D) // if Decrement is selected
			begin
				Count <= BusOut - 1; //decrement the outputbus value by 1 and load
			end		                //it into the Count
end

//After the operation is done , load the BusOut with the Count value
assign BusOut [3:0] = Count;
endmodule

//****************************************************************//
//uController
module uPController1 (clk, reset, Zin, ControlBus, seg);
//the clock input, the Zin input coming from the uController Hardware
//and the reset input to reset the program counter
input clk;
input Zin;
input reset;

//The 7-bit Control signal output 
//the 7-bit output for the 7-seg display
output [6:0] ControlBus;
output [6:0] seg;

//3-bit program counter that store the address for next uinstruction (6 instrc)
reg [2:0] pCounter;
//2-bit condition select: 00- increment; 01-load and; 10-halt 
reg [1:0] conditionSelect;
//3-bit brach address of the uinstrcution
reg [2:0] branchAddress;
//12-bit control register to store the microinstructions
reg [11:0] ControlRegister;
//12 bit data(output) lines and 6 bit address(input) line
reg [11:0] ROM [5:0]; //6 postion and save 12-bit for each data 12x6

//The segment display module is called with the program counter
// and the segment display
BCDSeg dig (pCounter, seg);

//initializing block
initial
begin
	//set the program counter to 0
	pCounter <= 0;
	//load the Rom address with the corresponding data value
	ROM[0]<=12'b000000000110;
	ROM[1]<=12'b000001000000;
	ROM[2]<=12'b000000111000;
	ROM[3]<=12'b010100000000;
	ROM[4]<=12'b000000000001;
	ROM[5]<=12'b101010000000;
end

//Always at the positive edge of the clock
always @(posedge clk)
begin	
	//load the uinstruction at the brach address of the rom
	ControlRegister <= ROM[pCounter];
	//load the last 2-bit of the ROM as they are the condition select
	conditionSelect <= ControlRegister[11:10];
	//the last 2-bit of the ROM data after the controlRegister has the
	//branch address store in them	
	branchAddress <= ControlRegister[9:7];	
end

//trigger this loop when the negetive edge of the clock or
//if the reset is pressed
always @ (negedge clk, posedge reset)
begin
	if(reset) //if the reset is selected
	begin
		pCounter <= 0; // it resets the program counter
	end
	else
	begin
	case(conditionSelect) // if conditionSelect is 00- increment
		2'b00:
		begin
			pCounter <= pCounter + 1; // the pCounter is incremented by 
		end		
		2'b01: //if 01- load
		begin
			if(Zin) //if Zin is selected also; condition jump
			pCounter <= branchAddress; //the branch address of where the
			                           // loop will go on for sucessive addition  
			else
			pCounter <= pCounter + 1; //else the program pCounter is incremented by 1;
		end
		2'b10: //if 10-halt
		begin 
		pCounter <= branchAddress; //unconditional jump; the output to be displayed 
		                           //continuously, so the last branch address is fed into
											// pCounter which stores the branch address of the next
											// uinstruction 
		end
	endcase
	end
end 

//after the countrol unit circuitry has run, the 7-bit controlRegister which
//contans the uProcessor instructions are loaded into the controlbus which
//is the output
assign ControlBus = ControlRegister [6:0];

endmodule

//************************************************************//
// A BCD7
module BCDSeg(bcd,seg);
     
//Declare inputs,outputs and internal variables.
input [3:0] bcd;
output [6:0] seg;
reg [6:0] seg;

//always block for converting bcd digit into 7 segment format
    always @(bcd)
    begin
        case (bcd) //case statement
            0 : seg = 7'b1000000;
            1 : seg = 7'b1111001;
            2 : seg = 7'b0100100;
            3 : seg = 7'b0110000;
            4 : seg = 7'b0011001;
            5 : seg = 7'b0010010;
            6 : seg = 7'b0000010;
            7 : seg = 7'b1111000;
            8 : seg = 7'b0000000;
            9 : seg = 7'b0011000;
            //switch off 7 segment character when the bcd digit is not a decimal number.
            default : seg = 7'b1111111; 
        endcase
    end

endmodule
