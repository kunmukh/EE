import java.util.Iterator;

public class BinaryTree<T extends Comparable<? super T>> implements Iterable<T>{

    private BTNode root = null;

    public void add(T item) {
        BTNode newNode = new BTNode();
        newNode.data = item;

        if (root == null) {
            root = newNode;
        } else {
            root.addChild(newNode);
        }
    }

    public int size() {
        if (root == null) {
            return 0;
        } else {
            return root.size();
        }
    }

    public Iterator<T> iterator() {
        return new BTIterator(root);
    }

    private class BTIterator implements Iterator<T> {
        BTNode curr;

        public BTIterator(BTNode root) {
            curr = root;
            // initialize to the leftmost element

            while (curr != null & curr.left != null)
                curr = curr.left;
        }

        @Override
        public boolean hasNext() {
            return curr != null;
        }

        @Override
        public T next() {
            T returnValue = curr.data;

            if (curr.right != null) {
                curr = curr.right;
                while (curr.left != null) {
                    curr = curr.left;
                }
            } else {
                BTNode x = curr.parent;
                while (x != null && x.right == curr) {
                    x = x.parent;
                    curr = curr.parent;
                }
                curr = x;
            }

            return returnValue;
        }
    }

    private class BTNode {
        T data;
        BTNode left = null;
        BTNode right = null;
        BTNode parent = null;

        public void addChild(BTNode newNode) {
            if (newNode.data.compareTo(data) < 0) {
                if (left == null) {
                    left = newNode;
                    newNode.parent = this;
                } else {
                    left.addChild(newNode);
                }
            } else {
                if (right == null) {
                    right = newNode;
                    newNode.parent = this;
                } else {
                    right.addChild(newNode);
                }
            }
        }

        public int size() {
            int size = 1;
            if (left != null) {
                size += left.size();
            }
            if (right != null) {
                size += right.size();
            }
            return size;
        }
    }

}
