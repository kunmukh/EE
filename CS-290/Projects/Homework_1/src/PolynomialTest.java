import org.junit.Assert;
import org.junit.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;



public class PolynomialTest {
    @Test
    public void testConstructor(){
        System.out.print("\nPolynomial Testing Starts\n");

        //Test 1
        Polynomial p = new Polynomial("0");
        System.out.print(p + "\n");

        //Test 2
        Polynomial p1 = new Polynomial("2x");
        checkPoly(p1.toString(),"+2x");
        System.out.print(p1 + "\n");

        //Test 3

        Polynomial p2 = new Polynomial("2x^2+y");
        checkPoly(p2.toString(),"+2x^2+y");
        System.out.print(p2.toString() + "\n");


        //Test 4
        Polynomial p3 = new Polynomial("5xy^2-2x+6");
        checkPoly(p3.toString(),"+5xy^2-2x+6");
        System.out.print(p3 + "\n");

        //Test 5
        Polynomial p4_1 = new Polynomial("3x^2+2x");
        Polynomial p4_2 = new Polynomial("0");

        Polynomial p4_3 = p4_1.add(p4_2);
        Polynomial p4_4 = p4_2.add(p4_1);
        Polynomial p4_5 = p4_1.sub(p4_2);
        Polynomial p4_6 = p4_2.sub(p4_1);
        Polynomial p4_7 = p4_1.mult(p4_2);
        Polynomial p4_8 = p4_2.mult(p4_1);


        checkPoly(p4_3.toString(),"+3x^2+2x");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+3x^2+2x");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"+3x^2+2x");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"-3x^2-2x");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"0");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"0");
        System.out.println(p4_8);



        //Test 6
        p4_1 = new Polynomial("2x+3y");
        p4_2 = new Polynomial("4x+6y");
        p4_3 = p4_1.add(p4_2);
        p4_4 = p4_2.add(p4_1);
        p4_5 = p4_1.sub(p4_2);
        p4_6 = p4_2.sub(p4_1);
        p4_7 = p4_1.mult(p4_2);
        p4_8 = p4_2.mult(p4_1);

        checkPoly(p4_3.toString(),"+6x+9y");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+6x+9y");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"-2x-3y");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"+2x+3y");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"+8x^2+24xy+18y^2");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"+8x^2+24xy+18y^2");
        System.out.println(p4_8);

        //test 7
        p4_1 = new Polynomial("3x^2+2x");
        p4_2 = new Polynomial("1");
        p4_3 = p4_1.add(p4_2);
        p4_4 = p4_2.add(p4_1);
        p4_5 = p4_1.sub(p4_2);
        p4_6 = p4_2.sub(p4_1);
        p4_7 = p4_1.mult(p4_2);
        p4_8 = p4_2.mult(p4_1);

        checkPoly(p4_3.toString(),"+3x^2+2x+1");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+1+3x^2+2x");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"+3x^2+2x-1");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"+1-3x^2-2x");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"+3x^2+2x");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"+3x^2+2x");
        System.out.println(p4_8);

        //test8
        p4_1 = new Polynomial("3x^2+2x");
        p4_2 = new Polynomial("2x");
        p4_3 = p4_1.add(p4_2);
        p4_4 = p4_2.add(p4_1);
        p4_5 = p4_1.sub(p4_2);
        p4_6 = p4_2.sub(p4_1);
        p4_7 = p4_1.mult(p4_2);
        p4_8 = p4_2.mult(p4_1);

        checkPoly(p4_3.toString(),"+4x+3x^2");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+4x+3x^2");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"+3x^2");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"-3x^2");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"+6x^3+4x^2");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"+6x^3+4x^2");
        System.out.println(p4_8);

        //test9
        p4_1 = new Polynomial("x+2");
        p4_2 = new Polynomial("x+1");
        p4_3 = p4_1.add(p4_2);
        p4_4 = p4_2.add(p4_1);
        p4_5 = p4_1.sub(p4_2);
        p4_6 = p4_2.sub(p4_1);
        p4_7 = p4_1.mult(p4_2);
        p4_8 = p4_2.mult(p4_1);

        checkPoly(p4_3.toString(),"+2x+3");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+2x+3");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"+1");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"-1");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"+x^2+3x+2");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"+x^2+3x+2");
        System.out.println(p4_8);

        //test10
        p4_1 = new Polynomial("x+2");
        p4_2 = new Polynomial("2x^2+2x+3");
        p4_3 = p4_1.add(p4_2);
        p4_4 = p4_2.add(p4_1);
        p4_5 = p4_1.sub(p4_2);
        p4_6 = p4_2.sub(p4_1);
        p4_7 = p4_1.mult(p4_2);
        p4_8 = p4_2.mult(p4_1);

        checkPoly(p4_3.toString(),"+3x+5+2x^2");
        System.out.println(p4_3);
        checkPoly(p4_4.toString(),"+3x+5+2x^2");
        System.out.println(p4_4);
        checkPoly(p4_5.toString(),"-1x-1-2x^2");
        System.out.println(p4_5);
        checkPoly(p4_6.toString(),"+x+1+2x^2");
        System.out.println(p4_6);
        checkPoly(p4_7.toString(),"+2x^3+6x^2+7x+6");
        System.out.println(p4_7);
        checkPoly(p4_8.toString(),"+2x^3+6x^2+7x+6");
        System.out.println(p4_8);

        //test11
        p2 = new Polynomial("3xy+z^2");
        char[] c = {'x','y','z'};
        double[] d = {1.0,2.0,3.0};
        double result = p2.evaluate(c,d);
        assertEquals(result,15);
        System.out.print("Eval " + result + "\n");

    }

    @Test
    public void testBadvariable(){
        assertThrows(ArithmeticException.class, () -> {
            Polynomial p2 = new Polynomial("3xy+z^2");
            char[] c = {'x','y'};
            double[] d = {1.0,2.0,3.0};
            double result = p2.evaluate(c,d);
            assertEquals(result,15);
            System.out.print("Eval " + result + "\n");
        });
    }
    @Test
    public void testBadvariable_1(){
        assertThrows(ArithmeticException.class, () -> {
            Polynomial p2 = new Polynomial("3xy+z^2");
            char[] c = {'x','y','a'};
            double[] d = {1.0,2.0,3.0};
            double result = p2.evaluate(c,d);
            assertEquals(result,15);
            System.out.print("Cannot Eval " + result + "\n");
        });
    }


    private void checkPoly (String calculated, String shouldBe){
        Assert.assertEquals(calculated, shouldBe);
    }


}
