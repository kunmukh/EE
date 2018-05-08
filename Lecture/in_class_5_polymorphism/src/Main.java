import java.util.Vector;

public class Main {
    public static void main(String[] args){
        Vector<Drawable> figs = new Vector<>();

        Drawable d;

        d = new NotAFigure();

        figs.add(d);
        figs.add(new CircleFigure());
        figs.add(new LineFigure());
        figs.add(new RectangleFigure());
        figs.add(new NothingFigure());

        //polymorphism here
        for (Drawable each : figs){
            each.draw();
        }
    }
}
