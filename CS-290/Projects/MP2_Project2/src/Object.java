import java.io.File;
import java.io.IOException;
import java.util.Scanner;

public class Object {
    private String name;
    private boolean movable;
    private String description;
    private boolean available;

    Object(String nameObj){
        File filename = new File( nameObj + ".txt");
        try  {
            Scanner sc = new Scanner(filename).useDelimiter("-");
            name = sc.next();
            description  = sc.next();
            String line = sc.next();
            movable = line.contains("movable");
            available = true;
        } catch (IOException e) {
            throw new ArithmeticException();
        }
    }

    public void setAvailable(boolean available) {
        this.available = available;    }
    public void setMovable(boolean movable) {
        this.movable = movable;
    }

    public boolean isMovable() {
        return movable;
    }
    public String getName() {
        return name;
    }
    public String getDescription() {
        return description;
    }
    public String toString(){
        return name;
    }

}
