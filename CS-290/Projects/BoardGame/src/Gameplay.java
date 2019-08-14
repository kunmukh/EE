public abstract class Gameplay {
    Colors currentPlayer;

    abstract void run();
    abstract void getInputLogic();

    public Colors getOppositeplayer(Colors currentPlayer) {
        switch (currentPlayer) {
            case RED:
                return Colors.BLUE;
            case BLUE:
                return Colors.RED;
            default:
                return Colors.GREY;
        }
    }
}
