import java.io.File;
import java.io.IOException;
import java.util.Scanner;

public class Riddle {
    private String description;
    private String answer;

    public Riddle (String riddleName){
        File filename = new File( riddleName + ".txt");
        try  {
            Scanner sc = new Scanner(filename).useDelimiter("-");
            description = sc.next();
            answer = sc.next();

        } catch (IOException e) {
            System.out.println("Could not open riddle file " + riddleName);
        }
    }
    public String getDescription() {
        return description;
    }
    public boolean doRiddle(){
        System.out.print("Enter the answer to the riddle here:\n>");
        Scanner scanner = new Scanner(System.in);
        String userAnswer = scanner.nextLine();
        if (answer.contentEquals(userAnswer.toLowerCase())) {
            System.out.println("Congratulations!!! The answer is correct. Door unlocked");
            return true;
        }
        else {
            System.out.println("Sorry!!! The answer is wrong.");
            return false;
        }
    }
}
