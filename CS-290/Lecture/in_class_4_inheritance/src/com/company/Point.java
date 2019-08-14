package com.company;

public class Point {
    protected double x;
    protected double y;

    public Point(){
        this.x = 0.0;
        this.y = 0.0;
    }
    public Point(double x, double y){
        this.x = x;
        this.y = y;
    }

    public void moveBy(double dx, double dy){
        x = x + dx;
        y = y + dy;
    }

    @Override
    public String toString() {
        return "(" + x + "," + y + ")";
    }

    public double getY(){
        return y;
    }
    public double getX(){
        return x;
    }



}
