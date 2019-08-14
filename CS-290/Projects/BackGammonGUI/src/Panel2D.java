import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.geom.Ellipse2D;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;

public class Panel2D extends JPanel {
    private int width, height;
    private ArrayList<Slot> board;
    private ArrayList<Integer> dice;
    private ArrayList<Integer> choice = new ArrayList<>();
    private String input;
    private GameplayBlackGammon game;
    private boolean player1won = false, player2won = false;

    public Panel2D(int width,int height){
        this.width = width;
        this.height = height;
        this.game = new GameplayBlackGammon();
        game.rollDice();
        game.setNoMovesAvailable();
        this.board = new ArrayList<>(game.getBoard());
        this.dice = new ArrayList<>(game.getDice());
        setBackground(Color.WHITE);
        int numChoice;

        addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                int player = 0;
                if (e.getY() < height/2){
                    player = 13 - (e.getX() / (width/15));
                    if (player > 7){
                        player --;
                    }else if(player == 7){
                        player = 26;
                    }
                }else{
                    player = 13 + (e.getX() / (width/15));
                    if (player > 19){
                        player --;
                    }else if(player == 19){
                        player = 27;
                    }
                }

                if(game.getNoMovesAvailable()){
                    setNewplayer();
                }else {
                    choice.add(player);
                    getMove();
                    updateMove(player, choice.size());
                }
            }
        });

    }

    private void setNewplayer() {
        game.setInput(new ArrayList<>());
        game.setCurrentPlayer(game.getOppositeplayer(game.getCurrentPlayer()));
        game.getBoardBlackGammon().setCurrentPlayer(game.getCurrentPlayer());
        game.rollDice();
        game.setNoMovesAvailable();
        update(game.getBoard(),game.getDice());
    }

    private void getMove() {
        input = "";
        if (choice.size() == 2) {
            String input1, input2;

            if (choice.get(0) == 0) {
                input1 = "0";
            } else if (choice.get(0) == 25) {
                input1 = "27";
            } else if (choice.get(0) == 26) {
                input1 = "25";
            } else if (choice.get(0) == 27) {
                input1 = "26";
            } else {
                input1 = Integer.toString(choice.get(0));
            }

            if (choice.get(1) == 0) {
                input2 = "0";
            } else if (choice.get(1) == 25) {
                input2 = "27";
            } else if (choice.get(1) == 26) {
                input2 = "25";
            } else if (choice.get(1) == 27) {
                input2 = "26";
            } else {
                input2 = Integer.toString(choice.get(1));
            }

            input = input1 + "-" + input2 + "-";
            runGame(input);
            choice.clear();
        }
    }

    private void runGame(String input) {
        if (!game.getBoardBlackGammon().isWin(Colors.RED) && !game.getBoardBlackGammon().isWin(Colors.BLUE)) {

            if(!game.getNoMovesAvailable()) {
                game.run(input);
                update(game.getBoard(), game.getDice());
                game.setNoMovesAvailable();
            }

            if (game.getDice().isEmpty() || game.getNoMovesAvailable()) {
                setNewplayer();
            }
        }

        if (game.getBoardBlackGammon().isWin(Colors.RED)) {
            player1won = true;
            update(game.getBoard(), game.getDice());
        }
        if (game.getBoardBlackGammon().isWin(Colors.BLUE)) {
            player2won = true;
            update(game.getBoard(), game.getDice());
        }
    }

    @Override
    public Dimension getPreferredSize() {
        return new Dimension(width + 15, height+100);
    }

    public void paintComponent(Graphics g){
        super.paintComponent(g);
        Graphics2D g2 = (Graphics2D)g;

        drawTriangleUpper(g2, 0, 6, Color.BLACK);
        drawTriangleUpper(g2, 1, 6, Color.LIGHT_GRAY);
        drawTriangleUpper(g2, 7, 13, Color.BLACK);
        drawTriangleUpper(g2, 8, 13, Color.LIGHT_GRAY);
        drawTriangleLower(g2, 0, 6, Color.LIGHT_GRAY);
        drawTriangleLower(g2, 1, 6, Color.BLACK);
        drawTriangleLower(g2, 7, 13, Color.LIGHT_GRAY);
        drawTriangleLower(g2, 8, 13, Color.BLACK);
        drawBox(g2, Color.orange, 6);
        drawBox(g2, Color.black, 14);
        drawBoxSpecial(g2, new Color(175,25,0), 0);
        drawBoxSpecial(g2, new Color(0,25,98), height/2);
        drawBorder(g2);
        drawSpecialborder(g2);
        drawCoin(g2);
        drawDice(g2);
        Font font = new Font("Serif", Font.BOLD, 75);
        if (game.getCurrentPlayer().equals(Colors.RED) && !player1won){
            g2.setColor(Color.RED);
            g2.setFont(font);
            g2.drawString("Player 1 TURN!!!", 0, height+75);
        }
        if (game.getCurrentPlayer().equals(Colors.BLUE) && !player2won){
            g2.setColor(Color.BLUE);
            g2.setFont(font);
            g2.drawString("Player 2 TURN", width/2, height+75);
        }
        font = new Font("Serif", Font.BOLD, 150);
        if (player1won){
            g2.setColor(Color.RED);
            g2.setFont(font);
            g2.drawString("Player 1 WON!!!", 20, height/2);
        }
        if (player2won){
            g2.setColor(Color.BLUE);
            g2.setFont(font);
            g2.drawString("Player 2 WON!!!", 20, height/2);
        }
        if(game.isMessage()){
            if(game.getCurrentPlayer().equals(Colors.RED)) {
                g2.setColor(Color.RED);
            }
            else {
                g2.setColor(Color.blue);
            }
            String text;
            if (game.getMessage().contains("Wrong Direction"))
                text = game.getMessage().substring(0,20);
            else
                text = game.getMessage();
            game.setMessagefalse();
            font = new Font("TimesRoman", Font.BOLD, 40);
            g2.setFont(font);
            drawString(g2,text,15,height /5);
        }
    }

    private void drawSpecialborder(Graphics2D g2) {
        g2.setColor(Color.yellow);
        float thickness = 15;
        Stroke oldStroke = g2.getStroke();
        g2.setStroke(new BasicStroke(thickness));
        g2.drawRect(((width / 15) * 13),0,width/15,height/2);
        g2.drawRect(((width / 15) * 13),height/2,width/15,height/2);
        g2.setStroke(oldStroke);
    }

    private void drawDice(Graphics2D g2) {
        for (int i = 0; i < dice.size(); i++) {
            if (game.getCurrentPlayer().equals(Colors.RED)) {
                try {
                    String path = Integer.toString(dice.get(i)) + ".jpg";
                    g2.drawImage(ImageIO.read(new File(path)), width - width / 15, (85 * i), this);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            else{
                try {
                    String path = Integer.toString(dice.get(i)) + ".jpg";
                    g2.drawImage(ImageIO.read(new File(path)), width - width / 15, height - (85*(i+1)), this);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

        }
    }

    private void drawCoin(Graphics2D g2) {
        int startWidth = width - ((width/15) + (width/22));
        for (int i = 0; i < board.size(); i++){
            Ellipse2D e;
            if (i <= 13) {
                g2.setColor(getColor(board.get(i).getColor()));
                for (int j = 0; j < board.get(i).getNumColor();j++) {
                    e = new Ellipse2D.Double(startWidth - ((width / 15) * i), height / 90 + ((height / 30)*j), (width / 40), (height / 30));
                    g2.fill(e);

                    g2.setColor(Color.white);
                    float thickness = 5;
                    Stroke oldStroke = g2.getStroke();
                    g2.setStroke(new BasicStroke(thickness));
                    g2.drawOval(startWidth - ((width / 15) * i),height / 90 + ((height / 30)*j),(width / 40),(height / 30));
                    g2.setStroke(oldStroke);
                    g2.setColor(getColor(board.get(i).getColor()));

                }
            }else {
                g2.setColor(getColor(board.get(i).getColor()));
                int k = i - 14;
                for (int j = 0; j < board.get(i).getNumColor();j++) {
                    e = new Ellipse2D.Double(((width/ 50) + (width / 15) * k), height - (height / 30)- ((height / 30)*j), (width / 40), (height / 30));
                    g2.fill(e);

                    g2.setColor(Color.white);
                    float thickness = 5;
                    Stroke oldStroke = g2.getStroke();
                    g2.setStroke(new BasicStroke(thickness));
                    g2.drawOval(((width/50)+ (width / 15) * k),height - (height / 30)- ((height / 30)*j),(width / 40),(height / 30));
                    g2.setStroke(oldStroke);
                    g2.setColor(getColor(board.get(i).getColor()));

                }
            }
        }
    }

    private void drawBorder(Graphics2D g2) {
        g2.setColor(Color.black);
        float thickness = 20;
        Stroke oldStroke = g2.getStroke();
        g2.setStroke(new BasicStroke(thickness));
        g2.drawRect(0,0,width,height);
        g2.setStroke(oldStroke);
    }

    private void drawBoxSpecial(Graphics2D g2, Color red, int i) {
        Shape bearoff;
        g2.setColor(red);
        bearoff = new Rectangle(((width / 15) * 13), i, (width / 15), height / 2);
        g2.fill(bearoff);
    }

    private void drawBox(Graphics2D g2, Color orange, int i) {
        Shape bearoff;
        g2.setColor(orange);
        bearoff = new Rectangle(((width / 15) * i), 0, (width / 15), height);
        g2.fill(bearoff);
    }

    private void drawTriangleLower(Graphics2D g2, int i2, int i3, Color lightGray) {
        for (int i = i2; i < i3; i = i + 2) {
            g2.setColor(lightGray);
            g2.fillPolygon(new int[]{((width / 15) * i), (width / 30) + ((width / 15) * i), (width / 15) + ((width / 15) * i)}, new int[]{height, (height / 2), height}, 3);
        }
    }

    private void drawTriangleUpper(Graphics2D g2, int i2, int i3, Color black) {
        for (int i = i2; i < i3; i = i + 2) {
            g2.setColor(black);
            g2.fillPolygon(new int[]{((width / 15) * i), (width / 30) + ((width / 15) * i), (width / 15) + ((width / 15) * i)}, new int[]{0, (height / 2), 0}, 3);
        }
    }

    private Color getColor(Colors color){

        switch (color){
            case RED:
                return Color.RED;
            case BLUE:
                return new Color(58 ,44,255);
            default:
                return Color.white;
        }
    }

    private void update(ArrayList<Slot> newBoard, ArrayList<Integer> newDice){
            board = new ArrayList<Slot>(newBoard);
            dice = new ArrayList<Integer>(newDice);
            Graphics g = this.getGraphics();
            paintComponent(g);
    }

    private void updateMove(int player, int num){
        Graphics g = this.getGraphics();
        if (num == 1)
            paintComponent(g);
        Color myColour = new Color(125, 255, 255, 175 );
        g.setColor(myColour);
        if(player <= 12) {
            if (player >= 7)
                g.fillRect(width / 15 * (12 - player), 0, width / 15, height / 2);
            else
                g.fillRect(width / 15 * (12 - (player - 1)), 0, width / 15, height / 2);
        }
        else {
            if (player <= 18)
                g.fillRect(width / 15 * (player - 13), height / 2, width / 15, height / 2);
            else if (player < 26)
                g.fillRect(width / 15 * (player - 12), height / 2, width / 15, height / 2);
        }
        if (player == 26){
            g.fillRect(width / 15 * 6, 0, width / 15, height / 2);
        }
        if (player == 27){
            g.fillRect(width / 15 * 6, height / 2, width / 15, height / 2);
        }
        if (num == 2)
            paintComponent(g);

    }

    private void drawString(Graphics g, String text, int x, int y) {
        for (String line : text.split("\n"))
            g.drawString(line, x, y += g.getFontMetrics().getHeight());
    }
}
