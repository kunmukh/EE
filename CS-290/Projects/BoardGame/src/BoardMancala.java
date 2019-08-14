import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class BoardMancala extends Board {
    private boolean extraTurn = false;
    private Map<Integer, Integer> oppositePair = new HashMap<Integer, Integer>();
    private Map<Integer, Integer> inputPairP1 = new HashMap<Integer, Integer>();
    private Map<Integer, Integer> inputPairP2 = new HashMap<Integer, Integer>();
    private Map<Colors, Integer> special = new HashMap<Colors, Integer>();

    public BoardMancala(){

        mapMaker(1,6, 0);
        mapMaker(7,12, 2);
        correlator.put(13,0);
        correlator.put(0,0);


        special.put(Colors.BLUE,0);
        special.put(Colors.RED,7);

        oppositePairmaker();
        inputPairmaker();

        boardMaker(0,0,Colors.RED,0);
        boardMaker(1,6,Colors.RED,4);
        boardMaker(7,7,Colors.RED,0);
        boardMaker(8,8,Colors.GREY,0);
        boardMaker(9,14,Colors.RED,4);
        boardMaker(15,15,Colors.GREY,0);
        currentPlayer = Colors.RED;

    }
    public int numColslot(int start){
        return board.get(start).getNumColor();
    }
    public boolean isExtraturn() {
        return extraTurn;
    }
    public void draw(){
        System.out.println("Score: RED :" + board.get(special.get(Colors.RED)).getNumColor()
                +  " BLUE : " + board.get(special.get(Colors.BLUE)).getNumColor());
        ArrayList<String> slotString = new ArrayList<>();

        for (int i = 0; i <= 15; i++){
            slotString.add(board.get(i).toString().replace("*", " "));
        }

        StringBuilder num = new StringBuilder();

        num.append("| ").append("    ").append("   |");
        for (int i = 12; i >= 7; i--){
            if (i < 10)
                num.append("|    ").append(i).append("   |");
            else
                num.append("|   ").append(i).append("   |");
        }
        num.append("|  ").append("RED").append("   |");


        StringBuilder lineHigh = new StringBuilder();
        StringBuilder lineLow = new StringBuilder();

        for (int i = 0; i < num.length(); i++) {
            lineHigh.append("-");
            lineLow.append("_");
        }

        System.out.println(lineLow);
        System.out.println(num);
        System.out.println("|        |------------------------------------------------------------|        |");

        StringBuilder slot = new StringBuilder();
        String slotTemp;

        for (int j = 0; j < 5; j++) {
            for (int i = 15; i >= 8; i--) {
                slotTemp = slotString.get(i).substring(5*j, (5*(j+1)));
                slot.append("|  ").append(slotTemp).append(" |");
            }
            slot.append("\n");
        }
        System.out.println(slot);
        System.out.println("|        |------------------------------------------------------------|        |");

        slot = new StringBuilder();
        num = new StringBuilder();

        for (int i = 0; i <= 7; i++){
            slotString.add(board.get(i).toString());
        }

        for (int j = 0; j < 5; j++) {
            for (int i = 0; i <= 7; i++) {
                slotTemp = slotString.get(i).substring(5*j, (5*(j+1)));
                slot.append("|  ").append(slotTemp).append(" |");
            }
            slot.append("\n");
        }
        System.out.println(slot);
        System.out.println("|        |------------------------------------------------------------|        |");


        for (int i = 0; i <= 7; i++){
            if (i == 0)
                num.append("|   BLUE").append(" |");
            else if (i == 7)
                num.append("|   ").append("     |");
            else {
                num.append("|   ").append(i).append("    |");
            }
        }

        System.out.println(num);
        System.out.println(lineLow);
    }
    public void move(int start, int end){
        extraTurn = false;
        int last = 0, numStones = board.get(correlator.get(start)).getNumColor();
        int add = 0;
        board.get(correlator.get(start)).setNumColor(0);

        for (int i = 1; i <= numStones; i++){
            if(board.get(correlator.get(((start + i) % 13))).getNumColor() == 0){
                board.get(getInput((start + i) % 13)).increNumcolor();
               add = board.get(special.get(currentPlayer)).getNumColor()
                       + board.get(oppositePair.get(((start + i) % 13))).getNumColor();
               board.get(special.get(currentPlayer)).setNumColor(add);
               if((((start + i) % 13) != 0) || (((start + i) % 13) != 7)) {
                   board.get(oppositePair.get(((start + i) % 13))).setNumColor(0);
               }
            }else{
                board.get(getInput((start + i) % 13)).increNumcolor();
            }
            last = getInput((start + i) % 13);
        }

        if (currentPlayer.equals(Colors.RED) &&
                special.get(Colors.RED).equals(last)) {
            System.out.println("You hav extra turn as your last stone is in your store");
            extraTurn = true;
        } else if (currentPlayer.equals(Colors.BLUE) &&
                special.get(Colors.BLUE).equals(last)) {
            System.out.println("You hav extra turn as your last stone is in your store");
            extraTurn = true;
        }

    }
    public boolean isWin(Colors color) {
        return board.get(special.get(color)).getNumColor() >
                board.get(special.get(getOppositeplayer(color))).getNumColor();
    }
    public boolean isHomeready(Colors color){
        switch (color){
            case RED:
                for (int i = 1; i <= 6; i++){
                    if (board.get(correlator.get(i)).getNumColor() != 0){
                        return false;
                    }
                }
                return true;
            case BLUE:
                for (int i = 7; i <= 12; i++){
                    if (board.get(correlator.get(i)).getNumColor() != 0){
                        return false;
                    }
                }
                return true;
            default:
                return false;
        }
    }
    public void finalGameboard(){
        int tempSum;
        for (int i = 1; i <= 12; i++){
            tempSum = 0;
            if(i < 7){
                tempSum = board.get(correlator.get(i)).getNumColor() + board.get(special.get(Colors.RED)).getNumColor();
                board.get(correlator.get(i)).setNumColor(0);
                board.get(special.get(Colors.RED)).setNumColor(tempSum);
            }else{
                tempSum = board.get(correlator.get(i)).getNumColor() + board.get(special.get(Colors.BLUE)).getNumColor();
                board.get(correlator.get(i)).setNumColor(0);
                board.get(special.get(Colors.BLUE)).setNumColor(tempSum);
            }
        }
    }
    public int getScore(Colors color){
        return board.get(special.get(color)).getNumColor();
    }
    private void oppositePairmaker(){
        oppositePair.put(0,8);
        for (int i = 1; i <= 6; i++)
            oppositePair.put(i,correlator.get(13 - i));
        for (int i = 1; i <= 6; i++)
            oppositePair.put(i + 6,correlator.get(7 - i));
    }
    private void inputPairmaker(){
        for (int i = 0; i <= 7; i++){
            if (i == 0) {
                inputPairP1.put(i, special.get(Colors.BLUE));
                inputPairP2.put(i, special.get(Colors.BLUE));
            }else if (i == 7) {
                inputPairP1.put(i, special.get(Colors.RED));
                inputPairP2.put(i, special.get(Colors.RED));
            }
            else {
                inputPairP1.put(i, i);
                inputPairP2.put(i, i);
            }
        }
        for (int i = 8; i <= 14; i++){
            inputPairP1.put(i, correlator.get((i-1) % 14));
            inputPairP2.put(i, correlator.get(i % 13));
        }

    }
    private int getInput(int original){
        switch (currentPlayer){
            case RED:
                return inputPairP1.get(original);
            case BLUE:
                return inputPairP2.get(original);
            default:
                return  -2;
        }
    }
    public boolean isSlotEmpty(int point){
        return board.get(correlator.get(point)).getNumColor() == 0;
    }
}
