import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class MatrixTest {
    @Test
    public void testMultiply() {
        Matrix m1 = new Matrix(2,2);
        m1.set(0,0, 1.0);
        m1.set(0,1, 2.0);
        m1.set(1,0, 3.0);
        m1.set(1,1, 4.0);

        Matrix m2 = new Matrix(2,2);
        m2.set(0,0, -1.0);
        m2.set(0,1, 2.0);
        m2.set(1,0, -3.0);
        m2.set(1,1, 4.0);

        Matrix r = m1.multiply(m2);
        assertEquals(-7, r.get(0,0));
        assertEquals(10, r.get(0,1));
        assertEquals(-15, r.get(1,0));
        assertEquals(22, r.get(1,1));
    }


}
