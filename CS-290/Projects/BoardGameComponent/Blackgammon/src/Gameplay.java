import com.sun.javafx.image.BytePixelSetter;

import java.util.ArrayList;
import java.util.NoSuchElementException;
import java.util.Random;
import java.util.Scanner;

public class Gameplay {
    private Board board;
    private ArrayList<Integer> dice = new ArrayList<>();
    private ArrayList<Integer> input = new ArrayList<>();
    private Colors currentPlayer;
    private String dicesName;
    private Boolean skip, found, playerInput, bearOff, skipBearOff;

    public Gameplay() {
        System.out.println("*-*-*-*-*- BLACKGAMMON -*-*-*-*-*");
        System.out.println("RED- О\nBLUE- Θ");
        board = new Board();
        board.draw();
        currentPlayer = board.getCurrentPlayer();
        skip = false;
        found = false;
        playerInput = false;
        bearOff = false;
        skipBearOff = false;
    }

    public void run() {
        int start, end;
        currentPlayer = Colors.RED;
        skip = false;

        rollDice();
        getInputLogic();

        while (skip) {
            currentPlayer = getOppositeplayer(currentPlayer);
            board.setCurrentPlayer(currentPlayer);
            board.draw();
            rollDice();
            getInputLogic();
        }

        start = input.get(0);
        input.remove(0);
        end = input.get(0);
        input.remove(0);

        while (!board.isWin(Colors.RED) && !board.isWin(Colors.BLUE)) {
            board.move(start, end);
            board.draw();
            input.clear();

            if (dice.isEmpty()) {
                input.clear();
                currentPlayer = getOppositeplayer(currentPlayer);
                board.setCurrentPlayer(currentPlayer);
                board.draw();
                rollDice();
            }

            if (!board.isWin(Colors.RED) && !board.isWin(Colors.BLUE)) {
                getInputLogic();
            } else
                break;

            while (skip) {
                currentPlayer = getOppositeplayer(currentPlayer);
                board.setCurrentPlayer(currentPlayer);
                board.draw();
                rollDice();
                getInputLogic();
            }

            start = input.get(0);
            input.remove(0);
            end = input.get(0);
            input.remove(0);
        }

        if (board.isWin(Colors.RED)) {
            System.out.println("Congratulations!!! Red has Won");
        }
        if (board.isWin(Colors.BLUE)) {
            System.out.println("Congratulations!!! Blue has Won");
        }
    }

    private void getInputLogic() {
        GetInput getInputinitital = new GetInput().invoke(1);
        int startNum = getInputinitital.getStartNum();
        int endNum = getInputinitital.getEndNum();
        int result = getInputinitital.getResult();

        while (input.isEmpty() && !skip) {

            if (bearOff) {
                skipBearOff = true;
                while (!found && bearOff && !skip) {
                    for (Integer i : dice) {
                        if (((i.equals(endNum)) && currentPlayer.equals(Colors.BLUE)) ||
                                ((endNum == (25 - i)) && currentPlayer.equals(Colors.RED))) {
                            if (board.canMove(startNum, endNum)) {
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
                        GetInput getInput = new GetInput().invoke(2);
                        startNum = getInput.getStartNum();
                        endNum = getInput.getEndNum();
                        if (skip) {
                            break;
                        }
                    }
                }
            }

            if (!skipBearOff && !skip) {
                while (!playerInput && !skipBearOff && !skip) {

                    while (!playerInput) {
                        if (currentPlayer == Colors.RED) {
                            if (startNum > endNum) {
                                playerInput = true;
                            } else
                                System.out.println("Wrong Direction.");
                        } else if (currentPlayer == Colors.BLUE) {
                            if (startNum < endNum) {
                                playerInput = true;
                            } else
                                System.out.println("Wrong Direction.");
                        }
                        if (!playerInput) {
                            GetInput getInput = new GetInput().invoke(2);
                            startNum = getInput.getStartNum();
                            endNum = getInput.getEndNum();
                            result = getInput.getResult();
                            if (skip)
                                break;
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
                                        if (board.canMove(startNum, endNum)) {
                                            InputSetter(startNum, endNum, i);
                                        }
                                        break;
                                    }
                                } else if ((i.equals(result))) {
                                    if (board.canMove(startNum, endNum)) {
                                        InputSetter(startNum, endNum, i);
                                        break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (!found) {
                            GetInput getInput = new GetInput().invoke(2);
                            startNum = getInput.getStartNum();
                            endNum = getInput.getEndNum();
                            result = getInput.getResult();
                            break;
                        }
                    }
                }
            }
        }

    }

    private void InputSetter(int startNum, int endNum, Integer i) {
        input.add(startNum);
        input.add(endNum);
        dice.remove(i);
        numDicesLeft();
        found = true;
        return;
    }

    private void rollDice() {
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
        for (Integer i : dice) {
            output.append(" " + i + " ");

        }

        dicesName = output.toString();

    }

    private void numDicesLeft() {
        StringBuilder output = new StringBuilder();
        output.append("The number on the dice are: ");
        for (Integer i : dice) {
            output.append(" " + i + " ");

        }
        dicesName = output.toString();
    }

    private Colors getOppositeplayer(Colors currentPlayer) {
        switch (currentPlayer) {
            case RED:
                return Colors.BLUE;
            case BLUE:
                return Colors.RED;
            default:
                return Colors.GREY;
        }
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

        public GetInput invoke(int choice) {
            skip = false;

            ArrayList<Integer> numDice = new ArrayList<>(dice);

            boolean canMove = false;
            int numD = numDice.get(0);


            while (!canMove) {
                if (currentPlayer.equals(Colors.BLUE)) {
                    for (int i = 0; i <= 27; i++) {
                        if (board.canMovewithoutMessage(i, Math.abs(i + numD) % 28)) {
                            canMove = true;
                        }
                    }
                } else {
                    for (int i = 27; i >= 0; i--) {
                        if (board.canMovewithoutMessage(i, Math.abs(i - numD) % 28)) {
                            canMove = true;
                        }
                    }
                }
                if (!canMove && !numDice.isEmpty()) {
                    numDice.remove(0);
                    if(!numDice.isEmpty())
                        numD = numDice.get(0);
                } else {
                    if(numDice.isEmpty() && !canMove) {
                        skip = true;
                        System.out.println("No moves available for " + currentPlayer);
                        break;
                    }
                }

            }

            found = false;
            playerInput = false;
            bearOff = false;
            skipBearOff = false;
            input.clear();

            String endInputTemp, startInputTemp;
            if (!skip) {
                if(choice == 2) {
                    System.out.println("Invalid Input. Please enter the correct input.");
                }
                System.out.println("Player " + currentPlayer + " turn.");
                numDicesLeft();
                System.out.println(dicesName);
                System.out.println("Enter the spot 'initial-final' to move the pieces. Type 'skip' to skip your turn if you cannot move your pieces.");
                System.out.print("> ");

                Scanner getRawinput = new Scanner(System.in);
                String raw = getRawinput.nextLine() + "-";
                Scanner in = new Scanner(raw).useDelimiter("-");
                try {
                    startInputTemp = in.next();
                } catch (NoSuchElementException e) {
                    System.out.println("You need to enter a number.");
                    startInputTemp = "-0000";
                }
                try {
                    startNum = Integer.valueOf(startInputTemp);
                } catch (NumberFormatException e) {
                    if (startInputTemp.toUpperCase().contentEquals("R"))
                        startNum = 0;
                    else if (startInputTemp.toUpperCase().contentEquals("B"))
                        startNum = 27;
                    else if (startInputTemp.toUpperCase().contentEquals("RB")) {
                        startNum = 25;
                        bearOff = true;
                        skipBearOff = true;
                    } else if (startInputTemp.toUpperCase().contentEquals("BB")) {
                        startNum = 26;
                        bearOff = true;
                        skipBearOff = true;
                    } else if (startInputTemp.toLowerCase().contentEquals("skip")) {
                        skip = true;
                        found = true;
                        endNum = 0;
                    } else
                        System.out.println("Incorrect Input for initial input.");
                }
                if (!skip) {
                    try {
                        endInputTemp = in.next();
                    } catch (NoSuchElementException e) {
                        System.out.println("Incorrect Number format");
                        endInputTemp = "-99";
                    }
                    try {
                        endNum = Integer.valueOf(endInputTemp);
                    } catch (NumberFormatException e) {
                        if (endInputTemp.toUpperCase().contentEquals("R"))
                            endNum = 0;
                        else if (endInputTemp.toUpperCase().contentEquals("B"))
                            endNum = 27;
                        else
                            System.out.println("Incorrect Input for final input.");
                    }
                }
            }
            result = endNum - startNum;
            if (result < 0)
                result *= -1;
            return this;
        }
    }
}
