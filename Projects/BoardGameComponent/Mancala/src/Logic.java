import javafx.util.Pair;

import java.util.ArrayList;

public class Logic {
    private ArrayList<Integer> board;
    private ArrayList <Pair <Integer,Integer> > l;
    private boolean player1;
    private boolean extraTurn = false;

    Logic(ArrayList<Integer> oldState, String player){
        board = new ArrayList<>(oldState);
        player1 = player.contentEquals("player1");
        pairCreator();
    }

    public void play (int hole) {
        ArrayList<Integer> boardCopy = new ArrayList<>(board);
        int holeIndex = 0;
        if (hole < 7)
            holeIndex = hole - 1;
        else
            holeIndex = hole;

        int locContent = boardCopy.get(holeIndex);
        boardCopy.remove(holeIndex);
        boardCopy.add(holeIndex, 0);

        int realLocation = 0;
        boolean score,extraT = false;
        for (int i = 1; i < locContent + 1; i++) {
            score = false;
            realLocation = (holeIndex + i) % 14;

            if (player1) {
                if (realLocation == 13) {
                    realLocation = (holeIndex + locContent + 1) % 14;
                }
            } else {
                if (realLocation == 6) {
                    realLocation = (holeIndex + locContent + 1) % 14;
                }
            }
            if (realLocation == 6 || realLocation == 13){
                score = true;
                if (i == locContent){
                    extraT = true;
                }
            }
            int nextCellcontent = boardCopy.get(realLocation);
            boardCopy.remove(realLocation);
            boardCopy.add(realLocation, nextCellcontent + 1);

            int locContent0;

            if ((nextCellcontent == 0) && !score) {
                nextCellcontent = boardCopy.get(getPair(realLocation));
                if (player1) {
                    locContent0 = boardCopy.get(6);
                    boardCopy.remove(6);
                    boardCopy.add(6,locContent0 + nextCellcontent);
                }
                else {
                    locContent0 = boardCopy.get(13);
                    boardCopy.remove(13);
                    boardCopy.add(13,locContent0 + nextCellcontent);
                }
                boardCopy.remove(getPair(realLocation));
                boardCopy.add(getPair(realLocation),0);
            }
        }

        if (extraT ){
            extraTurn = true;
        }

        board = boardCopy;

    }

    public boolean isGameFinish(){
        if (isPlayer1empty() || isPlayer2empty()){
            return true;
        }
        return false;
    }

    private boolean isPlayer1empty() {
        ArrayList<Integer> temp = new ArrayList<>(board);
        for (int i = 0; i < 6; i++){
            if (!temp.get(i).equals(0)){
                return false;
            }
        }
        return true;
    }

    private boolean isPlayer2empty() {
        ArrayList<Integer> temp = new ArrayList<>(board);
        for (int i = 7; i < 13; i++){
            if (!temp.get(i).equals(0)){
                return false;
            }
        }
        return true;
    }

    public boolean isExtraturn() {
        return !extraTurn;
    }

    public int getPlayer1score() {
        return getPlayerscore(6);
    }

    public int getPlayer2score() {
        return getPlayerscore(13);
    }

    private int getPlayerscore(int player) {
        int n = 0;
        if (player == 13){
            n = 7;
        }
        if (!isGameFinish()) {
            return board.get(player);
        }
        else {
            int total = board.get(player);
            for (int i  = 0 + n; i < player; i++) {
                total += board.get( i);
            }
            int locContent1, locContent2;
            for (int i = 0 + n; i < player; i++){
                if (!board.get(i).equals(0)){
                    locContent1 = board.get(i);
                    locContent2 = board.get(player);
                    board.remove(i);
                    board.add(i,0);
                    board.remove(player);
                    board.add(player,locContent1+locContent2);

                }
            }
            return total;
        }
    }

    public ArrayList<Integer> getBoard() {
        return board;
    }

    private int getPair(int num){
        int val, answer = 0;
        for (Pair <Integer, Integer> temp : l){
            val = temp.getKey();
            if (val == num){
                answer = temp.getValue();
            }
        }
        return answer;
    }

    private void pairCreator(){

        l = new ArrayList <Pair <Integer,Integer> > ();

        l.add(new Pair <Integer, Integer> (0,12));
        l.add(new Pair <Integer, Integer> (12,0));
        l.add(new Pair <Integer, Integer> (1,11));
        l.add(new Pair <Integer, Integer> (11,1));
        l.add(new Pair <Integer, Integer> (2,10));
        l.add(new Pair <Integer, Integer> (10,2));
        l.add(new Pair <Integer, Integer> (3,9));
        l.add(new Pair <Integer, Integer> (9,3));
        l.add(new Pair <Integer, Integer> (4,8));
        l.add(new Pair <Integer, Integer> (8,4));
        l.add(new Pair <Integer, Integer> (5,7));
        l.add(new Pair <Integer, Integer> (7,5));

    }
}
