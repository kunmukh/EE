import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.NoSuchElementException;
import java.util.Scanner;

public class Application {
    public static void main(String args[]){
        Application app = new Application();
        app.run();
    }
    private void run(){
        boolean win = false;
        boolean die = false;
        int i = 0;
        Riddle cheatRiddle = new Riddle("cheat_main");
        try (BufferedReader br = new BufferedReader(
                new FileReader("initialization.txt"))) {
            String line = null;
            while ((line = br.readLine()) != null) {
                System.out.println(line);
            }
        } catch (IOException e) {
            System.out.println("Could not open file");
        }

        Gameplay mission = new Gameplay();
        System.out.print("> ");
        Scanner scanner = new Scanner(System.in);
        String argument = scanner.nextLine();
        Scanner scanner1 = new Scanner(argument.toLowerCase()).useDelimiter(" ");
        String userAnswer = scanner1.next();


        while (!die) {

            if ((userAnswer.contentEquals("quit")) ||
                    (userAnswer.contains("exit"))){
                System.out.println("Sorry!!! You have lost the game");
                break;
            }
            else if (userAnswer.contentEquals("go")){
                try {
                    userAnswer = scanner1.next();
                    mission.doCommandgo(userAnswer);
                }catch (NoSuchElementException e) {
                    System.out.println("Wrong Usage. 'go <direction>");
                }
            }
            else if (userAnswer.contentEquals("look")){
                if (scanner1.hasNext()){
                    userAnswer = scanner1.next();
                    mission.doCommandLookobj(userAnswer);                                      }
                else
                    mission.doCommandLook();
            }
            else if (userAnswer.contentEquals("help")){
                try (BufferedReader br = new BufferedReader(
                        new FileReader("help.txt"))) {
                    String line = null;
                    while ((line = br.readLine()) != null) {
                        System.out.println(line);
                    }
                } catch (IOException e) {
                    System.out.println("Could not open file");
                }
            }
            else if (userAnswer.contentEquals("inventory")){
                mission.doCommandInventory();
            }
            else if (userAnswer.contentEquals("get")){
                try {
                    userAnswer = scanner1.next();
                    mission.doCommandget(userAnswer);
                }catch (NoSuchElementException e) {
                    System.out.println("Wrong Usage. 'get <Object>");
                }
            }
            else if ((userAnswer.contentEquals("dig")) ||
                    (userAnswer.contentEquals("unscrew")) ||
                    (userAnswer.contentEquals("hit")) ||
                    (userAnswer.contentEquals("kill"))||
                    (userAnswer.contentEquals("burn"))||
                    (userAnswer.contentEquals("open")) ||
                    (userAnswer.contentEquals("light")) ||
                    (userAnswer.contentEquals("untie"))) {

                if ((userAnswer.contentEquals("dig"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommanddig(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'dig <object>");
                    }
                } else if ((userAnswer.contentEquals("unscrew"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandunscrew(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'unscrew <object>");
                    }
                } else if ((userAnswer.contentEquals("hit"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandhit(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'hit <object>");
                    }
                }
                else if ((userAnswer.contentEquals("kill"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandkill(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'kill <object>");
                    }
                }else if ((userAnswer.contentEquals("burn"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandburn(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'burn <object>");
                    }
                }
                else if ((userAnswer.contentEquals("open"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandopen(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'open <object>");
                    }
                }
                else if ((userAnswer.contentEquals("light"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommandlight(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'light <object>");
                    }
                }
                else if ((userAnswer.contentEquals("untie"))) {
                    try {
                        userAnswer = scanner1.next();
                        mission.doCommanduntie(userAnswer);
                    } catch (NoSuchElementException e) {
                        System.out.println("Wrong Usage. 'untie <object>");
                    }
                }
            }

            else if (userAnswer.contentEquals("examine")){
                try {
                    userAnswer = scanner1.next();
                    mission.doCommandexamine(userAnswer);
                }catch (NoSuchElementException e) {
                    System.out.println("Wrong Usage. 'Examine <object>");
                }
            }
            else if (userAnswer.contentEquals("remove")){
                try {
                    userAnswer = scanner1.next();
                    mission.doCommandremove(userAnswer);
                }catch (NoSuchElementException e) {
                    System.out.println("Wrong Usage. 'remove <object>");
                }
            }
            else if (userAnswer.contentEquals("cheat")){
                System.out.println(cheatRiddle.getDescription());
                win = cheatRiddle.doRiddle();
                break;
            }
            else if (userAnswer.contentEquals("objects")){
                mission.doCommandobjects();
            }
            else if (i > 150){
                System.out.println("You took too much time to find Bruce. He is dead. You have lost the game.");
                break;
            }
            else{
                System.out.println("Command not found. Type" +
                        " help " + "to get the list of commands ");
            }

            win = mission.isWin();
            die = mission.isDie();
            if (win){
                break;
            }
            System.out.print("> ");
            scanner = new Scanner(System.in);
            argument = scanner.nextLine();
            scanner1 = new Scanner(argument.toLowerCase()).useDelimiter(" ");
            userAnswer = scanner1.next();
            i++;

        }
        if (win){
            System.out.print("Congratulations !!! You have Won!!!");
        }
        if (die){
            System.out.print("You Have died!!! Good Luck!!! next Time.");
        }
    }
}





