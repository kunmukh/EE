import java.util.ArrayList;
import java.util.Collections;

public class Term {
    private int coef;
    private ArrayList<Character> var;
    private ArrayList<Integer> exp;

    public int getCoef() {
        return coef;
    }

    public ArrayList<Character> getVar() {
        return var;
    }

    public ArrayList<Integer> getExp() {
        return exp;
    }

    public Term ( int coef, ArrayList<Character> var, ArrayList<Integer> exp) {

        ArrayList<Integer> new_var_temp = new ArrayList<>();
        ArrayList<Integer> new_var_temp_1 = new ArrayList<>();
        ArrayList<Character> new_var = new ArrayList<>();
        ArrayList<Integer> new_exp = new ArrayList<>();

        for (Character c : var){
            new_var_temp.add((int) c);
        }
        Collections.sort(new_var_temp);
        int temp1, temp1_1;
        int temp2, temp2_1;
        for (Integer i : new_var_temp){
            temp1 = i;
            temp1_1 = var.indexOf((char) temp1);
            temp2 = exp.get(temp1_1);

            new_exp.add(temp2);
            new_var.add((char) temp1);
        }

        this.var = new_var;
        this.coef =  coef;
        this.exp = new_exp;

        ArrayList<Character> n_var = new ArrayList<>();
        ArrayList<Integer> n_exp = new ArrayList<>();




    }

    public Term (Term t){
        coef = t.getCoef();
        var = t.getVar();
        exp = t.getExp();
    }

    public String toString() {
        ArrayList<Character> var_temp = new ArrayList<>(var);
        ArrayList<Integer> exp_temp = new ArrayList<>(exp);

        String temp = "";
        String special = "^";
        String s_expr = "";
        String s_coef = Integer.toString(coef);


        char temp_var;
        int str_length;
        int temp_exp;

        while (!exp_temp.isEmpty()){

            temp_var = var_temp.get(0);
            temp_exp = exp_temp.get(0);
            s_expr += temp_var;
            s_expr += special;
            s_expr += Integer.toString(temp_exp);
            if (temp_exp == 1){
                str_length = s_expr.length();
                s_expr = s_expr.substring(0,str_length-2);
            }
            if (temp_exp == 0){
                s_expr = "";
            }
            var_temp.remove(0);
            exp_temp.remove(0);
            //System.out.print("---**" + s_expr + "**---");
        }

        if (coef > 1)
            temp = s_coef + s_expr;
        else if (coef == 0)
            temp = "";
        else if (coef == 1) {
            temp = s_expr;
            if (s_expr.isEmpty())
                temp = "1";
        }
        else
            temp =  s_coef + s_expr;
        //System.out.print("---**" + temp + "**---");"-" +



        return temp;
    }

    public Boolean isEqual(Term t2){

        if (var.equals(t2.var)){
            //System.out.print("  ");
            if (exp.equals(t2.exp))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public Term add( Term t2){
        Term t =  new Term(t2.coef + coef, var, exp);
        //System.out.print(t);
        return t;
    }

    public Term sub( Term t2){
        int ans = coef - t2.coef;
        //System.out.println("ans " + ans);
        Term t =  new Term( ans, var, exp);
        return t;
    }

    public Term mult( Term t2){
        ArrayList<Character> var_temp = new ArrayList<>(var);
        ArrayList<Integer> exp_temp = new ArrayList<>(exp);
        ArrayList<Character> t2var_temp = new ArrayList<>(t2.getVar());
        ArrayList<Integer> t2exp_temp = new ArrayList<>(t2.getExp());

        ArrayList<Character> new_var = new ArrayList<>();
        ArrayList<Integer> new_exp = new ArrayList<>();

        ArrayList<Integer> i_index = new ArrayList<>();
        ArrayList<Integer> j_index = new ArrayList<>();

        int new_coef =  coef * t2.coef;
        int t_size =  var.size();
        int t2_size = t2.var.size();

        int temp_exp;

        for (int i = 0; i < t_size; i++){
            for (int j = 0; j < t2_size; j++){
                if (var_temp.get(i).toString().contentEquals(t2var_temp.get(j).toString())){
                    new_var.add(var_temp.get(i));
                    temp_exp = exp_temp.get(i) + t2exp_temp.get(j);
                    new_exp.add(temp_exp);

                    i_index.add(i);
                    j_index.add(j);
                }
            }
        }
        //System.out.println("lol");
        int f = 0;
        for (Integer i : i_index){
            var_temp.remove(i-f);
            exp_temp.remove(i-f);
            f++;
        }
        f = 0;
        for (Integer i : j_index){
            t2var_temp.remove(i-f);
            t2exp_temp.remove(i-f);
            f++;
        }
        if (!var_temp.isEmpty()){
            for (int i = 0; i < var_temp.size(); i++){
                new_var.add(var_temp.get(i));
                new_exp.add(exp_temp.get(i));
            }
        }
        if (!t2var_temp.isEmpty()){
            for (int i = 0; i < t2var_temp.size(); i++){
                new_var.add(t2var_temp.get(i));
                new_exp.add(t2exp_temp.get(i));
            }
        }
        Term t =  new Term(new_coef, new_var, new_exp);
        return t;

    }

    public boolean containElementinPoly (Polynomial t2){
        for (Term t : t2.getTerms()){
            ArrayList<Character> vars  = new ArrayList<>(t.getVar());
            ArrayList<Integer> exps  = new ArrayList<>(t.getExp());

            if (var.equals(vars)){
                if (exp.equals(exps)){
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public boolean containElementinArray (ArrayList<Character> c){
        ArrayList<Character> c_temp = new ArrayList<>(c);

            for (Character c1 : c_temp) {
                if (!var.contains(c1)) {
                    return false;
                }
                c.remove(c1);
            }

        return true;
    }

    public double evaluate ( char[] c,double [] d){
        ArrayList<Double> sum = new ArrayList<>();
        int temp;

        for (int i = 0; i < d.length; i++){
            for (Character c1 : var){
                if (c1.equals(c[i])){
                    temp = var.indexOf(c1);
                    sum.add(Math.pow(d[i],exp.get(temp)));
                }
            }
        }
        //System.out.print(sum);
        double eval = 1;
        for (Double d1: sum){
            eval *= d1;
        }

        return coef * eval;
    }





}
