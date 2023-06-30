using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeightedList<T> : IEnumerable<WeightedList<T>.Item>
{
    [SerializeField] private List<Item> list = new();
    [SerializeField] private float sum; // for editor purposes

    [System.Serializable]
    public struct Item
    {
        [SerializeField] private float weight;
        public float Weight => weight;
        [SerializeField] private T value;
        public T Value => value;


        public Item(int weight, T value)
        {
            this.weight = weight;
            this.value = value;
        }
    }

    public int ItemsCount => list.Count;
    public IReadOnlyList<Item> List => list;


    public T GetRandomValue()
    {
        var index = GetRandomWeightIndex();

        return list[index].Value;
    }

    public T GetRandomValue(System.Func<float, float, float> randomGenerator)
    {
        var index = GetRandomWeightIndex(randomGenerator);

        return list[index].Value;
    }

    public int GetRandomWeightIndex()
    {
        return GetRandomWeightIndex(Random.Range);
    }

    public int GetRandomWeightIndex(System.Func<float, float, float> randomGenerator)
    {
        var sum = 0f;
        foreach (var item in list)
        {
            sum += item.Weight;
        }
        var rnd = randomGenerator(0, sum);
        for (int i = 0; i < list.Count; i++)
        {
            if (rnd <= list[i].Weight) return i;
            rnd -= list[i].Weight;
        }
        return -1;
    }


    public void Add(int weight, T value)
    {
        list.Add(new Item(weight, value));
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }
}

[System.Serializable]
public class CustomWeightedList<T> : IEnumerable<T>
{
    [SerializeField] private List<T> list = new List<T>();

    public int ItemsCount => list.Count;
    public IReadOnlyList<T> List => list;


    public CustomWeightedList(List<T> list)
    {
        this.list = list;
    }


    public T GetRandomValue(System.Func<T, float> weightSelector)
    {
        var index = GetRandomWeightIndex(weightSelector);

        return list[index];
    }

    public T GetRandomValue(System.Func<T, float> weightSelector, System.Func<float, float, float> randomGenerator)
    {
        var index = GetRandomWeightIndex(weightSelector, randomGenerator);

        return list[index];
    }

    public int GetRandomWeightIndex(System.Func<T, float> weightSelector)
    {
        return GetRandomWeightIndex(weightSelector, Random.Range);
    }

    public int GetRandomWeightIndex(System.Func<T, float> weightSelector, System.Func<float, float, float> randomGenerator)
    {
        return WeightedListUtil.GetRandomWeightIndex(list.Select(weightSelector), randomGenerator);
    }


    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }
}

public static class WeightedListUtil
{
    public static int GetRandomWeightIndex(IEnumerable<int> weights, System.Func<int, int, int> randomGenerator)
    {
        int sum = weights.Sum();
        int count = weights.Count();
        int rnd = randomGenerator(0, sum + 1);
        for (int i = 0; i < count; i++)
        {
            var w = weights.ElementAt(i);
            if (rnd <= w) return i;
            rnd -= w;
        }
        return -1;
    }

    public static int GetRandomWeightIndex(IEnumerable<float> weights, System.Func<float, float, float> randomGenerator)
    {
        var sum = weights.Sum();
        int count = weights.Count();
        var rnd = randomGenerator(0f, sum);
        for (int i = 0; i < count; i++)
        {
            var w = weights.ElementAt(i);
            if (rnd <= w) return i;
            rnd -= w;
        }
        return -1;
    }
}