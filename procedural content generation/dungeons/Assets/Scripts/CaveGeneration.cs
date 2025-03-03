using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGeneration : MonoBehaviour
{
    public GameObject cubePrefab;  
    public GameObject fillPrefab;
    public GameObject chestPrefab;
    public GameObject pebblePrefab;
    public int seed = 12;
    int length = 100;
    int width = 100;
    bool[,] cellmap;
    bool[,] visited;
    List<int> componentSizes = new List<int>();

    void Start()
    {
        Random.InitState(seed);
        createCaves();
        labelConnectedComponents();
        placeTreasureChests();
        createRockPiles();
    }

    void createCaves() {
        float filledStartProb = 0.5f;
        cellmap = new bool[width, length];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                if (filledStartProb > Random.value) {
                    cellmap[x, z] = true;
                }
            }
        }
        for (int i = 0; i < 15; i++) {
		    cellmap = performCellularAutomata(cellmap);
	    }
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                if (cellmap[x, z] == true) {
                    Vector3 position = new Vector3(x * 0.75f, 1, z * 0.75f);
                    Instantiate(cubePrefab, position, Quaternion.identity);
                }
            }
        }
    }

    int neighborStatus(bool[,] map, int x, int z) {
        int filledNeighbors = 0;
        for(int i = x - 1; i <= x + 1; i++){
            for(int j = z - 1; j <= z + 1; j++){
                if(i == x && j == z){
                }
                else if (i < 0 || j < 0 || i >= width || j >= length) {
                    filledNeighbors++;
                }
                else if(map[i,j]){
                    filledNeighbors++;
                }
            }
        }
        return filledNeighbors;
    }
    
    bool[,] performCellularAutomata(bool[,] prevMap) {
        bool[,] nextMap = new bool[width, length];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                int filledNeighbors = neighborStatus(prevMap, x, z);
                if (prevMap[x,z]) {
                    if (filledNeighbors >= 4) {
                        nextMap[x, z] = true;
                    } else {
                        nextMap[x, z] = false;
                    }
                } else {
                    if (filledNeighbors >= 5) {
                        nextMap[x, z] = true;
                    } else {
                        nextMap[x, z] = false;
                    }
                }
            }
        }
        return nextMap;
    }


    void labelConnectedComponents() {
        visited = new bool[width, length];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                if (!visited[x, z] && !cellmap[x, z]) {
                    List<Vector2Int> componentCells = new List<Vector2Int>();
                    int componentSize = flood(x, z, componentCells);
                    if (componentSize <= 1000) {
                        fillComponent(componentCells);
                    } else {
                        componentSizes.Add(componentSize);
                    }
                }
            }
        }
        // Debug.Log("Total components: " + componentSizes.Count);
        // for (int i = 0; i < componentSizes.Count; i++) {
        //     Debug.Log("Component " + (i) + " size: " + componentSizes[i]);
        // }
    }

    int flood(int xInit, int zInit, List<Vector2Int> componentCells) {
        int componentSize = 0;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(xInit, zInit));
        visited[xInit, zInit] = true;
        Vector2Int[] directions = { 
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };
        while (queue.Count > 0) {
            Vector2Int curr = queue.Dequeue();
            componentCells.Add(curr);
            componentSize++;
            foreach (Vector2Int direction in directions) {
                int xNext = curr.x + direction.x;
                int zNext = curr.y + direction.y;
                if (xNext >= 0 && xNext < width && zNext >= 0 && zNext < length) {
                    if (!visited[xNext, zNext] && !cellmap[xNext, zNext]) {
                        queue.Enqueue(new Vector2Int(xNext, zNext));
                        visited[xNext, zNext] = true;
                    }
                }
            }
        }
        return componentSize;
    }

    void fillComponent(List<Vector2Int> componentCells) {
        foreach (Vector2Int cell in componentCells) {
            cellmap[cell.x, cell.y] = true;
            Vector3 position = new Vector3(cell.x * 0.75f, 1, cell.y * 0.75f);
            Instantiate(fillPrefab, position, Quaternion.identity); 
        }
    }

    void placeTreasureChests() {
        int chestCount = 0;
        while (chestCount < 15) {
            int xRand = Random.Range(1, 99);
            int zRand = Random.Range(1, 99);
            bool cellAndAllNeighborsEmpty = (cellmap[xRand - 1, zRand + 1] == false && 
                                            cellmap[xRand, zRand + 1] == false &&
                                            cellmap[xRand + 1, zRand + 1] == false && 

                                            cellmap[xRand - 1, zRand] == false && 
                                            cellmap[xRand, zRand] == false &&
                                            cellmap[xRand + 1, zRand] == false && 

                                            cellmap[xRand - 1, zRand - 1] == false && 
                                            cellmap[xRand, zRand - 1] == false &&
                                            cellmap[xRand + 1, zRand - 1] == false);
            if (cellAndAllNeighborsEmpty) {
                Vector3 position = new Vector3(xRand * 0.75f, 1, zRand * 0.75f);
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                Instantiate(chestPrefab, position, randomRotation);
                chestCount++;
                cellmap[xRand - 1, zRand + 1] = true;
                cellmap[xRand, zRand + 1] = true;
                cellmap[xRand + 1, zRand + 1] = true; 

                cellmap[xRand - 1, zRand] = true;
                cellmap[xRand, zRand] = true;
                cellmap[xRand + 1, zRand] = true;

                cellmap[xRand - 1, zRand - 1] = true;
                cellmap[xRand, zRand - 1] = true;
                cellmap[xRand + 1, zRand - 1] = true;
            }
        }
    }  

    void createRockPiles() {
        int pileCount = 0;
        while (pileCount < 5) {
            int xRand = Random.Range(1, 99);
            int zRand = Random.Range(1, 99);
            bool cellAndAllNeighborsEmpty = (cellmap[xRand - 1, zRand + 1] == false && 
                                            cellmap[xRand, zRand + 1] == false &&
                                            cellmap[xRand + 1, zRand + 1] == false && 

                                            cellmap[xRand - 1, zRand] == false && 
                                            cellmap[xRand, zRand] == false &&
                                            cellmap[xRand + 1, zRand] == false && 

                                            cellmap[xRand - 1, zRand - 1] == false && 
                                            cellmap[xRand, zRand - 1] == false &&
                                            cellmap[xRand + 1, zRand - 1] == false);
            if (cellAndAllNeighborsEmpty) {
                for (int i = 0; i < 10; i++) {
                    Vector3 position = new Vector3(xRand * 0.75f + Random.Range(-1, 2) * 0.2f, i * 0.5f, zRand * 0.75f + Random.Range(-1, 2) * 0.2f);
                    Instantiate(pebblePrefab, position, Quaternion.identity);
                } 
                pileCount++;
            }
        }
    }

    

    void Update() {
        
    }
}
