import java.sql.BatchUpdateException;

public class Hole {
    private int max = 25;
    private char holder_1[] = new char[max];
    private char holder_2[] = new char[max];
    private String name1;
    private String name2;
    private int col = 5;

    Hole(int total_1, int total_2, String name_1, String name_2){

        name1 = name_1;
        name2 = name_2;

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

        for (int i = 0; i < max; i++){
            if (total_2 > 0) {
                holder_2[i] = p.toString().charAt(0);
                total_2--;
            }
            else {
                holder_2[i] = ' ';
            }
        }
    }

    @Override
    public String toString() {
        boolean first = true;
        String result = "";

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < max; i = i + 5){
            String format = "          ⧭               ";

            if (first) {
                builder.append("| ").append(name1).append(" | ").append(linePrint(1,i )).append(format).append(linePrint(2,i)).append("| ").append(name2).append(" |").append("\n");
                first = false;
            }
            else{
                builder.append("      ").append(linePrint(1,i)).append(format).append(linePrint(2,i)).append("\n");
            }
        }
        result = builder.toString();
        result += "-------------------------------------------------";
        return result;
    }

    private String linePrint(int select, int i){
        String result = "";
        StringBuilder builder = new StringBuilder();
        for (int j = 0; j < col; j++){
            if (select == 1)
                builder.append(result).append(holder_1[i + j]);
            else
                builder.append(result).append(holder_2[i + j]);
        }
        return builder.toString();
    }
}
