import org.junit.jupiter.api.Test;

import java.io.*;
import java.util.Scanner;

import static java.lang.System.out;

public class FileTest {
    @Test

    public void fileTest() {
        File f = new File("text.txt");
        PrintWriter out;
        try {
            out = new PrintWriter("output.txt");
            try {
                out.write("foo\n");
                out.println("This sucks");

                return;
            } finally {
                if (out != null)
                    out.close();
            }
            } catch(IOException e){
                System.out.println("Could not open file");
            }
        }
        @Test
    public void testInput() {
            Scanner s = new Scanner(System.in);

            Scanner fs = new Scanner("input.txt");
            try {
                while (fs.hasNextInt()) {
                    int x = fs.nextInt();
                }
            } finally {
                fs.close();
            }
        }
    }


