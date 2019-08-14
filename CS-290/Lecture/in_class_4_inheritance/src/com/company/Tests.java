package com.company;

import org.junit.Test;
import java.awt.*;
import static junit.framework.TestCase.assertEquals;


public class Tests {
    @Test
    public void testInheritance(){
        Point p = new Point(4.0, 5.0);
        assertEquals(4.0, p.getX());
        assertEquals(5.0, p.getY());

        p.moveBy(2.0, -3.0);
        assertEquals(6.0, p.getX());
        assertEquals(2.0, p.getY());

        ColorPoint cp = new ColorPoint (4.0,5.0);
        assertEquals(4.0, cp.getX());
        assertEquals(5.0, cp.getY());

        cp.moveBy(2.0, -3.0);
        assertEquals(6.0, cp.getX());
        assertEquals(2.0, cp.getY());

        assertEquals(Color.BLACK, cp.getColor());

        Point p2 = p;
        ColorPoint cp2 = cp;

        p2 = cp;
        assertEquals(6.0, p2.getX());
        assertEquals(2.0, p2.getY());

        ColorPoint cp3 = (ColorPoint) p2;
        assertEquals(Color.BLACK, cp3.getColor());

    }
    @Test
    public void testToString(){
        Point p = new Point (4,5);
        assertEquals("(4.0,5.0)", p.toString());

        ColorPoint cp = new ColorPoint(4.0,5.0);
        assertEquals("#ff000000: (4.0,5.0)", cp.toString());

        ColorPoint cp2 = new ColorPoint();
    }
}
