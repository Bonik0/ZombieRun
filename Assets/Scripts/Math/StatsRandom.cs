using UnityEngine;

public static class StatsRandom
{
    public static float Normal(float standardDeviation)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randomStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1, 10)) * Mathf.Sin(2 * Mathf.PI * u2);
        return randomStdNormal * standardDeviation;
    }

    public static float NormalInRange(float standardDeviation, float range)
    {
        if (range < standardDeviation)
        {
            Debug.LogError("Range should be bigger than the standard deviation!");
        }

        float value;
        do
        {
            value = Normal(standardDeviation);
        } while (Mathf.Abs(value) > range);

        return value;
    }

    public static float LogNormal(float mean, float standardDeviation)
    {
        return Mathf.Exp(Normal(standardDeviation)) * mean;
    }
}
