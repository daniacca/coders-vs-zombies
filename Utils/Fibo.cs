namespace CodersVsZombies.Utils;

public static class Fibonacci
{
    private static Dictionary<int, int> fibValues = new Dictionary<int, int>{{0, 0}, {1, 1}};

    public static int Calculate(int n)
    {
      if (!fibValues.ContainsKey(n))
        fibValues[n] = Calculate(n - 1) + Calculate(n - 2);

      return fibValues[n];
    }
}