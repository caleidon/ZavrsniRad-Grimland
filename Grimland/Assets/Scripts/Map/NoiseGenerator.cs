using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;


public static class NoiseGenerator
{
    public static float[,] GenerateTerrainNoiseMap(int mapWidth, int mapHeight)
    {
        const int octaves = 4;
        float noiseScale = UnityRandom.Range(35f, 45f);
        float persistance = UnityRandom.Range(0.37f, 0.43f);
        float lacunarity = UnityRandom.Range(2.0f, 2.3f);
        int seed = UnityRandom.Range(int.MinValue, int.MaxValue);

        return GenerateNoiseMap(mapWidth, mapHeight, noiseScale, persistance, lacunarity, seed, octaves);
    }

    // TODO: fix YAML basetile range (some must start from zero)
    public static float[,] GenerateSoilNoiseMap(int mapWidth, int mapHeight)
    {
        int octaves = (int)UnityRandom.Range(2f, 3f);
        float noiseScale = UnityRandom.Range(25f, 30f);
        float persistance = UnityRandom.Range(0.1f, 0.12f);
        float lacunarity = UnityRandom.Range(6.0f, 8.0f);
        int seed = UnityRandom.Range(int.MinValue, int.MaxValue);

        return GenerateNoiseMap(mapWidth, mapHeight, noiseScale, persistance, lacunarity, seed, octaves);
    }

    private static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float noiseScale, float persistance,
        float lacunarity, int seed, int octaves)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        Random randomNumber = new Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0;
            i < octaves;
            i++)
        {
            float offsetX = randomNumber.Next(-100000, 100000);
            float offsetY = randomNumber.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}