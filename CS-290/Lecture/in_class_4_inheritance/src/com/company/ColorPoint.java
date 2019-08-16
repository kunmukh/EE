package com.company;

import java.awt.*;

public class ColorPoint extends Point{
    protected Color color;

    public ColorPoint(){
        super();
        this.color = Color.BLACK;
    }

    public ColorPoint(double x, double y){
        super(x,y);
        this.color = Color.BLACK;
    }
    public Color getColor() {
        return color;
    }
    @Override
    public String toString(){
        String superString = super.toString();
        return "#" + Integer.toHexString(color.getRGB()) + ": " + superString;
    }




}
