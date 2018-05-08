import com.sun.xml.internal.messaging.saaj.util.ByteInputStream;
import org.junit.jupiter.api.Test;
import java.io.ByteArrayInputStream;
import java.io.PushbackInputStream;
import java.util.*;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class CollectionTests {
    @Test
    public void testArrays(){
        int arr[] = new int[10];
        for (int i = 0; i < 10; i++){
            arr[i] = i;
        }
        for (int i = 0; i < arr.length; i++){
            //System.out.println(arr[i]);
        }
        int sum = 0;
        for (int each: arr){
            sum += each;
        }
        assertEquals(45, sum);

        int arr2[];
        arr2 = Arrays.copyOf(arr, arr.length);
        assertTrue(Arrays.equals(arr, arr2));
        arr[3] = 666;
        assertEquals(666, arr[3]);
        assertEquals(3, arr2[3]);
    }

    @Test
    public void testString(){
        String s = "Fo";
        s = s + "o";
        assertTrue (s.equals("Foo"));
        PushbackInputStream str  = new PushbackInputStream(
                new ByteArrayInputStream(s.getBytes()));

        StringBuilder result  = new StringBuilder();
        for (int i= 0; i <10; i++){
            result.append(i)
                  .append(" ");
            System.out.println(result);
        }
        String answer = result.toString();
        System.out.println("The Final answer :" + result);
    }

    @Test
    void testCollections(){
        //track to see if I have something or not
        Set<String> names = new TreeSet<>();
        names.add("Don");
        names.add("Kunal");
        names.add("Dough");
        names.add("Sue");
        assertEquals(4, names.size());
        names.add("Don");

        assertEquals(4, names.size());
        assertTrue(names.contains("Don"));
        assertFalse(names.contains("Densie"));

        for (String each : names){
            System.out.println(each);
        }
    }
    @Test
    void testCollectionshash(){
        //track to see if I have something or not
        Set<String> names = new HashSet<>();
        names.add("Don");
        names.add("Kunal");
        names.add("Dough");
        names.add("Sue");
        assertEquals(4, names.size());
        names.add("Don");

        assertEquals(4, names.size());
        assertTrue(names.contains("Don"));
        assertFalse(names.contains("Densie"));

        for (String each : names){
            System.out.println(each);
        }
    }

    @Test
    void testPrimitiveCollections(){
        Set<Integer> s = new HashSet<>();
        //Integer i = new Integer(5);
        //s.add(i);
        s.add(50);
        for (int j : s){
            int x = j;
        }
    }

    @Test
    void testSequences(){
        Vector<Integer> v =  new Vector<>();
        ArrayList<Integer> l = new ArrayList<>();

        for (int i = 0; i < 10; i++){
            v.add(i);
        }
        assertEquals(10, v.size());
        assertTrue(v.contains(5));

        for (int i = 0; i < v.size(); i++){
            System.out.println(v.elementAt(i));
        }

        Vector<Character> vc = new Vector<>();
        vc.add('d');
        vc.add('o');
        vc.add('n');

        vc.remove(new Character('o'));
        assertEquals(2, vc.size());

        vc.set(0,'D');
    }

    @Test
    public void testMap(){
        Map<Character, Double> si = new HashMap<>();

        si.put('c', .001);
        si.put('K', 1000.0);
        si.put('m', .001);

        assertEquals(.001, si.get('m').doubleValue());

        for (double d: si.values()) {

        }
    }

    @Test
    public void testIterator(){
        Vector<String> v = new Vector<>();
        v.add("Foo");
        v.add("Bar");
        v.add("Baz");
        Iterator<String> it = v.iterator();
        Iterator<String> it2 = v.iterator();
        while (it.hasNext() && it2.hasNext()){
            String s = it.next();
            System.out.println(s);
        }
    }

}
