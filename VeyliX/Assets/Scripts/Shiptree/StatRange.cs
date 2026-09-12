using UnityEngine;
public class StatRange
{
    public float min;
    public float max;
    public float current;

    public StatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
        current = min;
    }

    public void Upgrade(float tier)
    {
        float value = (max - min) / tier;
        current = Mathf.Clamp(current + value, min, max);
    }
    public float GetValue()
    {
        return current;
    }
}