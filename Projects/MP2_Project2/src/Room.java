import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;

public class Room {
    private String name;
    private String description;
    private String newDescription;
    private int numObjs;
    private ArrayList<Object> items = new ArrayList<>();
    private ArrayList<Object> requireObjforKey = new ArrayList<>();
    private boolean key;
    private boolean first;
    private boolean open;
    private boolean newDescriptionaccess = false;
    private boolean riddleSolved;
    private boolean finale;
    private boolean isRiddle;
    private Riddle riddle;

    public Room(String roomName) {
        File filename = new File(roomName + ".txt");
        int requirementKey;
        try {
            Scanner sc = new Scanner(filename).useDelimiter("-");
            name = sc.next();
            description = sc.next();
            numObjs = sc.nextInt();
            items = new ArrayList<>();
            Object temp;
            String sTemp;
            for (int i = 0; i < numObjs; i++) {
                temp = new Object(sc.next());
                items.add(temp);
            }
            sTemp = sc.next();
            newDescription = sTemp;
            sTemp = sc.next();
            if (sTemp.contains("key")) {
                key = true;
                requirementKey = sc.nextInt();
                for (int i = 0; i < requirementKey; i++) {
                    temp = new Object(sc.next());
                    requireObjforKey.add(temp);
                }
            } else
                key = false;
            first = true;
            riddleSolved = true;
            sTemp = sc.next();
            finale = sTemp.contains("yes");

            if (sc.hasNext()) {
                sTemp = sc.next();
                if (sTemp.contains("riddle")) {
                    isRiddle = true;
                    riddle = new Riddle(sTemp);
                    riddleSolved = false;
                } else {
                    isRiddle = false;
                    riddleSolved = true;
                }
            }
            open = !key && riddleSolved;

        } catch (IOException e) {
            System.out.println("Could not open room file" + roomName);
        }
    }

    public void setFirst(boolean first) {
        this.first = first;
    }

    public void setOpentoFalse() {
        this.open = false;
    }

    public void setNumObjs(int numObjs) {
        this.numObjs = numObjs;
    }

    public void setItems(ArrayList<Object> items) {
        this.items = items;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setRiddle(boolean riddle) {
        isRiddle = riddle;
    }

    public int getNumObjs() {
        return numObjs;
    }

    public boolean isKey() {
        return key;
    }

    public boolean isFirst() {
        return first;
    }

    public boolean isOpen() {
        return open;
    }

    public boolean isRiddleSolved() {
        return riddleSolved;
    }

    public boolean isFinale() {
        return finale;
    }

    public boolean isNewDescriptionaccess() {
        return newDescriptionaccess;
    }

    public boolean isRiddle() {
        return isRiddle;
    }

    public String getName() {
        return name;
    }

    public String getRiddle() {
        return riddle.getDescription();
    }

    public String toString() {
        return name + " " + description + "\n";
    }

    public String getDescription() {
        return description;
    }

    public ArrayList<Object> getItems() {
        return items;
    }

    private void setRiddleSolved(boolean riddleSolved) {
        this.riddleSolved = riddleSolved;
    }

    public void doRiddle() {
        setRiddleSolved(riddle.doRiddle());
    }

    public void setAvailabilitytrue(Object tool) {
        int toolIndex = -1;
        for (Object each : items) {
            if (each.getName().contentEquals(tool.getName())) {
                toolIndex = items.indexOf(each);
            }
        }
        items.get(toolIndex).setAvailable(true);
        items.get(toolIndex).setMovable(true);
    }

    public void removeObject(String objectName) {
        int i = 0;
        for (Object object : items) {
            if (object.getName().contentEquals(objectName)) {
                break;
            }
            i++;
        }
        items.remove(i);
    }

    public boolean canOpen(ArrayList<Object> toolbelt) {
        ArrayList<Object> regCopy = new ArrayList<>(requireObjforKey);
        for (int i = 0; i < toolbelt.size(); i++) {
            for (int j = 0; j < regCopy.size(); j++) {
                if (toolbelt.get(i).getName().contains(regCopy.get(j).getName())) {
                    regCopy.remove(j);
                }
            }
        }

        return regCopy.isEmpty();
    }

    public boolean containObject(String objectName) {
        for (Object obj : items) {
            if (obj.getName().contentEquals(objectName)) {
                return true;
            }
        }
        return false;
    }

    public boolean canMoveobject(String objectName) {
        int i = 0;
        if (containObject(objectName)) {
            for (Object object : items) {
                if (object.getName().contentEquals(objectName)) {
                    break;
                }
                i++;
            }
            return items.get(i).isMovable();
        } else {
            return false;
        }
    }

    public String getObjectDescription(String objName) {

        String answer = "Object not found";
        for (int i = 0; i < items.size(); i++) {
            if (items.get(i).getName().contains(objName)) {
                answer = items.get(i).getDescription();
            }
        }
        return answer;
    }

    public String getNewDescription() {
        newDescriptionaccess = true;
        return newDescription;
    }

    public ArrayList<String> getRequireObjforKey() {
        ArrayList<String> keyDescription = new ArrayList<>();
        for (Object obj : requireObjforKey) {
            keyDescription.add(obj.getDescription());
        }
        return keyDescription;
    }
}