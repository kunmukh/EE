import org.omg.Messaging.SYNC_WITH_TRANSPORT;

import java.awt.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class Board {
    private ArrayList<Slot> board = new ArrayList<>();
    private Map<Integer, Integer> correlator = new HashMap<Integer, Integer>();
    private Map<String, Integer> correlatorSpecial = new HashMap<String, Integer>();
    private Colors currentPlayer;
    private boolean choiceMessage = true;

    public Board(){
        correlatorSpecial.put("R", 0);
        correlatorSpecial.put("B", 27);
        correlatorSpecial.put("RB", 7);
        correlatorSpecial.put("BB", 20);


        mapMaker(1,6, 0);
        mapMaker(7,18, 1);
        mapMaker(19,24, 2);
        correlator.put(0,0);
        correlator.put(25,7);
        correlator.put(26,20);
        correlator.put(27,27);

        boardMaker(0,0,Colors.RED,0);
        boardMaker(1,1,Colors.GREY,0);
        boardMaker(2,5,Colors.GREY,0);
        boardMaker(6,6,Colors.RED,5);
        boardMaker(7,7,Colors.RED,0);
        boardMaker(8,8,Colors.GREY,0);
        boardMaker(9,9,Colors.RED,3);
        boardMaker(10,12,Colors.GREY,0);
        boardMaker(13,13,Colors.GREY,0);
        boardMaker(14,14,Colors.RED,5);
        boardMaker(15,17,Colors.GREY,0);
        boardMaker(18,18,Colors.GREY,0);
        boardMaker(19,19,Colors.GREY,0);
        boardMaker(20,20,Colors.BLUE,0);
        boardMaker(21,21,Colors.BLUE,5);
        boardMaker(22,25,Colors.GREY,0);
        boardMaker(26,26,Colors.RED,2);
        boardMaker(27,27,Colors.BLUE,14);

        currentPlayer = Colors.RED;

    }

    public Colors getCurrentPlayer() {
        return currentPlayer;
    }

    public void setCurrentPlayer(Colors currentPlayer) {
        this.currentPlayer = currentPlayer;
    }

    public boolean isWin(Colors color){
        String baseName;
        if (color == Colors.RED)
            baseName = "R";
        else
            baseName = "B";

        return board.get(correlatorSpecial.get(baseName)).getNumColor() == 15;
    }

    public void draw() {
        System.out.println("Score: RED :" + board.get(correlatorSpecial.get("R")).getNumColor()
                +  " BLUE : " + board.get(correlatorSpecial.get("B")).getNumColor());
        ArrayList<String> slotString = new ArrayList<>();

        for (int i = 13; i >= 0; i--){
            slotString.add(board.get(i).toString());
        }

        StringBuilder num = new StringBuilder();

        for (int i = 12; i >= 7; i--){
            if (i < 10 && i != 0)
                num.append("|    ").append(i).append("   |");
            else
                num.append("|   ").append(i).append("   |");
        }
        num.append("|  ").append("RB").append("   |");
        for (int i = 6; i >= 0; i--){
            if (i == 0)
                num.append("|    ").append("R").append("   |");
            else
                num.append("|   ").append(i).append("    |");
        }

        StringBuilder lineHigh = new StringBuilder();
        StringBuilder lineLow = new StringBuilder();

        for (int i = 0; i < num.length(); i++) {
            lineHigh.append("-");
            lineLow.append("_");
        }

        System.out.println(lineLow);
        System.out.println(num);
        System.out.println(lineHigh);

        StringBuilder slot = new StringBuilder();
        String slotTemp;

        for (int j = 0; j < 3; j++) {
            for (int i = 0; i <= 13; i++) {
                slotTemp = slotString.get(i).substring(5*j, (5*(j+1)));
                slot.append("|  ").append(slotTemp).append(" |");
            }
            slot.append("\n");
        }
        System.out.println(slot);
        System.out.println(lineHigh);

        slot = new StringBuilder();
        num = new StringBuilder();

        for (int i = 14; i <= 27; i++){
            slotString.add(board.get(i).toString());
        }

        for (int j = 0; j < 3; j++) {
            for (int i = 14; i <= 27; i++) {
                slotTemp = slotString.get(i).substring(5*j, (5*(j+1)));
                slot.append("|  ").append(slotTemp).append(" |");
            }
            slot.append("\n");
        }
        System.out.println(slot);
        System.out.println(lineHigh);


        for (int i = 13; i <= 26; i++){
            if (i == 19)
                num.append("|  ").append("BB").append("   |");
            else if (i == 26)
                num.append("|    ").append("B").append("   |");
            else {
                if (i >= 20)
                    num.append("|   ").append(i-1).append("   |");
                else
                    num.append("|   ").append(i).append("   |");
            }
        }

        System.out.println(num);
        System.out.println(lineLow);



    }

    private void boardMaker (int start, int end, Colors col, int numCol){
        for (int i = start; i <= end; i++){
            Slot s = new Slot(numCol, col);
            board.add(s);
        }
    }

    private void mapMaker (int start, int end , int offset){
        for (int i = start; i <= end; i++){
            correlator.put(i, i+offset);
        }
    }

    private Colors getOppositeplayer(Colors currentPlayer) {
        switch (currentPlayer){
            case RED:
                return Colors.BLUE;
            case BLUE:
                return Colors.RED;
            default:
                return Colors.GREY;
        }
    }

    public boolean isHomeready(Colors color){
        int startNonbase, endNonbase;
        if (color == Colors.RED){
            startNonbase = 7;
            endNonbase = 24;
        }
        else if (color == Colors.BLUE){
            startNonbase = 1;
            endNonbase = 18;
        }
        else{
            startNonbase = 0;
            endNonbase = 0;
        }
        return emptySlotcounter(startNonbase, endNonbase);
    }

    private boolean emptySlotcounter(int start, int end){
        for (int i = start; i <= end; i++) {
            if (board.get(correlator.get(i)).getColor() == currentPlayer) {
                if (board.get(correlator.get(i)).getNumColor() != 0) {
                    return false;
                }
            }
        }
        return true;
    }

    public Boolean canMove(int start, int end){
        switch (currentPlayer){
            case RED:
                return canMovelogic(Colors.RED, start, end);
            case BLUE:
                return canMovelogic(Colors.BLUE, start, end);
            default:
                return false;
        }
    }

    private boolean canMovelogic(Colors color, int start, int end) {
        String baseColor = "";
        String bearOffcolor = "";
        Colors playerColor = Colors.GREY;
        Colors opponentColor = Colors.GREY;

        if (color == Colors.RED){
            baseColor = "R";
            bearOffcolor = "RB";
            playerColor = Colors.RED;
            opponentColor = Colors.BLUE;
        }
        else if (color == Colors.BLUE){
            baseColor = "B";
            bearOffcolor = "BB";
            playerColor = Colors.BLUE;
            opponentColor = Colors.RED;
        }

        if (board.get(correlator.get(start)).getColor() == playerColor) {
            if (end != correlatorSpecial.get(baseColor)) {
                if (board.get(correlatorSpecial.get(bearOffcolor)).getNumColor() == 0) {
                    if (board.get(correlator.get(end)).getColor() != opponentColor) {
                        return true;
                    } else {
                        if (board.get(correlator.get(end)).getNumColor() < 2) {
                            return true;
                        }
                        else {
                            printMessagelogic(3);
                            return false;
                        }
                    }
                } else {
                    if ((board.get(correlator.get(start)).getColor() ==
                            board.get(correlatorSpecial.get(bearOffcolor)).getColor()) &&
                            ((correlator.get(start).equals(correlatorSpecial.get(bearOffcolor)))) &&
                            ((board.get(correlator.get(end)).getNumColor() < 2) ||
                                    board.get(correlator.get(end)).getColor() != opponentColor))
                        return true;
                    else if ((board.get(correlator.get(start)).getColor() ==
                            board.get(correlatorSpecial.get(bearOffcolor)).getColor()) &&
                            ((correlator.get(start).equals(correlatorSpecial.get(bearOffcolor))))) {
                        printMessagelogic(3);
                        return false;
                    }
                    else {
                        printMessagelogic(5);
                        return false;
                    }
                }
            } else {
                if (isHomeready(playerColor) &&
                        (board.get(correlatorSpecial.get(bearOffcolor)).getNumColor() == 0)) {
                    return true;
                } else {
                    printMessagelogic(4);
                    return false;
                }
            }
        }else{
            if (board.get(correlator.get(start)).getColor() == Colors.GREY) {
                printMessagelogic(2);
            }
            else if (board.get(correlator.get(start)).getColor() == getOppositeplayer(currentPlayer)) {
                printMessagelogic(1);
            }
            return false;
        }
    }

    public void printMessagelogic(int choice){
        if (choiceMessage){
            switch (choice){
                case 1:
                    System.out.println("The starting slot is occupied by your opponent.");
                    break;
                case 2:
                    System.out.println("The starting slot is empty.");
                    break;
                case 3:
                    System.out.println("The final slot is occupied by the opponent color.");
                    break;
                case 4:
                    System.out.println("You cannot move the coins into the base, unless all the coins are in the homeboard");
                    break;
                case 5:
                    System.out.println("You have coins in the bear off slot.");
                    break;
            }
        }

    }

    public boolean canMovewithoutMessage(int start, int end){
        boolean answer;
        choiceMessage = false;
        answer = canMove(start,end);
        choiceMessage = true;
        return answer;

    }

    public void move(int start, int end){

        String bearOffoppoColor = "";
        Colors playerColor = Colors.GREY;
        Colors opponentColor = Colors.GREY;


        if (currentPlayer == Colors.RED){
            bearOffoppoColor = "BB";
            playerColor = Colors.RED;
            opponentColor = Colors.BLUE;
        }
        else if (currentPlayer == Colors.BLUE){
            bearOffoppoColor = "RB";
            playerColor = Colors.BLUE;
            opponentColor = Colors.RED;
        }

        if (board.get(correlator.get(end)).getColor() != opponentColor){
            board.get(correlator.get(start)).decrNumcolor();
            board.get(correlator.get(end)).increNumcolor();
            board.get(correlator.get(end)).setColor(playerColor);
        }else{
            board.get(correlator.get(start)).decrNumcolor();
            board.get(correlator.get(end)).setColor(playerColor);
            board.get(correlator.get(end)).setNumColor(1);
            board.get(correlatorSpecial.get(bearOffoppoColor)).increNumcolor();
        }
    }


}
