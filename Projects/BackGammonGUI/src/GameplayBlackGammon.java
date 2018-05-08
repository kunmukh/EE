import java.util.ArrayList;
import java.util.Random;
import java.util.Scanner;

public class GameplayBlackGammon{
    private Colors currentPlayer;
    private BoardBlackGammon boardBlackGammon;
    private ArrayList<Integer> dice = new ArrayList<>();
    private ArrayList<Integer> input = new ArrayList<>();
    private boolean skip, found, playerInput, bearOff, skipBearOff, hasMessage, noMovesAvailable = false ;
    private String message = "";

    public GameplayBlackGammon() {
        boardBlackGammon = new BoardBlackGammon();

        skip = false;
        found = false;
        playerInput = false;
        bearOff = false;
        skipBearOff = false;
        currentPlayer = boardBlackGammon.getCurrentPlayer();

        boolean start = false;
        while(!start){
            rollDice();
            makeMessage(Colors.RED + " has rolled: " + dice.get(0));
            makeMessage(Colors.BLUE + " has rolled: " + dice.get(1));

            if(dice.get(0) > dice.get(1)){
                makeMessage("RED has higher number so, it is Red's turn");
                boardBlackGammon.setCurrentPlayer(Colors.RED);
                start= true;
            }
            else if(dice.get(0) < dice.get(1)){
                makeMessage("BLUE has higher number so, it is Blue's turn");
                boardBlackGammon.setCurrentPlayer(Colors.BLUE);
                start= true;
            }
            else{
                makeMessage("Both have rolled an equal equal number.\n So, another dice Roll.");
            }
            rollDice();
        }
        currentPlayer = boardBlackGammon.getCurrentPlayer();
    }

    public void run(String userInput) {

        int start, end;
        skip = false;
        getInputLogic(userInput);


        if (!skip) {
            start = input.get(0);
            input.remove(0);
            end = input.get(0);
            input.remove(0);

            if (!boardBlackGammon.isWin(Colors.RED) && !boardBlackGammon.isWin(Colors.BLUE)) {
                boardBlackGammon.move(start, end);
                input.clear();
            }
        }

    }

    private void getInputLogic(String inputUser) {

        GetInput getInputinitital = new GetInput().invoke(inputUser);

        int startNum = getInputinitital.getStartNum();
        int endNum = getInputinitital.getEndNum();
        int result = getInputinitital.getResult();

        if(!skip) {
            if (bearOff) {
                skipBearOff = true;
                while (!found && bearOff && !skip) {
                    for (Integer i : dice) {
                        if (((i.equals(endNum)) && currentPlayer.equals(Colors.BLUE)) ||
                                ((endNum == (25 - i)) && currentPlayer.equals(Colors.RED))) {
                            if (boardBlackGammon.canMove(startNum, endNum)) {
                                input.clear();
                                input.add(startNum);
                                input.add(endNum);
                                dice.remove(i);
                                numDicesLeft();
                                found = true;
                            }
                            break;
                        }
                    }
                    if (!found) {
                        canMoveMessage(endNum, startNum, false, false);
                    }
                }
            }


            if (!skipBearOff && !skip) {
                while (!playerInput && !skipBearOff && !skip) {

                    while (!playerInput) {
                            if (currentPlayer == Colors.RED) {
                                if (startNum > endNum) {
                                    playerInput = true;
                                } else {
                                    makeMessage("Wrong Direction.");
                                }
                            } else {
                                if (startNum == 26 || startNum < endNum) {
                                    playerInput = true;
                                } else {
                                    makeMessage("Wrong Direction.");
                                }
                            }
                        if (!playerInput) {
                            canMoveMessage(endNum, startNum, true, true);
                        }
                    }

                    while (!found) {
                        playerInput = true;
                        for (Integer i : dice) {
                            if (((startNum >= 0 && startNum <= 27) &&
                                    (endNum >= 0 && endNum <= 27))) {
                                if ((endNum == 0 || endNum == 27)) {
                                    if (endNum == 27)
                                        result -= 2;
                                    if (result <= i) {
                                        if (boardBlackGammon.canMove(startNum, endNum)) {
                                            InputSetter(startNum, endNum, i);
                                        }
                                        break;
                                    }
                                } else if ((i.equals(result))) {
                                    if (boardBlackGammon.canMove(startNum, endNum)) {
                                        InputSetter(startNum, endNum, i);
                                        break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (!found) {
                            canMoveMessage(endNum, startNum, true, false);

                        }
                    }
                }
            }
        }

    }

    private void canMoveMessage(int endNum, int startNum, boolean b, boolean c) {
        boardBlackGammon.canMove(startNum, endNum);
        if (boardBlackGammon.isMessage()) {
            makeMessage(boardBlackGammon.getMessage());
            boardBlackGammon.setMessagefalse();
        }
        makeMessage("Incorrect Input");
        skip = true;
        found = b;
        playerInput = c;
    }

    private void InputSetter(int startNum, int endNum, Integer i) {
        input.add(startNum);
        input.add(endNum);
        dice.remove(i);
        numDicesLeft();
        found = true;

    }

    private class GetInput {

        private int startNum = 2;
        private int endNum = 2;
        private int result = 0;

        public int getResult() {
            return result;
        }

        public int getStartNum() {
            return startNum;
        }

        public int getEndNum() {
            return endNum;
        }

        public GetInput invoke(String inputUser) {
            skip = false;
            noMovesAvailable = false;
            found = false;
            playerInput = false;
            bearOff = false;
            skipBearOff = false;
            input.clear();

            String endInputTemp, startInputTemp;
            if (!skip) {
                Scanner in = new Scanner(inputUser).useDelimiter("-");
                startInputTemp = in.next();

                try {
                    startNum = Integer.valueOf(startInputTemp);
                } catch (NumberFormatException e) {
                    if (startNum == 25) {
                        bearOff = true;
                        skipBearOff = true;
                    } else if (startNum == 26) {
                        bearOff = true;
                        skipBearOff = true;
                    }
                }
                if (!skip) {
                    endInputTemp = in.next();
                    endNum = Integer.valueOf(endInputTemp);
                }
            }

            if (startNum != 26) {
                result = endNum - startNum;
            }else{
                result = endNum;
            }
            if (result < 0)
                result *= -1;
            return this;
        }
    }

    public void rollDice() {
        dice.clear();
        Random rand = new Random();
        int diceOne;
        int diceTwo;

        diceOne = rand.nextInt(6) + 1;
        diceTwo = rand.nextInt(6) + 1;

        if (diceOne == diceTwo) {
            dice.add(diceOne);
            dice.add(diceOne);
            dice.add(diceTwo);
            dice.add(diceTwo);
        } else {
            dice.add(diceOne);
            dice.add(diceTwo);
        }

        StringBuilder output = new StringBuilder();
        output.append("The number on the dice are: ");
        for (int i = 0; i < dice.size(); i++) {
            output.append(" ").append(dice.get(i)).append(" ");
        }

    }

    private void numDicesLeft() {
        StringBuilder output = new StringBuilder();
        output.append("The number on the dice are: ");
        for (int i = 0; i < dice.size(); i++) {
            output.append(" " + dice.get(i) + " ");
        }

    }

    public void setMessagefalse(){
        hasMessage = false;
        message = "";
    }

    private void makeMessage(String text){
        hasMessage = true;
        message = message + " \n " + text;
    }

    public void setNoMovesAvailable(){

        noMovesAvailable = true;
        skip = false;
        ArrayList<Integer> numDice = new ArrayList<>(dice);
        boolean canMove = false;
        int numD = -99;
        if (!numDice.isEmpty()) {
            numD = numDice.get(0);
        }

        while(!canMove) {

            if (currentPlayer.equals(Colors.BLUE)) {
                for (int i = 0; i <= 27; i++) {
                    if (boardBlackGammon.canMovewithoutMessage(i, Math.abs(i + numD) % 26)) {
                        canMove = true;
                        noMovesAvailable = false;
                    }
                }
            } else {
                for (int i = 27; i >= 0; i--) {
                    if (boardBlackGammon.canMovewithoutMessage(i, Math.abs(i - numD) % 26)) {
                        canMove = true;
                        noMovesAvailable = false;
                    }
                }
            }

            if(!canMove) {
                numDice.remove(0);
                if (!numDice.isEmpty()) {
                    numD = numDice.get(0);
                }else {
                    if(noMovesAvailable) {
                        makeMessage("NO Move AVAILABLE for " + currentPlayer +
                                ".\n So " + getOppositeplayer(currentPlayer) + " has the turn NOW.");
                        canMove = true;
                    }else
                        break;
                }
            }else
                break;
        }
    }

    public Colors getOppositeplayer(Colors currentPlayer) {
        switch (currentPlayer) {
            case RED:
                return Colors.BLUE;
            case BLUE:
                return Colors.RED;
            default:
                return Colors.GREY;
        }
    }

    public ArrayList<Integer> getDice() {
        return dice;
    }

    public void setInput(ArrayList<Integer> input) {
        this.input = input;
    }

    public BoardBlackGammon getBoardBlackGammon() {
        return boardBlackGammon;
    }

    public void setCurrentPlayer(Colors currentPlayer) {
        this.currentPlayer = currentPlayer;
    }

    public Colors getCurrentPlayer() {
        return currentPlayer;
    }

    public ArrayList<Slot> getBoard() {
        return boardBlackGammon.board;
    }

    public boolean isMessage() {
        return hasMessage;
    }

    public String getMessage() {
        return message;
    }

    public Boolean getNoMovesAvailable() {
        return noMovesAvailable;
    }



}
