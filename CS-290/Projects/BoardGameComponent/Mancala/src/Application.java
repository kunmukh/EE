import java.util.ArrayList;
import java.util.InputMismatchException;
import java.util.Scanner;

public class Application {
    public static void main(String args[]){
        System.out.println("***** Mancala *****\n" + "Type number 1-12 to move the stones from hole.\n Type 'QUIT' to quit the game." );
        boolean player_1 = true;
        boolean win = false;
        String player1 = "player1";
        String player2 = "player2";
        String playerTurn = player1;
        ArrayList<Integer> board = new ArrayList<>();

        for (int i = 0; i < 6; i++)
            board.add(4);
        board.add(0);
        for (int i = 0; i < 6; i++)
            board.add(4);
        board.add(0);

        Print p = new Print(board);
        p.printBoard();

        Logic l = new Logic(board, player1);
        Application app = new Application();
        int turn = app.readInput(playerTurn, true);
        if (turn == 999){
            System.out.println("The Game has been quit. You have Lost");
            return;
        }

        while(!win)
        {
            if (player_1){
                l = new Logic(board, player1);
                l.play(turn);
                if (l.isExtraturn()) {
                    player_1 = false;
                    playerTurn = player2;
                }
            }else{
                l = new Logic(board, player2);
                l.play(turn);
                if (l.isExtraturn()) {
                    player_1 = true;
                    playerTurn = player1;
                }
            }

            board = l.getBoard();
            p = new Print(board);
            p.printBoard();

            if (!l.isGameFinish()){
                turn = app.readInput(playerTurn, player_1);
                if (turn == 999){
                    System.out.println("The Game has been quit. You have Lost");
                    return;
                }

            }
            win = l.isGameFinish();
        }
        System.out.println("Game Over!!!");

        int player1Score = l.getPlayer1score();
        int player2Score = l.getPlayer2score();

        if (player1Score > player2Score){
            System.out.println("Congratulations!! Player 1 has Won");
        }
        else if (player1Score < player2Score){
            System.out.println("Congratulations!! Player 2 has Won");
        }
        else{
            System.out.println("Congratulations!! It is a tie Game");
        }
        board = l.getBoard();
        p = new Print(board);
        p.printBoard();
    }

    private int readInput(String playerTurn, boolean player1){
        Scanner scanner = new Scanner(System.in);
        while(true) {
            try {
                System.out.print(playerTurn +" > ");
                scanner = new Scanner(System.in);
                int turn = scanner.nextInt();
                if (player1) {
                   turn = readPlayerInput(turn,1,6,playerTurn);
                }
                else{
                    turn = readPlayerInput(turn,7,12,playerTurn);

                }
                return turn;
            }catch (InputMismatchException e){
                if (scanner.next().toLowerCase().contentEquals("quit"))
                    return 999;
                else
                System.out.println("Incorrect Input!!!Enter a valid board number.");
            }
        }
    }

    private int readPlayerInput(int turn, int start, int end, String playerTurn){
        while (turn < start || turn > end) {
            System.out.println("Enter a number between " + start + " - " + end );
            System.out.print(playerTurn + " > ");
            Scanner scanner = new Scanner(System.in);
            turn = scanner.nextInt();
        }
        return turn;
    }
}
