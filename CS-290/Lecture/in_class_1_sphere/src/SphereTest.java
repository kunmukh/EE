import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class SphereTest {
    @Test
    public void testConstructor(){
        Sphere s = new Sphere();
        checkSphere(s, 1.0, 0.0, 0.0, 0.0);

        Sphere s2 = new Sphere(5.0);
        checkSphere(s2, 5.0, 0.0, 0.0, 0.0);

        Sphere s3 = new Sphere (1.0, 2.0, 3.0);
        checkSphere(s3, 1.0, 1.0, 2.0, 3.0);

        Sphere s4 = new Sphere (3.0, -1.0, -2.0, -3.0);
        checkSphere(s4,3.0, -1.0, -2.0, -3.0);

        Sphere point = new Sphere (0.0);
        checkSphere(s4,0.0, 0.0, 0.0, 0.0);
    }

    @Test
    public void testBadSphere(){
        assertThrows(RuntimeException.class, () -> {
            Sphere bad = new Sphere (-1.0);
        });

    }

    @Test
    public void testIntersection(){
        Sphere s1 = new Sphere (2.0);
        Sphere s2 = new Sphere (1.0,2.5,2,5);
        Sphere s3 = new Sphere (1.0, 2, 2 , 2);

        assertTrue(s1.intersect(s3));
        assertFalse(s1.intersect(s2));
    }

    private void checkSphere (Sphere s, double radius, double x, double y, double z){
        assertEquals(radius, s.getRadius());
        assertEquals(x, s.getX());
        assertEquals(y, s.getY());
        assertEquals(z, s.getZ());
    }
}
