import org.junit.Test;

import java.util.ArrayList;

import static org.junit.Assert.assertEquals;

public class TermTest {
    @Test
    public void testConstructor(){
        System.out.print("\nTerm Testing Starts\n");

        int coeff = 0;
        ArrayList<Character> vars  = new ArrayList<>();
        ArrayList<Integer> exps = new ArrayList<>();
        char temp_var;
        int temp_exp;
        Term t =  new Term(coeff, vars, exps);
        checkTerm(t.toString(),"");
        System.out.print(t + "\n");

        vars.clear();
        exps.clear();
        coeff = 3;
        temp_var = 'x';
        temp_exp = 0;
        vars.add(temp_var);
        exps.add(temp_exp);
        Term t1 =  new Term(coeff, vars, exps);
        checkTerm(t1.toString(),"3");
        System.out.print(t1 + "\n");

        vars.clear();
        exps.clear();
        coeff = -13;
        temp_var = 'x';
        temp_exp = -3;
        vars.add(temp_var);
        exps.add(temp_exp);
        Term t2 =  new Term(coeff, vars, exps);
        checkTerm(t2.toString(), "-13x^-3");
        System.out.print(t2 + "\n");

        vars.clear();
        exps.clear();
        coeff = 3;
        temp_var = 'x';
        temp_exp = 2;
        vars.add(temp_var);
        exps.add(temp_exp);
        temp_var = 'y';
        temp_exp = 3;
        vars.add(temp_var);
        exps.add(temp_exp);
        Term t3 =  new Term(coeff, vars, exps);
        checkTerm(t3.toString(), "3x^2y^3");
        System.out.print(t3 + "\n");

        vars.clear();
        exps.clear();
        coeff = -3;
        temp_var = 'a';
        temp_exp = -3;
        vars.add(temp_var);
        exps.add(temp_exp);
        temp_var = 'x';
        temp_exp = 2;
        vars.add(temp_var);
        exps.add(temp_exp);
        temp_var = 'y';
        temp_exp = 3;
        vars.add(temp_var);
        exps.add(temp_exp);
        Term t4 =  new Term(coeff, vars, exps);
        checkTerm(t4.toString(), "-3a^-3x^2y^3");
        System.out.print(t4 + "\n");
    }

    private void checkTerm (String calculated, String shouldBe){
        assertEquals(calculated, shouldBe);
    }
}
