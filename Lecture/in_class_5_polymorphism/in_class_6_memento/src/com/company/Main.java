package com.company;

public class Main {
    private int myval;


    public Memento createMemento() {
	// write your code here
        InternalMemento m = new InternalMemento();
        m.reallysensitivestuff = myval;

        return m;
    }
    public void setMemento(Memento m){
        InternalMemento i = (InternalMemento)m;

        myval = i.reallysensitivestuff;
    }
    private class InternalMemento implements Memento{
        public int reallysensitivestuff;
    }
    private void draw() {
        System.out.println();
        for (int i = 13; i < 25; i++) {
            if (board.getAmountArray()[i] != 0) {
                System.out.print("| " + board.getAmountArray()[i] + " "
                        + board.getColorArray()[i] + " | ");
            } else {
                System.out.print("|         | ");
            }
        }
        if ((board.getAmountArray()[25] != 0)) {
            System.out.print("| " + board.getAmountArray()[25] + " BB | ");
        } else {
            System.out.print("|         | ");
        }
        System.out.println();
        System.out
                .println("-----------------------------------------------------------");
        for (int i = 12; i > 0; i--) {
            if (board.getAmountArray()[i] != 0) {
                System.out.print("| " + board.getAmountArray()[i] + " "
                        + board.getColorArray()[i] + " | ");
            } else {
                System.out.print("|         | ");
            }
        }
        if ((board.getAmountArray()[0] != 0)) {
            System.out.print("| " + board.getAmountArray()[0] + " WB | ");
        } else {
            System.out.print("|         | ");
        }
        System.out.println();

    }
}
