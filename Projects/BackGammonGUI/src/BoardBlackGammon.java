public class BoardBlackGammon extends Board{

    public BoardBlackGammon(){
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
        boardMaker(1,1,Colors.BLUE,2);
        boardMaker(2,5,Colors.GREY,0);
        boardMaker(6,6,Colors.RED,5);
        boardMaker(7,7,Colors.RED,0);
        boardMaker(8,8,Colors.GREY,0);
        boardMaker(9,9,Colors.RED,3);
        boardMaker(10,12,Colors.GREY,0);
        boardMaker(13,13,Colors.BLUE,5);
        boardMaker(14,14,Colors.RED,5);
        boardMaker(15,17,Colors.GREY,0);
        boardMaker(18,18,Colors.BLUE,3);
        boardMaker(19,19,Colors.GREY,0);
        boardMaker(20,20,Colors.BLUE,0);
        boardMaker(21,21,Colors.BLUE,5);
        boardMaker(22,25,Colors.GREY,0);
        boardMaker(26,26,Colors.RED,2);
        boardMaker(27,27,Colors.BLUE,0);
        currentPlayer = Colors.RED;

    }

    public boolean isWin(Colors color){
        String baseName;
        if (color == Colors.RED)
            baseName = "R";
        else
            baseName = "B";

        return board.get(correlatorSpecial.get(baseName)).getNumColor() == 15;
    }

    public boolean canMovewithoutMessage(int start, int end){
        return canMove(start,end);
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
            board.get(correlatorSpecial.get(bearOffoppoColor)).setColor(opponentColor);
        }
    }

}
