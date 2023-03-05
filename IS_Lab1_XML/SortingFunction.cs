namespace IS_Lab1_XML;

public static class SortingFunction
{
    public static int Sort(int x1, int x2)
    {
        if (x1 == x2) return 0;
        if (x1 < x2) return -1;
        return 1;
    }
}