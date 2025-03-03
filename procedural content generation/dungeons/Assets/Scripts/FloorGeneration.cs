using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneration : MonoBehaviour
{
    public int width = 100;
    public int length = 100;
    public float scale = 8f;

    private void Start() {
        generateCaveFloor();
    }

    void generateCaveFloor() {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = createPerlin(terrain.terrainData);
    }

    TerrainData createPerlin(TerrainData terrainData) {
        terrainData.heightmapResolution = width + 1; 
        terrainData.size = new Vector3(width, 3, length);
        float[,] heights = new float[width, length];

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                float xCoord = (float) x / width * scale;
                float zCoord = (float) z / length * scale;
                heights[x, z] = Mathf.PerlinNoise(xCoord, zCoord);
            }
        }
        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }
}
