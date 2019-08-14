import javax.swing.*;
import java.awt.*;

public class PieChartView extends JPanel {
    private BoundedRangeModel models[];    // array of models

    public void setModels(BoundedRangeModel models[]) {
        this.models = models;

        for(BoundedRangeModel each : models) {
            each.addChangeListener(e -> repaint());
        }
    }

    @Override
    public void paint(Graphics g) {
        if (models == null) return;
        g.setColor(Color.BLACK);
        int size = Math.min(getWidth(), getHeight());
        int x = (getWidth() - size) / 2;
        int y = (getHeight() - size) / 2;

        g.drawOval(x, y, size, size);

        int total = 0;
        for (BoundedRangeModel each : models) {
            total += each.getValue();
        }

        Color c[] = { Color.RED, Color.YELLOW, Color.GREEN, Color.BLUE};
        int currentAngle = 0;

        for (int i=0; i<4; i++) {
            int newAngle = models[i].getValue() * 360 / total;
            g.setColor(c[i]);
            g.fillArc(x, y, size, size, currentAngle, newAngle);
            currentAngle += newAngle;
        }

    }
}
