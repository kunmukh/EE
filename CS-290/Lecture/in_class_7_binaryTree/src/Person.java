public class Person implements Comparable<Person> {
    private String name;

    public Person(String n) {
        name = n;
    }

    public int compareTo(Person p) {
        return name.compareTo(p.name);
    }

    public String toString() {
        return "Person: " + name;
    }
}
