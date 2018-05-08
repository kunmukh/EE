public class Sphere {
    private double radius;
    private double x;
    private double y;
    private double z;


    public Sphere() {
        this(1.0);
    }

    public Sphere(double radius) {
        this(radius, 0, 0, 0);
    }

    public Sphere(double x, double y, double z) {
        this(1.0, x, y, z);
    }

    public Sphere(double radius, double x, double y, double z) {
        if (radius < 0.0) {
            throw new RuntimeException();
        }
        this.x = x;
        this.y = y;
        this.z = z;
        this.radius = radius;
    }

    public double getRadius() {
        return radius;
    }

    public double getX() {
        return x;
    }

    public double getY() {
        return y;
    }

    public double getZ() {
        return z;
    }

    public boolean intersect(Sphere s2) {
        double radiusSum = radius + s2.radius;
        double distance = Math.sqrt((x-s2.x) * (x-s2.x) + (y-s2.y) * (y-s2.y) + (z-s2.z) * (z-s2.z));

        return radiusSum > distance;

    }
}
