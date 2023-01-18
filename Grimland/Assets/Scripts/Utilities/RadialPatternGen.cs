using System.Collections.Generic;
using UnityEngine;

public static class RadialPatternGen
{
    private static readonly Vector3Int[] RadialPatternArray = new Vector3Int[10000];
    private static readonly float[] RadialPatternRadii = new float[10000];

    static RadialPatternGen()
    {
        InitializeRadialPattern();
    }

    private static void InitializeRadialPattern()
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        for (int x = -60; x < 60; x++)
        {
            for (int y = -60; y < 60; y++)
            {
                locations.Add(new Vector3Int(x, y, 0));
            }
        }

        locations.Sort(delegate(Vector3Int first, Vector3Int second)
        {
            float firstValue = HorizontalLengthSquared(first);
            float secondValue = HorizontalLengthSquared(second);
            if (firstValue < secondValue)
            {
                return -1;
            }

            return firstValue == secondValue ? 0 : 1;
        });
        for (int k = 0; k < 10000; k++)
        {
            RadialPatternArray[k] = locations[k];
            RadialPatternRadii[k] = HorizontalLength(locations[k]);
        }
    }

    private static int HorizontalLengthSquared(Vector3Int vector3Int)
    {
        return vector3Int.x * vector3Int.x + vector3Int.y * vector3Int.y;
    }

    private static float HorizontalLength(Vector3Int vector3Int)
    {
        return Mathf.Sqrt(vector3Int.x * vector3Int.x + vector3Int.y * vector3Int.y);
    }

    public static IEnumerable<Vector3Int> NodesInRadius(Vector3Int center, float radius, bool useCenter)
    {
        int nodes = NodesInRadius(radius);
        int counter;
        for (int i = useCenter ? 0 : 1; i < nodes; i = counter + 1)
        {
            yield return RadialPatternArray[i] + center;
            counter = i;
        }
    }

    public static IEnumerable<Vector3Int> NodesInRadius(Vector3Int center, float minRadius, float maxRadius)
    {
        int nodes = NodesInRadius(maxRadius);
        int counter;
        for (int i = 0; i < nodes; i = counter + 1)
        {
            if (HorizontalLength(RadialPatternArray[i]) >= minRadius)
            {
                yield return RadialPatternArray[i] + center;
            }

            counter = i;
        }
    }

    private static int NodesInRadius(float radius)
    {
        if (radius >= 10000)
        {
            return 10000;
        }

        float range = radius + float.Epsilon;
        for (int i = 0; i < 10000; i++)
        {
            if (RadialPatternRadii[i] > range)
            {
                return i;
            }
        }

        return 10000;
    }
}