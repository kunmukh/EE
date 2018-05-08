public class TwoDArray implements MatrixRepresentation {
    protected double[] data;
    protected int rows;
    protected int cols;

    public TwoDArray(int rows, int cols) {
        this.rows = rows;
        this.cols = cols;
        data = new double[rows*cols];
    }

    public double get(int r, int c) {
        if (r < 0 || r >= rows || c < 0 || c >= cols)
            throw new IndexOutOfBoundsException();

        return data[r * cols + c];
    }

    public void set(int r, int c, double entry) {
        if (r < 0 || r >= rows || c < 0 || c >= cols)
            throw new IndexOutOfBoundsException();

        data[r * cols + c] = entry;
    }

    public int getRows() {
        return rows;
    }

    public int getCols() {
        return cols;
    }

}
