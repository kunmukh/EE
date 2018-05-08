import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public abstract class Board {
    protected ArrayList<Slot> board = new ArrayList<>();
    protected Map<Integer, Integer> correlator = new HashMap<Integer, Integer>();
    protected Map<String, Integer> correlatorSpecial = new HashMap<String, Integer>();
    protected Colors currentPlayer;
    protected boolean choiceMessage = true;
    protected boolean hasMessage = false;
    protected String message = "";


    public Colors getOppositeplayer(Colors currentPlayer) {
        switch (currentPlayer){
            case RED:
                return Colors.BLUE;
            case BLUE:
                return Colors.RED;
            default:
                return Colors.GREY;
        }
    }
    public void boardMaker (int start, int end, Colors col, int numCol){
        for (int i = start; i <= end; i++){
            Slot s = new Slot(numCol, col);
            board.add(s);
        }
    }
    public void mapMaker (int start, int end , int offset){
        for (int i = start; i <= end; i++){
            correlator.put(i, i+offset);
        }
    }
    public void setCurrentPlayer(Colors currentPlayer) {
        this.currentPlayer = currentPlayer;
    }
    private void printMessagelogic(int choice){
        if (choiceMessage){
            switch (choice){
                case 1:
                    makeMessage("The starting slot is occupied by your opponent.");
                    break;
                case 2:
                    makeMessage("Wrong number of jumps.");
                    break;
                case 3:
                    makeMessage("The final slot is occupied by the opponent color.");
                    break;
                case 4:
                    makeMessage("You cannot move the coins into the base, \nunless all the coins are in the homeboard");
                    break;
                case 5:
                    makeMessage("You have coins in the bear off slot.");
                    break;
            }
        }

    }
    public void setMessagefalse(){
        hasMessage = false;
        message = "";
    }
    private void makeMessage(String text){
        this.hasMessage = true;
        this.message = " \n " + text;
    }
    public boolean canMove(int start, int end){
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
            else if (board.get(correlator.get(start)).getColor().equals(getOppositeplayer(currentPlayer))) {
                printMessagelogic(1);
            }
            return false;
        }
    }
    private boolean isHomeready(Colors color){
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
    public boolean isMessage() {
        return hasMessage;
    }
    public String getMessage() {
        return message;
    }
    public Colors getCurrentPlayer() {
        return currentPlayer;
    }

    abstract void move(int start, int end);
    abstract boolean isWin(Colors color);
}