import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;

public class Direction {
    private ArrayList<String> doorway = new ArrayList<>();
    private ArrayList<String> direction = new ArrayList<>();
    private ArrayList<String> destination = new ArrayList<>();
    private String [] acceptDirections = {"north", "south", "east" , "west", "northeast", "northwest" , "southeast" , "southwest", "n", "s", "e" , "w", "ne" , "nw" , "se", "sw" };

    public Direction(String directionName){
        File filename = new File( directionName + "_destination.txt");
        try  {
            Scanner sc = new Scanner(filename).useDelimiter("-");
            String sTemp;
            int numDirection = sc.nextInt();
            for (int i = 0; i < numDirection; i++){
                sTemp = sc.next();
                doorway.add(sTemp);
                doorway.add(sTemp);
                sTemp = sc.next();
                direction.add(sTemp);
                sTemp = sc.next();
                direction.add(sTemp);
                sTemp = sc.next();
                destination.add(sTemp);
                destination.add(sTemp);
            }

        } catch (IOException e) {
            System.out.println("Could not open file direction " + directionName);
        }
    }
    public boolean canGodirection (String way){
        boolean right_command = false;
        for (String acceptDirection : acceptDirections) {
            if (acceptDirection.contentEquals(way)) {
                right_command = true;
            }
        }
        int indexWay;
        if (direction.contains(way) && right_command){
            indexWay = direction.indexOf(way);
            return (doorway.get(indexWay).contains("exit"))
                    || (doorway.get(indexWay).contains("both"));
        }
        else {
            return false;
        }
    }
    public String goDirection (String way){
        return destination.get(direction.indexOf(way));
    }

}
