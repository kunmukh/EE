import javax.swing.*;

public class BackGammonGUI extends JApplet{
    private JFrame frame;
    private Panel2D panel;

    public static void main(String args[]){
        BackGammonGUI gui = new BackGammonGUI();
        gui.run();
    }

    public void run(){
        frame = new JFrame();
        frame.setTitle("BackGammonGUI");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        JApplet applet = new BackGammonGUI();
        applet.init();
        frame.getContentPane().add(applet);
        frame.pack();
        frame.setVisible(true);
        frame.setResizable(false);
    }

    public void init(){
        panel = new Panel2D(1200,700);
        getContentPane().add(panel);
    }

}
