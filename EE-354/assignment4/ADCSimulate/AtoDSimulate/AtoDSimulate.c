//AtoDTest.c
#include <at89c51cc03.h>
//Takes the input from the A/D converter channel 0 and
// sends it to P2 and P3.  Assumes P2 has D/A triggered
// by bit P4.0.  P2 holds 8 MSBs.
/****************************************************************
To run this in the simulator start the logic analyzer and in Setup enter AIN0 
to allow input on channel 0 of the A/D.  To see the output enter
port2 in Setup.

To start a simulated sine wave on AIN0 type ADC() at the bottom of the 
simulator page in the command window after the prompt >
*/
void main (void) 
   {unsigned char tmp;     
    ADCF  = 0x01;        // P1.0 = ADC[0]
    ADCON = 0x20;        // Enable ADC Function
    ADCLK = 0x00;        // Prescaler to 0
    EA = 0;              //Turn off interrupts
    while(1)             // Loop Forever
     {ADCON &= 0xF8;     // Reset ADC Channel Select
      ADCON |= 0x00;     // Select ADC = Ch0            
      ADCON |= 0x20;     // Use Standard mode
      ADCON |= 0x08;     // Start ADC Convert
      tmp = (ADCON & 0x10);    // Get done bit
      while(tmp != 0x10)       // Loop until complete
        tmp = (ADCON & 0x10);
      P2 = ADDH;               // Send 8 MSB to P2
      P3 = ADDL;
      P4_0 = 0;                // Low going pulse to D to A 
      P4_0 = 1;                //  write line
      ADCON &= 0xEF;           // Clear ADEOC = 0
     }      
   }
