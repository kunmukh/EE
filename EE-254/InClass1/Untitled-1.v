module InClass1 (A, B, F);

input [1:0] A;
input [1:0] B;
output F;

wire B0_n, B1_n, and_out1, and_out2, and_out3;

not inv1 (B0_n, B[0]);
not inv2 (B1_n, B[1]);

and and1 (and_out1, B1_n, B0_n, A[0]);
and and2 (and_out2, A[1], B1_n);
and and3 (and_out3, A[1], A[0], B0_n);

or or1 (F, and_out1, and_out2, and_out3);

endmodule 