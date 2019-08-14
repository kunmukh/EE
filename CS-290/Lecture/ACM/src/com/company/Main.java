package com.company;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Random;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
	Main m = new Main();
        try {
            m.cricket();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    void cricket() throws IOException {
        /*Scanner s = new Scanner(System.in).useDelimiter(" ");
        int xI = 0,yI = 0;
        double x,y,d,d1Max = 0, d2Max = 0;
        int xMax1 = 0, yMax1 = 0, xMax2 = 0, yMax2 = 0;
        for (int i = 0; i < 12; i++) {

            xI = s.nextInt();
            yI = s.nextInt();

            x = ((double) xI);
            y = ((double) yI);
            d = Math.sqrt(Math.pow(x,2.0)+ Math.pow(y,2.0));
            if (d > d1Max){
                d2Max = d1Max;
                xMax2 = xMax1;
                yMax2 = yMax1;
                d1Max = d;
                xMax1 = ((int) x);
                yMax1 = ((int) y);
            }
            if (d < d1Max && d > d2Max){
                d2Max = d;
                xMax2 = ((int) x);
                yMax2 = ((int) y);
            }
        }

        System.out.println("(" + xMax1 + " " + yMax1 + ")(" + xMax2 + " " + yMax2 + ")");*/

       BufferedWriter out = null;
        Random rand = new Random();

        int  n = rand.nextInt(100000) + 1;

        try {
            FileWriter fstream = new FileWriter("rn.txt", true); //true tells to append data.
            out = new BufferedWriter(fstream);

            for (int i = 1; i < 999999; i++) {
                out.write(i + " ");

                out.write((i + 1) + " ");


            }

        }
        catch (IOException e) {
            System.err.println("Error: " + e.getMessage());
        }

        if (out != null) {
            out.close();
        }


    }
}
