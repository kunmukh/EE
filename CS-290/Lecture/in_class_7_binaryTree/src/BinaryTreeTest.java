import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

import java.util.ArrayList;
import java.util.Iterator;

public class BinaryTreeTest {

    @Test
    public void binaryTreeTest() {
        BinaryTree<String> tree = new BinaryTree<>();

        tree.add("Don");
        tree.add("Roberts");
        tree.add("Adam");
        tree.add("Zebra");
        assertEquals(4, tree.size());
    }

    @Test
    public void binaryPersonTest() {
        BinaryTree<Manager> tree = new BinaryTree<>();
        tree.add(new Manager("Don"));
        tree.add(new Manager("Roberts"));
        tree.add(new Manager("Blandford"));

        assertEquals(3, tree.size());

        int count = 0;

        for (Manager each : tree) {
            count++;
        }

        assertEquals(3, count);
    }


    @Test
    public void testCollection() {
        ArrayList<String> a = new ArrayList<>();

        a.add("Don");
        a.add("Roberts");
        a.add("Adam");

        assertEquals(14,
                a.stream()
                .mapToInt( e -> e.length())
                .sum());
    }

}
