public class IdentityRepresentation implements MatrixRepresentation {
    protected int size;

    public IdentityRepresentation(int size) {
        this.size = size;
    }

    @Override
    public int getRows() {
        return size;
    }

    @Override
    public int getCols() {
        return size;
    }

    @Override
    public double get(int row, int col) {
        if (row == col)
            return 1.0;
        else
            return 0.0;
    }

    @Override
    public void set(int row, int col, double value) {
        throw new ArithmeticException("Cannot alter identity matrix");
    }
}
