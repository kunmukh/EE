import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.PushbackInputStream;
import java.util.ArrayList;

public class Polynomial {

    private ArrayList<Term> poly =  new ArrayList<>();

    private static Term nextTermFrom(PushbackInputStream s) {
        int sign = 1;
        int coef = 1;
        ArrayList<Character> vars = new ArrayList<>();
        ArrayList<Integer> exps = new ArrayList<>();
        try {
            if (s.available() == 0)
                return null;

            char ch = (char) s.read();
            if (ch == '-') {
                sign = -1;
                ch = (char) s.read();
            } else if (ch == '+') {
                ch = (char) s.read();
            }
            if (!Character.isDigit(ch)) {
                coef = 1;
            } else {
                coef = 0;
                while (Character.isDigit(ch)) {
                    coef = coef * 10 + ch - '0';
                    ch = (char) s.read();
                }
            }
            coef *= sign;
            while (Character.isLowerCase(ch)) {
                vars.add(ch);
                ch = (char) s.read();
                int exp;
                if (ch == '^') {
                    exp = 0;
                    ch = (char) s.read();
                    while (Character.isDigit(ch)) {
                        exp = exp * 10 + ch - '0';
                        ch = (char) s.read();
                    }
                } else {
                    exp = 1;
                }
                exps.add(exp);
            }
            s.unread(ch);
            // At this point the variables hold the following information about the term:
            // coef - The coefficient of the term
            // vars - The variables in the term
            // exps - The exponent of the corresponding variable from vars
            Term t = new Term(coef, vars, exps);
            //System.out.print("coef" + coef);
            return t;

        } catch (IOException e) {
            return null;
        }
    }

    private ArrayList<Term> sort (){

        ArrayList<Term> org = new ArrayList<>(poly);

        Term t1, t2;

        for (int i = 0; i < org.size(); i++){
            for (int j = 0; j < org.size(); j++){
                //System.out.print(" Original" + org.toString());
                if ( (org.get(i).isEqual(org.get(j))) && (i != j) ){
                    //System.out.print(" All in" + org.toString());
                    t1 = org.get(i);
                    t2 = org.get(j);
                    org.remove(i);
                    //System.out.print(" i removed" + org.toString());
                    org.remove(j - 1);
                    //System.out.print(" j-1 removed" + org.toString());
                    org.add(i, t1.add(t2));
                    //System.out.print( org.toString() + " i = " + i + " j = " + j
                }
            }
        }
        return org;
    }

    public Polynomial (){
        Polynomial p = new Polynomial("");
        poly = p.getTerms();
    }

    public Polynomial (ArrayList<Term> t){
        for (Term t1 : t){
            poly.add(t1);
        }
    }

    public Polynomial (String str){
        Term indiv;

        byte bytes[];
        ByteArrayInputStream bis;
        bytes = str.getBytes();
        bis = new ByteArrayInputStream(bytes);
        PushbackInputStream input = new PushbackInputStream(bis);

        indiv = nextTermFrom(input);
        poly.add(indiv);
        while (bis.available() > 0){
            indiv = nextTermFrom(input);
            //System.out.print(indiv + " ");
            poly.add(indiv);
        }

    }

    public ArrayList<Term> getTerms() {
        return poly;
    }

    public String toString() {
        boolean first = true;
        String temp = "";

        for (Term t : poly) {
            //System.out.println("t___" + t);
            if (!first &&
                    ((!t.toString().contains("0") && (t.toString().length() > 1))) ) {
                if (t.toString().contains("-") && !t.toString().isEmpty()) {
                    temp += t.toString();
                } else {
                    if (!t.toString().isEmpty()) {
                        temp = temp + "+" + t.toString();
                    }
                }

            }
            else{
                if (!t.toString().isEmpty()) {
                    if ((t.toString().contains("-"))) {
                        temp += t.toString();
                    } else {
                        temp = temp + "+" + t.toString();
                    }
                }
                else{
                    temp += "";
                }

                first = false;
            }
            //System.out.print("---*" + t.toString() + "*---");
        }
        if (temp.isEmpty()){
            temp = "0";
        }

        return temp;
    }

    public Polynomial add (Polynomial t2){

        Polynomial sum = addSubcombine(t2, "add");
        return sum;
    }

    public Polynomial sub (Polynomial t2){
        Polynomial sub = addSubcombine(t2, "sub");
        return sub;
    }

    public Polynomial mult (Polynomial t2) {
        Term temp;
        ArrayList<Term> temp_1 = new ArrayList<>();

        for (Term t : poly) {
            for (Term t_2 : t2.getTerms()) {
                temp = t.mult(t_2);
                //System.out.println("temp " + temp);
                temp_1.add(temp);
            }
        }

        Polynomial unSortedp = new Polynomial(temp_1);
        Polynomial sortedP = new Polynomial(unSortedp.sort());
        //System.out.print(" The element1 got " + newP1.toString() + " ");
        return sortedP;
    }

    public double evaluate (char[] a, double[] c){
        ArrayList<Double> sum = new ArrayList<>();
        double val = 0;
        try {
            for (Term t : poly) {
                val = t.evaluate(a, c);
                sum.add(val);
                //System.out.print(sum);
            }
            double ans = 0.0;
            for (Double d : sum) {
                ans += d;
            }


        boolean termContain = false;

        ArrayList<Character> charList =  new ArrayList<>();

        for (int i = 0; i < a.length; i++){
            charList.add(a[i]);
        }

        for (Term t : poly) {
            termContain = t.containElementinArray(charList);
        }

        if (!termContain){
            throw new ArithmeticException();
        }
        return ans;
        }
        catch (ArrayIndexOutOfBoundsException e){
            throw new ArithmeticException();
        }
    }

    public Polynomial addSubcombine (Polynomial t2, String op){
        Term temp;
        ArrayList<Term> temp_1  = new ArrayList<>();

        for (Term t : poly){
            for (Term t_2 : t2.getTerms()){
                if (t.isEqual(t_2)){
                    if (op.contains("sub")) {
                        temp = t.sub(t_2);
                    }
                    else{
                        temp = t.add(t_2);
                    }
                    temp_1.add(temp);
                }
            }
        }

        Polynomial temp_result = new Polynomial(temp_1);

        for (Term t : poly){
            if (!t.containElementinPoly(temp_result)){
                temp_1.add(t);
            }
        }
        int i;

        for (Term t : t2.getTerms()){
            if (!t.containElementinPoly(temp_result)){
                if (op.contains("sub")) {
                    i = t.getCoef() * -1;
                    Term t1 = new Term(i, t.getVar(), t.getExp());
                    temp_1.add(t1);
                }
                else{
                    temp_1.add(t);
                }
            }
        }
        //System.out.println("temp: " + temp_1);
        Polynomial result = new Polynomial(temp_1);

        return result;
    }

}




