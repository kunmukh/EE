import java.util.InputMismatchException;
import java.util.Scanner;

public class GameplayMancala extends Gameplay {
    private BoardMancala boardMancala;
    private int start, end;

    public GameplayMancala() {
        System.out.println("*-*-*-*-*- MANCALA -*-*-*-*-*");
        System.out.println("RED- 1- 6 BLUE- 7 - 12\n");
        boardMancala = new BoardMancala();
        boardMancala.draw();
        currentPlayer = boardMancala.getCurrentPlayer();
    }

    void run() {
        System.out.println("***** Mancala *****\n" + "Type number 1-12 to move the stones from hole.\n Type 'QUIT' to quit the game.");

        currentPlayer = Colors.RED;
        getInputLogic();
        end = boardMancala.numColslot(start);

        while (!boardMancala.isHomeready(Colors.RED) && !boardMancala.isHomeready(Colors.BLUE)) {
            boardMancala.move(start, end);
            boardMancala.draw();

            if (!boardMancala.isExtraturn()) {
                currentPlayer = getOppositeplayer(currentPlayer);
                boardMancala.setCurrentPlayer(currentPlayer);
            }

            if (!boardMancala.isHomeready(Colors.RED) && !boardMancala.isHomeready(Colors.BLUE)) {
                getInputLogic();
                end = boardMancala.numColslot(start);
            } else
                break;
        }
        boardMancala.finalGameboard();
        boardMancala.draw();
        System.out.println("RED: " + boardMancala.getScore(Colors.RED) + " BLUE: " + boardMancala.getScore(Colors.BLUE));
        if (boardMancala.isWin(Colors.RED)) {
            System.out.println("Congratulations!!! Red has Won");
        }
        else {
            System.out.println("Congratulations!!! Blue has Won");
        }

    }

    public void getInputLogic() {
        if (currentPlayer.equals(Colors.RED)) {
            readPlayerInput(1, 6, currentPlayer);
        } else {
            readPlayerInput(7, 12, currentPlayer);
        }
    }

    private void readPlayerInput(int startP, int endP, Colors playerTurn) {
        int turn = 99999;
        while (turn < startP || turn > endP) {
            Scanner scanner = new Scanner(System.in);
            System.out.println("Enter a number between " + startP + " - " + endP);
            System.out.print(playerTurn + " > ");
            try {
            turn = scanner.nextInt();
            if (boardMancala.isSlotEmpty(turn)) {
                turn = 99999;
                System.out.println("Input slot is empty!!!");
            }
            } catch (InputMismatchException e) {
                System.out.println("Incorrect Input!!!Enter a valid board number.");
                turn = 99999;
            }
        }
        start = turn;
    }



}
