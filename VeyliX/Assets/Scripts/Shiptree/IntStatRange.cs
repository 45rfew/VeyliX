public class IntStatRange
{
    public int min;
    public int max;
    public int current;

    public IntStatRange(int min, int max)
    {
        this.min = min;
        this.max = max;
        current = min;
    }

    public void Upgrade(int tier)
    {
        int value = (max - min) / tier;
        current = UnityEngine.Mathf.Clamp(current + value, min, max);
    }

    public int GetValue()
    {
        return current;
    }
}