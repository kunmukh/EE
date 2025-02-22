/*
 * @(#)RectangleFigure.java
 *
 * Project:		JHotdraw - a GUI framework for technical drawings
 *				http://www.jhotdraw.org
 *				http://jhotdraw.sourceforge.net
 * Copyright:	� by the original author(s) and all contributors
 * License:		Lesser GNU Public License (LGPL)
 *				http://www.opensource.org/licenses/lgpl-license.html
 */

package CH.ifa.draw.figures;

import java.awt.*;
import java.io.IOException;
import java.util.Vector;
import CH.ifa.draw.framework.*;
import CH.ifa.draw.standard.*;
import CH.ifa.draw.util.*;


/**
 * A rectangle figure.
 *
 * @version <$CURRENT_VERSION$>
 */

@SuppressWarnings("ALL")
public class SpecialFigure extends AttributeFigure {

    private Rectangle   fDisplayBox;

    /*
     * Serialization support.
     */
    private static final long serialVersionUID = 184722075881789163L;
    private int rectangleFigureSerializedDataVersion = 1;


    public SpecialFigure() {

        this(new Point(0,0), new Point(0,0));
    }

    public SpecialFigure(Point origin, Point corner) {
        basicDisplayBox(origin,corner);
    }

    public Vector handles() {
        Vector handles = new Vector();
        BoxHandleKit.addHandles(this, handles);
        return handles;
    }

    public void basicDisplayBox(Point origin, Point corner) {
        fDisplayBox = new Rectangle(origin);
        fDisplayBox.add(corner);
    }

    public Rectangle displayBox() {
        return new Rectangle(
                fDisplayBox.x,
                fDisplayBox.y,
                fDisplayBox.width,
                fDisplayBox.height);
    }

    protected void basicMoveBy(int x, int y) {
        fDisplayBox.translate(x,y);
    }

    public void drawBackground(Graphics g) {
        Rectangle r = displayBox();
        g.fillRect(r.x, r.y, r.width, r.height);
        g.fillOval(r.x, r.y, r.width, r.height);
    }

    public void drawFrame(Graphics g) {
        Rectangle r = displayBox();
        g.drawRect(r.x, r.y, r.width-1, r.height-1);
        g.drawOval(r.x, r.y, r.width-1, r.height-1);
    }



    //-- store / load ----------------------------------------------

    public void write(StorableOutput dw) {
        super.write(dw);
        dw.writeInt(fDisplayBox.x);
        dw.writeInt(fDisplayBox.y);
        dw.writeInt(fDisplayBox.width);
        dw.writeInt(fDisplayBox.height);
    }

    public void read(StorableInput dr) throws IOException {
        super.read(dr);
        fDisplayBox = new Rectangle(
                dr.readInt(),
                dr.readInt(),
                dr.readInt(),
                dr.readInt());
    }

}
