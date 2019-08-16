import java.util.HashSet;
import java.util.Set;
import java.util.Vector;

public class Application {
    public static void main(String args[]){
        Vector<Set<Integer>> v = new Vector<>();

        v.add(new HashSet<>()); //0th element
        HashSet<Integer> one = new HashSet<>();
        one.add(9); // 1st Element;
        v.add(one);

        for (int i = 2; i <= 9; i++) {
            HashSet<Integer> newSet = new HashSet<>();
            for (int j = i - 1; j >= 1; j--) {
                Set<Integer> s1 = v.get(j);
                Set<Integer> s2 = v.get(i - j);

                for (int a : s1) {
                    for (int b : s2) {
                        newSet.add(a + b);
                        newSet.add(a - b);
                        newSet.add(a * b);
                        if (b != 0) {
                            newSet.add(a / b);
                        }
                    }
                }
            }
            //System.out.println(newSet);
            System.out.println(i + " Nine has"  + newSet.size() + " element");
            v.add(newSet);
        }
        Set<Integer> nineNines = v.get(9);
        int smallest = 0;
        while (nineNines.contains(smallest)){
            smallest++;
        }
        System.out.println("The smallest number you cant make is " + smallest);
    }
}
