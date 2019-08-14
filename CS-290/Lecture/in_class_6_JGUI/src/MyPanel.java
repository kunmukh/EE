import java.awt.*;

public class MyPanel extends javax.swing.JPanel {
    @Override
    public void paint(Graphics g) {
        g.setColor(Color.GREEN);
        Dimension d = getSize();
        g.drawOval(0, 0, d.width, d.height);
    }
}
