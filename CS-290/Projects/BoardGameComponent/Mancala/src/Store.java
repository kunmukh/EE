
public class Store {
    private int max = 50;
    private char holder_1[] = new char[max];
    private int col = 25;

    Store(int total_1){

        Piece p = new Piece();
        for (int i = 0; i < max; i++){
            if (total_1 > 0) {
                holder_1[i] = p.toString().charAt(0);
                total_1--;
            }
            else {
                holder_1[i] = ' ';
            }
        }
    }

    @Override
    public String toString() {
        String result = "";
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < max; i = i + col){
            builder.append(result).append(linePrint(i)).append("\n");
        }
        result += "-------------------------------------------------";
        return builder.toString();
    }

    private String linePrint(int i){
        String result = "";
        String format = "     ";
        StringBuilder builder = new StringBuilder();
        for (int j = 0; j < col; j++){
            builder.append(result ).append(holder_1[i + j]);
        }
        return format + builder.toString();
    }
}
