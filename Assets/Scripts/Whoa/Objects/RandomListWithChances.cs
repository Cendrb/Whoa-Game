using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


public class RandomListWithChances<T>
{
    private int totalProbability;
    private Dictionary<T, Range> ranges = new Dictionary<T, Range>();
    private Dictionary<T, int> probabilities = new Dictionary<T, int>();

    public RandomListWithChances()
    {

    }

    public void AddItem(T item, int chance)
    {
        probabilities.Add(item, chance);

        calculateTotalProbability();
    }

    public T GetRandomItem()
    {
        int result = UnityEngine.Random.Range(0, totalProbability);
        return ranges.First<KeyValuePair<T, Range>>(new Func<KeyValuePair<T, Range>, bool>((pair) => pair.Value.IsInRange(result))).Key;
    }

    private void calculateTotalProbability()
    {
        ranges.Clear();
        int counter = 0;
        foreach (KeyValuePair<T, int> pair in probabilities)
        {
            int before = counter;
            counter += pair.Value;

            ranges.Add(pair.Key, new Range(before, counter));
        }
        totalProbability = counter;
    }
}

