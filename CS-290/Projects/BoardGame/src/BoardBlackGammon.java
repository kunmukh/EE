import java.util.ArrayList;

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

    public void draw() {
        System.out.println("\nScore: RED :" + board.get(correlatorSpecial.get("R")).getNumColor()
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

        for (int j = 0; j < 5; j++) {
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

        for (int j = 0; j < 5; j++) {
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
