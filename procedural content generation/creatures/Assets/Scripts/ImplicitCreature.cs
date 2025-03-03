using UnityEngine;
using System.Collections.Generic;

public class ImplicitCreature : Implicit
{
    public int mainRandomSeed = 16;

    private int[] creatureSeeds = new int[5];
    private int creatureCounter = 0;

    public GameObject legType1;
    public GameObject legType2;
    public GameObject legType3;
    public GameObject eyeBall;
    public GameObject antennae;
    public Material scales;

    private GameObject[] allAntennae = new GameObject[10];
    private int antennaeCount = 0;
    private GameObject[] allWings = new GameObject[10];
    private int wingCount = 0;
    private Vector3 gridOffset;
    private static List<ImplicitCreature> creatures = new List<ImplicitCreature>();
    private Vector3 position = Vector3.zero;
    
    void Start() {
        Random.InitState(mainRandomSeed);
        for (int i = 0; i < 5; i++) {
            creatureSeeds[i] = Random.Range(1, 501);
        }
        if (creatures.Count == 0) {
            multCreatures();
        }
    }

    void Update() {
        for (int i = 0; i < allAntennae.Length; i++) {
            if (allAntennae[i] != null) {
                float angle = Mathf.PingPong(Time.time * 40, 30);
                allAntennae[i].transform.localEulerAngles = new Vector3(
                    angle, 
                    allAntennae[i].transform.localEulerAngles.y, 
                    allAntennae[i].transform.localEulerAngles.z);
            }
        }
        for (int i = 0; i < allWings.Length; i++) {
            if (allWings[i] != null) {
                float flapAngle = Mathf.PingPong(Time.time * 30, 45);
                if (i % 2 == 0) { 
                    allWings[i].transform.localEulerAngles = new Vector3(
                        allWings[i].transform.localEulerAngles.x, 
                        flapAngle - 90, 
                        allWings[i].transform.localEulerAngles.z);
                } else {
                    allWings[i].transform.localEulerAngles = new Vector3(
                        allWings[i].transform.localEulerAngles.x, 
                        - flapAngle - 90, 
                        allWings[i].transform.localEulerAngles.z);
                }
            }
        }
        
    }

    private void multCreatures()
    {
        float spaceBetween = 25f;
        int numCreatures = 5;
        float totalWidth = spaceBetween * (numCreatures - 1);
        float startX = -totalWidth / 2;
        position = new Vector3(startX, 0f, 0f);
        creatures.Add(this);
        initCreature();
        randomSwappables();
        for (int i = 1; i < numCreatures; i++) {
            GameObject creatureObj = new GameObject($"Creature_{i}");
            ImplicitCreature creature = creatureObj.AddComponent<ImplicitCreature>();
            creature.position = new Vector3(startX + spaceBetween * i, 0f, 0f);
            creatures.Add(creature);
            creature.initCreature();
            position = new Vector3(startX + spaceBetween * i, 0f, 0f);
            randomSwappables();
        }
    }

    private void initCreature() {
        gridOffset = position + new Vector3(gridSize * cubeSize / 2f, 0, gridSize * cubeSize / 2f);
        meshData();
        createMesh();
    }


    public void meshData() {
        meshVerts.Clear();
        meshTris.Clear();
        vertexNorms.Clear();
        vertDict.Clear();
        int halfGridX = gridSize / 2; 
        int halfGridY = gridSize;
        Vector3 startPos = position - new Vector3(halfGridX * cubeSize, halfGridY * cubeSize / 2, halfGridX * cubeSize);
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < 2 * halfGridY; y++) {
                for (int z = 0; z < gridSize; z++) {
                    float[] cubeVals = new float[8];
                    Vector3 cubePos = startPos + new Vector3(x * cubeSize, y * cubeSize, z * cubeSize);
                    for (int i = 0; i < 8; i++) {
                        Vector3 worldPos = cubePos + cubeVerts[i] * cubeSize;
                        cubeVals[i] = implicitCreature(worldPos);
                    }
                    marchingCubes(cubePos, cubeVals);
                }
            }
        }
        calcSharedNorms();
    }

    float implicitCreature(Vector3 pos) {
        Vector3 headCenter = position + new Vector3(0, 5.5f, 0);
        Vector3 bodyStart = position + new Vector3(0, 1f, 0);
        Vector3 bodyEnd = position + new Vector3(0, -3f, 0);
        float totalField = float.MaxValue;
        float headField = implicitSphere(pos, headCenter, 2f);
        totalField = smoothMin(totalField, headField, blendFalloff);
        float bodyField = implicitEllipsoid(pos, bodyStart, bodyEnd, 2f);
        totalField = smoothMin(totalField, bodyField, blendFalloff);
        return totalField;
    }

    void randomSwappables() {
        Random.InitState(creatureSeeds[creatureCounter]);
        int legType = Random.Range(1, 4); 
        float legSize = Random.Range(0.5f, 1.3f);
        int wingType = Random.Range(1, 4); 
        float wingSize = Random.Range(0.3f, 1.01f);
        Color wingColor = colors[Random.Range(0, 8)];
        int legNum = 2;
        if (Random.value >= 0.5) {
            legNum = 4;
        }
        float antSize = Random.Range(0.2f, 0.51f);
        Color antColor = colors[Random.Range(0, 8)];
        createWings(wingType, wingSize, wingColor);
        createLimbs(legNum, legType, legSize);
        createAntennae(antSize, antColor);
        Instantiate(eyeBall, position + new Vector3(0, 5.5f, -1.5f), Quaternion.Euler(0, 180, 0));
        creatureCounter++;
    }

    void createLimbs(int limbCount, int limbType, float limbSize) {
        GameObject left;
        GameObject right;
        GameObject back;
        GameObject front;
        if (limbType == 1) {
            left = Instantiate(legType1, position + new Vector3(-2, -4f, 0), Quaternion.Euler(0, 0, 0));
            right = Instantiate(legType1, position + new Vector3(2, -4f, 0), Quaternion.Euler(0, 180, 0));
            left.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            right.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            if (limbCount == 4) {
                back = Instantiate(legType1, position + new Vector3(0, -4f, 2), Quaternion.Euler(0, 90, 0));
                front = Instantiate(legType1, position + new Vector3(0, -4f, -2), Quaternion.Euler(0, -90, 0));
                back.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
                front.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            } 
        } else if (limbType == 2) {
            left = Instantiate(legType2, position + new Vector3(-2, -3f, 0), Quaternion.Euler(0, 0, 0));
            right = Instantiate(legType2, position + new Vector3(2, -3f, 0), Quaternion.Euler(0, 180, 0));
            left.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            right.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            if (limbCount == 4) {
                back = Instantiate(legType2, position + new Vector3(0, -3f, 2), Quaternion.Euler(0, 90, 0));
                front = Instantiate(legType2, position + new Vector3(0, -3f, -2), Quaternion.Euler(0, -90, 0));
                back.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
                front.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            }
        } else if (limbType == 3) {
            left = Instantiate(legType3, position + new Vector3(-2, -4f, 0), Quaternion.Euler(0, 0, 0));
            right = Instantiate(legType3, position + new Vector3(2, -4f, 0), Quaternion.Euler(0, 180, 0));
            left.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            right.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            if (limbCount == 4) {
                back = Instantiate(legType3, position + new Vector3(0, -4f, 2), Quaternion.Euler(0, 90, 0));
                front = Instantiate(legType3, position + new Vector3(0, -4f, -2), Quaternion.Euler(0, -90, 0));
                back.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
                front.transform.localScale = new Vector3(limbSize, limbSize, limbSize);
            } 
        }  
    }

    void createWings(int wingType, float wingSize, Color wingColor) {
        if (wingType == 1) {
            WingType1 wing1 = new WingType1(position + new Vector3(0, 0, 1.5f), 45, wingSize, wingColor);
            WingType1 wing2 = new WingType1(position + new Vector3(0, 0, 1.5f), -45, wingSize, wingColor);
            allWings[wingCount] = wing1.wing_obj;
            wingCount++;
            allWings[wingCount] = wing2.wing_obj;
            wingCount++;

        } else if (wingType == 2) {
            WingType2 wing1 = new WingType2(position + new Vector3(0, -1, 1.5f), 45, wingSize, wingColor);
            WingType2 wing2 = new WingType2(position + new Vector3(0, -1, 1.5f), -45, wingSize, wingColor);
            allWings[wingCount] = wing1.wing_obj;
            wingCount++;
            allWings[wingCount] = wing2.wing_obj;
            wingCount++;
        } else if (wingType == 3) {
            WingType3 wing1 = new WingType3(position + new Vector3(0, 0, 1.5f), 45, wingSize, wingColor);
            WingType3 wing2 = new WingType3(position + new Vector3(0, 0, 1.5f), -45, wingSize, wingColor);
            allWings[wingCount] = wing1.wing_obj;
            wingCount++;
            allWings[wingCount] = wing2.wing_obj;
            wingCount++;
        }
    }

    void createAntennae(float antSize, Color antColor) {
        GameObject antennae1 = Instantiate(antennae, position + new Vector3(-0.5f, 6.5f, -1f), Quaternion.Euler(0, 0, 30));
        GameObject antennae2 = Instantiate(antennae, position + new Vector3(0.5f, 6.5f, -1f), Quaternion.Euler(0, 0, -30));
        setObjColor(antennae1, antColor);
        setObjColor(antennae2, antColor);
        antennae1.transform.localScale = new Vector3(antSize, antSize, antSize);
        antennae2.transform.localScale = new Vector3(antSize, antSize, antSize);
        allAntennae[antennaeCount] = antennae1;
        antennaeCount++;
        allAntennae[antennaeCount] = antennae2;
        antennaeCount++;
    }

    private void setObjColor (GameObject obj, Color color) {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null){
            renderer.material.color = color;
        } else {
            Renderer[] childRenderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in childRenderers) {
                childRenderer.material.color = color;
            }
        }
    }
}