import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class Gameplay {
    private String [] room = {"room1", "room2", "room3" , "room4"
            , "room5", "room6" , "room7", "room8" ,"room9",
             "room10"};
    private ArrayList<Object> toolbelt =  new ArrayList<>();
    private ArrayList<Room> environment = new ArrayList<>();
    private ArrayList<Direction> wayPoint = new ArrayList<>();
    private Map<String, String> keyHolder = new HashMap<String, String>();
    private boolean win = false;
    private boolean die = false;
    private boolean bruce_untie = false;
    private Room currentRoom;

    public Gameplay() {
        Room r_temp;
        Direction d_temp;
        for (String aRoom : room) {
            r_temp = new Room(aRoom);
            d_temp = new Direction(aRoom);
            environment.add(r_temp);
            wayPoint.add(d_temp);
        }
        currentRoom = environment.get(0);
        currentRoom.setOpentoFalse();
        System.out.println(currentRoom.getDescription());
        keyholderMaker();
    }

    public void doCommandgo (String command){
        int roomIndex = environment.indexOf(currentRoom);
        String destination;
        Room temp;
        if (wayPoint.get(roomIndex).canGodirection(command)) {
            destination = wayPoint.get(roomIndex).goDirection(command);
            for (Room r : environment) {
                if (r.getName().contains(destination)) {
                    roomIndex = environment.indexOf(r);
                }
            }
        }
        else{
            System.out.println("The direction is wrong or the direction doesnot exist");
            return;
        }
        temp = environment.get(roomIndex);

        if (!temp.isOpen()) {
            if (temp.isKey() && temp.isRiddle()) {
                if (temp.canOpen(toolbelt)) {
                    if (temp.isRiddle()) {
                        System.out.println("There is a riddle here. " +
                                "You have to answer it to unlock the door:");
                        System.out.println("The Riddle is: " + temp.getRiddle());
                        temp.doRiddle();
                        if (temp.isFinale()) {
                            if (!temp.isRiddleSolved()) {
                                currentRoom = environment.get(0);
                                currentRoom.setFirst(false);
                                System.out.println("This mistake will cost you dearly.");
                                System.out.println("You opened a trapdoor " +
                                        "and you are in " + currentRoom.getName());
                            } else if (bruce_untie) {
                                System.out.println(temp.getDescription());
                                win = true;
                            } else {
                                temp.setRiddle(false);
                                System.out.println("You have answered correctly but, you forgot to do the most important " +
                                        "thing, that is to release Bruce by untiring him." +
                                        "You cannot leave this room without Bruce.");
                            }
                        }
                        if (temp.isRiddleSolved() && !temp.isFinale()) {
                            currentRoom = temp;
                            currentRoom.setRiddle(false);
                            System.out.println(currentRoom.getName());
                            System.out.println(currentRoom.getDescription());
                        }
                        else {
                            if (!temp.isFinale()) {
                                System.out.println("Sorry could not solve the riddle." +
                                        " The door is locked.oo");
                            }
                        }

                    } else {
                        System.out.println("The key has opened the door");
                        currentRoom = temp;
                        currentRoomchnage();
                    }
                } else {
                    System.out.println("Sorry Cannot Go That Direction. " +
                            "You have the door is locked. " +
                            "You dont have all the required materials." + " You NEED:" + temp.getRequireObjforKey());
                }
            } else {
                if (!temp.isRiddle()) {
                    currentRoom = temp;
                    currentRoomchnage();
                }
                else {
                    System.out.println("There is a riddle here. " +
                            "You have to answer it to unlock the door:");
                    System.out.println("The Riddle is: " + temp.getRiddle());
                    temp.doRiddle();
                    if (temp.isRiddleSolved() && !temp.isFinale()) {
                        currentRoom = temp;
                        currentRoom.setRiddle(false);
                        System.out.println(currentRoom.getName());
                        System.out.println(currentRoom.getDescription());
                    }
                    else {
                        System.out.println("Sorry could not solve the riddle." +
                                " The door is locked.");
                    }
                }
            }
        }
        else {
            currentRoom = temp;
            currentRoomchnage();
        }
    }
    public void doCommandLook (){
        System.out.println(currentRoom.getDescription());
    }
    public void doCommandLookobj(String userAnswer) {
        System.out.println(currentRoom.getObjectDescription(userAnswer));
    }
    public void doCommandInventory() {
        StringBuilder toollist = new StringBuilder();
        for (Object tool : toolbelt){
            if (!tool.getName().contains("key") &&
                    !tool.getName().contains("medallion")) {
                toollist.append(tool.toString()).append(" ");
            }
            else if (tool.getName().contains("medallion")) {
                if (tool.getName().contains("medallion_1"))
                    toollist.append("red medallion").append(" ");
                else
                    toollist.append("blue medallion").append(" ");
            }
            else if (tool.getName().contains("key"))
                toollist.append(tool.getDescription()).append(" ");
            else
                toollist.append(tool.getName()).append(" ");
        }
        if (toollist.length() == 0)
            System.out.println("The toolbelt is empty. Nothing to show.");
        else
            System.out.println(toollist);
    }
    public void doCommandget(String userAnswer) {
        String temp = userAnswer;
        userAnswer = getKeyfileName(userAnswer);
        if (userAnswer == null)
            userAnswer = temp;
        boolean found = false;
        String newDescription;
        try {
                Object obj = new Object(userAnswer);
                if (currentRoom.containObject(userAnswer)
                    && (currentRoom.canMoveobject(userAnswer))) {
                if (toolbelt.size() != 7) {
                    toolbelt.add(obj);
                    System.out.println("Item successfully put into toolbelt.");
                    currentRoom.removeObject(userAnswer);
                    found = true;
                    if(userAnswer.contains("medallion")){
                        if (userAnswer.contains("medallion_2"))
                            userAnswer = "blue medallion";
                        else
                            userAnswer = "red medallion";
                    }
                    newDescription = currentRoom.getDescription().replace(userAnswer, "something, but there is nothing now");
                    currentRoom.setDescription(newDescription);
                }
                else {
                    System.out.println("Item found in the room, but toolbelt is full. " +  "Remove an item from the tool belt and try to add again");
                    found = true;
                    //currentRoom.getItems().get(currentRoom.getItems().indexOf(obj)).setAvailable(true);
                }
            }
                else if (currentRoom.containObject(userAnswer)
                        && !currentRoom.canMoveobject(userAnswer)){
                    System.out.println("Item cannot be moved from the location.");
                    found = true;
                }

                if (!found){
                        System.out.println("The item cannot be found in the room.");
                    }

            }catch (Exception e){
                System.out.println("Object not available to create, please spell correctly");
            }
    }
    public void doCommandremove(String userAnswer) {
        String temp = userAnswer;
        userAnswer = getKeyfileName(userAnswer);
        if (userAnswer == null)
            userAnswer = temp;
        int toolIndex = -1;
        ArrayList<Object> roomItems;
        int roomItemnum;

        for (Object tool : toolbelt){
            if (tool.getName().contains(userAnswer)){
                toolIndex = toolbelt.indexOf(tool);
            }
        }
        if (toolIndex < 0) {
            System.out.println("The tool not found in toolbelt");
        }
        else {
            System.out.println("The tool successfully removed");
            roomItems = new ArrayList<>(currentRoom.getItems());
            roomItemnum = currentRoom.getNumObjs();
            roomItems.add(toolbelt.get(toolIndex));
            roomItems.get(roomItems.indexOf(toolbelt.get(toolIndex)))
                    .setMovable(true);
            roomItemnum += 1;
            toolbelt.remove(toolIndex);
            currentRoom.setItems(roomItems);
            currentRoom.setNumObjs(roomItemnum);
            currentRoom.setDescription(currentRoom.getDescription().concat(" There is a " + userAnswer + " laying there now."));
            if (currentRoom.isNewDescriptionaccess())
                currentRoom.setDescription(currentRoom.getNewDescription().concat(" There is a " + userAnswer + " laying there now."));
        }


    }
    public void doCommanddig(String mud){
        doCommandaction("shovel",mud,"key1", "dig");
    }
    public void doCommandunscrew(String userAnswer) {
        doCommandaction("screwdriver",userAnswer,"key2", "unscrew");
    }
    public void doCommandhit(String userAnswer) {
        doCommandaction("hammar",userAnswer,"key3", "hit");
    }
    private void doCommandaction(String originalEquipment, String originalEnvironment, String key, String action){
        boolean workdone = false;
        boolean mudFound = false;
        try {
            Object equipment = new Object(originalEquipment);
            Object environment = new Object(originalEnvironment);
            Object newKey = new Object(key);
            ArrayList<Object> temp_obj = currentRoom.getItems();

            for (Object aToolbelt : toolbelt) {
                if (aToolbelt.getName().contentEquals(equipment.getName())) {
                    for (int j = 0; j < temp_obj.size(); j++) {
                        if (temp_obj.get(j).getName().contentEquals(environment.getName())) {
                            temp_obj.remove(j);
                            System.out.println("The " + originalEnvironment + " has been removed you have found a " + newKey.getDescription() + ".");
                            mudFound = true;
                        }
                    }
                }
            }
            for (int j = 0; j < temp_obj.size(); j++) {
                if ((temp_obj.get(j).getName().contentEquals(newKey.getName())) && (mudFound)) {
                    currentRoom.setAvailabilitytrue(temp_obj.get(j));
                    currentRoom.setItems(temp_obj);
                    currentRoom.setNumObjs(currentRoom.getNumObjs() - 1);
                    currentRoom.setDescription(currentRoom.getNewDescription());
                    workdone = true;
                }
            }
            if (!workdone) {
                System.out.println("Cannot " + action + " without a " + originalEquipment+ " or maybe there is no " + originalEnvironment + " to " + action + " .");
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommandkill(String userAnswer) {
        try {
            boolean workdone = false;
            boolean snakeFound = false;
            Object knief = new Object("knief");
            Object snake = new Object(userAnswer);
            Object key4 = new Object("key4");
            ArrayList<Object> temp_obj = currentRoom.getItems();
            for (int i = 0; i < toolbelt.size(); i++) {
                if (toolbelt.get(i).getName().contentEquals(knief.getName())) {
                    for (int j = 0; j < temp_obj.size(); j++) {
                        if (temp_obj.get(j).getName().contentEquals(snake.getName())) {
                            temp_obj.remove(j);
                            System.out.println("You have killed Penguin and you have uncovered a diamond Key");
                            snakeFound = true;
                        }
                    }
                }
            }
            for (int j = 0; j < temp_obj.size(); j++) {
                if ((temp_obj.get(j).getName().contentEquals(key4.getName()) && (snakeFound))) {
                    currentRoom.setAvailabilitytrue(temp_obj.get(j));
                    currentRoom.setItems(temp_obj);
                    currentRoom.setNumObjs(currentRoom.getNumObjs() - 1);
                    currentRoom.setDescription(currentRoom.getNewDescription());
                    workdone = true;
                }
            }
            if (!workdone && !snakeFound) {
                System.out.println("Cannot kill a Penguin without a knief or maybe the Penguin is not here.");
            } else if (!workdone) {
                System.out.println("Cannot kill a Penguin without a knief and he has killed you.");
                die = true;
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommandburn(String userAnswer) {
        try {
            boolean workdone = false;
            boolean shackFound = false;
            Object torch = new Object("torch");
            Object shack = new Object(userAnswer);
            Object key3 = new Object("golden_box");
            Object key4 = new Object("key5");
            ArrayList<Object> temp_obj = currentRoom.getItems();
            for (Object aToolbelt : toolbelt) {
                if (aToolbelt.getName().contentEquals(torch.getName())) {
                    for (int j = 0; j < temp_obj.size(); j++) {
                        if (temp_obj.get(j).getName().contentEquals(shack.getName())) {
                            temp_obj.remove(j);
                            System.out.println("The burn of the shack has revealed a golden box and silver key.");
                            shackFound = true;
                        }
                    }
                }
            }
            for (int j = 0; j < temp_obj.size(); j++) {
                if ((temp_obj.get(j).getName().contentEquals(key3.getName()) && (shackFound))) {
                    currentRoom.setAvailabilitytrue(temp_obj.get(j));
                    currentRoom.setItems(temp_obj);
                    currentRoom.setNumObjs(currentRoom.getNumObjs() - 1);
                    currentRoom.setDescription(currentRoom.getNewDescription());
                }
            }
            for (int j = 0; j < temp_obj.size(); j++) {
                if ((temp_obj.get(j).getName().contentEquals(key4.getName()) && (shackFound))) {
                    currentRoom.setAvailabilitytrue(temp_obj.get(j));
                    currentRoom.setItems(temp_obj);
                    currentRoom.setNumObjs(currentRoom.getNumObjs() - 1);
                    currentRoom.setDescription(currentRoom.getNewDescription());
                    workdone = true;
                }
            }

            if (!workdone) {
                System.out.println("Cannot burn a shack without a torch or maybe there is no shack to burn.");
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommandopen(String userAnswer) {
        try {
            boolean workdone = false;
            boolean gboxFound = false;
            boolean foundKey1 = false;
            boolean foundKey3 = false;
            Object key1 = new Object("key1");
            Object key3 = new Object("key3");
            Object g_box = new Object(userAnswer);
            Object key6 = new Object("key6");
            ArrayList<Object> temp_obj = currentRoom.getItems();
            for (int i = 0; i < toolbelt.size(); i++) {
                if (toolbelt.get(i).getName().contentEquals(key1.getName())) {
                    for (int j = 0; j < temp_obj.size(); j++) {
                        if (temp_obj.get(j).getName().contentEquals(g_box.getName())) {
                            currentRoom.setDescription(currentRoom.getNewDescription());
                            foundKey1 = true;
                        }
                    }
                }
            }
            for (int i = 0; i < toolbelt.size(); i++) {
                if (toolbelt.get(i).getName().contentEquals(key3.getName())) {
                    for (int j = 0; j < temp_obj.size(); j++) {
                        if (temp_obj.get(j).getName().contentEquals(g_box.getName())) {
                            foundKey3 = true;
                        }
                    }
                }
            }
            if (foundKey1 && foundKey3)
                gboxFound = true;
            else if (foundKey1)
                System.out.println("Iron Key has been found. But you still need titanium Key to open door.");
            else if (foundKey3)
                System.out.println("Titanium has been found. But you still need iron key to open door.");
            else {
                System.out.println("Cannot open the golden box without the iron key & titanium key");
                workdone = true;
            }

            for (int j = 0; j < temp_obj.size(); j++) {
                if ((temp_obj.get(j).getName().contentEquals(key6.getName()) && (gboxFound))) {
                    currentRoom.setAvailabilitytrue(temp_obj.get(j));
                    currentRoom.setItems(temp_obj);
                    currentRoom.setNumObjs(currentRoom.getNumObjs() - 1);
                    currentRoom.setDescription(currentRoom.getNewDescription());
                    workdone = true;
                    System.out.println("The Golden Box has revealed Golden key.");
                }
            }
            if (!workdone) {
                System.out.println("No such object found in this room");
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommandlight(String userAnswer) {
        try {
            boolean workdone = false;
            Object torch = new Object(userAnswer);

            for (int i = 0; i < toolbelt.size(); i++) {
                if (toolbelt.get(i).getName().contentEquals(torch.getName())) {
                    System.out.println("The torch has been lit");
                    currentRoom.setDescription(currentRoom.getNewDescription());
                    workdone = true;
                }
            }
            if (!workdone) {
                System.out.println("You can only light a torch.");
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommanduntie(String userAnswer) {
        try {
            boolean workdone = false;
            Object bruce = new Object(userAnswer);
            ArrayList<Object> temp_obj = currentRoom.getItems();
            for (int j = 0; j < temp_obj.size(); j++) {
                if (temp_obj.get(j).getName().contentEquals(bruce.getName())) {
                    System.out.println("Bruce has been untied.");
                    temp_obj.get(j).setAvailable(true);
                    temp_obj.get(j).setMovable(true);
                    workdone = true;
                    bruce_untie = true;
                    currentRoom.setDescription(currentRoom.getNewDescription());
                }
            }
            if (!workdone) {
                System.out.println("There is no Bruce to untie");
            }
        }catch (Exception e){
            System.out.println("This object cannot be found in this room.");
        }
    }
    public void doCommandexamine(String userAnswer) {
        boolean found = false;
        for (Object tool : toolbelt){
            if (tool.getName().contains(userAnswer)) {
                found = true;
                System.out.println(tool.getDescription());
            }
        }
        if (!found){
            System.out.println("Sorry, tool not in the toolbelt to examine.");
        }

    }
    public void doCommandobjects() {
        StringBuilder s = new StringBuilder();
        for (Object obj: currentRoom.getItems()){
            if (!obj.getName().contains("key") &&
                    !obj.getName().contains("medallion")) {
                s.append(obj.getName()).append("\n");
            }
            else if (obj.getName().contains("medallion")) {
                if (obj.getName().contains("medallion_1"))
                    s.append("red medallion\n");
                else
                    s.append("blue medallion\n");
            }
            else
               s.append(obj.getDescription()).append("\n");
        }
        System.out.print(s);
    }

    public boolean isDie() {
        return die;
    }
    public boolean isWin() {
        return win;
    }
    private String getKeyfileName (String userName){
        return keyHolder.get(userName);
    }

    private void currentRoomchnage(){
        if (currentRoom.isFirst()) {
            System.out.println(currentRoom.getName());
            System.out.println(currentRoom.getDescription());
            currentRoom.setFirst(false);
            currentRoom.setOpentoFalse();
        } else {
            System.out.println(currentRoom.getName());
            currentRoom.setOpentoFalse();
        }
    }
    private void keyholderMaker() {
        keyHolder.put("iron", "key1" );
        keyHolder.put("platinum", "key2" );
        keyHolder.put("titanium", "key3" );
        keyHolder.put("diamond", "key4" );
        keyHolder.put("silver", "key5" );
        keyHolder.put("golden", "key6");
        keyHolder.put("blue", "medallion_2");
        keyHolder.put("red", "medallion_1");
    }

}
