import java.util.InputMismatchException;
import java.util.Scanner;

public class BoardGame {
    public static void main(String args[]){
        BoardGame game = new BoardGame();
        game.gameplay();
    }

    private void gameplay(){
        int choice = 0;
        boolean rightInput = false;
        Scanner sc = new Scanner(System.in);
        while(!rightInput) {
            MainInput mainInput = new MainInput(choice).invoke();
            choice = mainInput.getChoice();
            rightInput = mainInput.isRightInput();
        }

        while(choice != 3){
            rightInput = false;
            if (choice == 2){
                System.out.println("Mancala has been selected.");
                Gameplay game = new GameplayMancala();
                game.run();

            }else if (choice == 1){
                System.out.println("BlackGammon has been selected.");
                Gameplay game = new GameplayBlackGammon();
                game.run();
            }
            while(!rightInput) {
                MainInput mainInput = new MainInput(choice).invoke();
                choice = mainInput.getChoice();
                rightInput = mainInput.isRightInput();
            }
        }
    }

    private class MainInput {
        private int choice;
        private boolean rightInput;

        public MainInput(int choice) {
            this.choice = choice;
        }

        public int getChoice() {
            return choice;
        }

        public boolean isRightInput() {
            return rightInput;
        }

        public MainInput invoke() {
            Scanner sc;
            System.out.println("What game do want to play (1 or 2)?\n1> BlackGammon\n2> Mancala\n3> Quit");
            System.out.print("> ");
            sc = new Scanner(System.in);
            try {
                choice = sc.nextInt();
                rightInput = true;
            } catch (InputMismatchException e) {
                rightInput = false;
                System.out.println("You have entered a wrong Input. (1 or 2)");
            }
            return this;
        }
    }
}
