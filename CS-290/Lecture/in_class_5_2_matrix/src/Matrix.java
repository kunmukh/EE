public class Matrix {
    MatrixRepresentation rep;

    public Matrix(int rows, int cols) {
        rep = new TwoDArray(rows, cols);
    }

    public Matrix multiply(Matrix m) {
        if (this.getCols() != m.getRows())
            throw new ArithmeticException("Bad Shape");

        Matrix result = new Matrix(this.getRows(), m.getCols());
        for(int r=0; r<result.getRows(); r++) {
            for (int c=0; c<result.getCols(); c++) {
                double entry = 0.0;
                for (int i=0; i<this.getCols(); i++) {
                    entry += this.get(r, i) * m.get(i, c);
                }
                result.set(r,c, entry);
            }
        }
        return result;
    }

    public void set(int r, int c, double entry) {
        rep.set(r, c, entry);
    }

    public double get(int row, int col) {
        return rep.get(row, col);
    }

    public int getCols() {
        return rep.getCols();
    }

    public int getRows() {
        return rep.getRows();
    }
}
