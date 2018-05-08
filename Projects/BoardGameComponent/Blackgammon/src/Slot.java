public class Slot {
    private Colors Color;
    private int numColor;

    public Slot(int initialCoin, Colors color ){
        Color = color;
        numColor = initialCoin;
    }

    public Colors getColor() {
        if (numColor == 0) {
            return Colors.GREY;
        }
        return Color;
    }

    public void setColor(Colors color) {
        Color = color;
    }

    public int getNumColor() {
        return numColor;
    }

    public void setNumColor(int numColor) {
        this.numColor = numColor;
    }

    public void setSlot(int numColor, Colors color){
        setNumColor(numColor);
        setColor(color);
    }

    public void increNumcolor (){
        numColor = numColor + 1;
    }

    public void decrNumcolor (){
        numColor = numColor - 1;
        if (numColor == 0)
            Color = Colors.GREY;
    }

    @Override
    public String toString(){
        switch (Color){
            case RED:
                return stringMaker(Colors.RED);
            case BLUE:
                return stringMaker(Colors.BLUE);
            case GREY:
                return stringMaker(Colors.GREY);
            default:
                return "";
        }

    }

    private String stringMaker (Colors colour){
        StringBuilder symbol = new StringBuilder();
        int numColortemp = numColor;
        String character = "";

        switch (colour){
            case RED:
                character = "О";
                break;
            case BLUE:
                character = "Θ";
                break;
            case GREY:
                character = "";
                break;
            default:
                break;
        }

        for (int i = 0; i < 15; i++) {
            if (numColor != 0) {
                if (numColortemp != 0) {
                    symbol.append(character);
                    numColortemp--;
                } else
                    symbol.append(" ");
            }
            else {
                symbol.append("  *    *    *  ");
                break;
            }
        }

        return symbol.toString();
    }
}
