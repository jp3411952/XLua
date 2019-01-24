
using UnityEngine;

public class MyClass
{
    public int a;

    public int Test(int d, int b,out int q)
    {
        q = a;
        int c = a + d + b;
        return c;
    }
}
