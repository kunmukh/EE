import java.util.ArrayList;

public class Print {
    private ArrayList<Integer> pieces;
    private String holeEmpty = "       ";

    Print(ArrayList<Integer> board){
        pieces = new ArrayList<>(board);
    }

    public void printBoard() {
        Store player1 = new Store(pieces.get(6));
        Store player2 = new Store(pieces.get(13));
        Hole hole1 = new Hole(pieces.get(0),pieces.get(12), "1", "12");
        Hole hole2 = new Hole(pieces.get(1),pieces.get(11), "2", "11");
        Hole hole3 = new Hole(pieces.get(2),pieces.get(10), "3", "10");
        Hole hole4 = new Hole(pieces.get(3),pieces.get(9), "4", "9");
        Hole hole5 = new Hole(pieces.get(4),pieces.get(8), "5", "8");
        Hole hole6 = new Hole(pieces.get(5),pieces.get(7), "6", "7");

        line();
        Score(pieces.get(13));
        System.out.println("----------------[ Player 2 ]---------------------");
        System.out.println(player2.toString());
        line();
        System.out.println(hole1.toString());
        System.out.println(hole2.toString());
        System.out.println(hole3.toString());
        System.out.println(hole4.toString());
        System.out.println(hole5.toString());
        System.out.println(hole6.toString());
        System.out.println(player1.toString());
        System.out.println("----------------[ Player 1 ]---------------------");
        Score(pieces.get(6));
        line();

    }

    private void Score(int score){
        System.out.println("----------------[ Score " + score + " ]----------------------");
    }

    private void line(){
        System.out.println("-------------------------------------------------");
    }
}
