using UnityEngine;

public static class RandomUtil
{
    public static bool TryChance(float chance)
    {
        return chance > Random.value;
    }
}