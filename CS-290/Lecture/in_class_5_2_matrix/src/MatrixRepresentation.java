public interface MatrixRepresentation {
    int getRows();
    int getCols();
    double get(int row, int col);
    void set(int row, int col, double value);
}
