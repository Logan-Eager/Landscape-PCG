using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public static class Noise
{

    private static FastNoiseLite noise;

    static Noise()
    {
        noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
    }

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        // doesent support fractalFrequency so doing manually
        noise.SetSeed(seed);
        float[,] noiseMap = new float[mapWidth, mapHeight];
    
        if (scale <= 0)
            scale = 0.0001f;
    
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
                    float sampleX = (x - halfWidth) / scale * frequency + (-offset.x * 100); // negated so it moves right when offset.x increases
                    float sampleY = (y - halfHeight) / scale * frequency + (offset.y * 100);
                    float perlinValue = noise.GetNoise(sampleX, sampleY);
    
                    noiseHeight += perlinValue * amplitude;
    
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
    
                noiseMap[x, y] = (noiseHeight + 1) * 0.5f; // normalise to 0-1
            }
        }
    
        return noiseMap;
    }
}
