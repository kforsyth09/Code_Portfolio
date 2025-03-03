using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public GameObject boidPrefab;
    public GameObject trailPrefab;
    public LayerMask obstacleLayer;

    public bool wandering = false;
    private float wanderingWeight = 0.1f;

    public bool flockCentering = false;
    private float flockCenteringWeight = 0.1f;

    public bool collisionAvoidance = false;
    private float collisionAvoidanceWeight = 1.5f;

    public bool velocityMatching = false;
    private float velocityMatchingWeight = 0.5f;

    public bool increaseBoidNumber;
    public int boidNumber = 30;
    public bool decreaseBoidNumber;


    private bool obstacleAvoidance = true;
    private float obstacleAvoidanceWeight = 10f;
    public bool trails = false;
    
    private float obstacleAvoidanceDistance = 5f;
    

    private int trailLength = 30;

    // World Box (with wall bouncing)
    private static float size = 50f;
    private float xMin = - size;
    private float xMax = size;
    private float yMin = - size;
    private float yMax = size;
    private float zMin = - size;
    private float zMax = size;

    private float deltat = 0.75f;

    private float velocityMin = 0.5f;
    private float velocityMax = 1.5f;

    private List<Boid> currentBoids = new List<Boid>();
    
    void Start() {
        for (int i = 0; i < boidNumber; i++) {
            Vector3 randPos = new Vector3(
                Random.Range(xMin, xMax + Mathf.Epsilon),
                Random.Range(yMin, yMax + Mathf.Epsilon),
                Random.Range(zMin, zMax + Mathf.Epsilon)
            );
            Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
            GameObject newInstance = Instantiate(boidPrefab, randPos, rot);
            Boid newBoid = new Boid();
            newBoid.instance = newInstance;
            currentBoids.Add(newBoid);
        }
    }

    void FixedUpdate() {
        if (increaseBoidNumber) {
            Vector3 randPos = new Vector3(
                Random.Range(xMin, xMax + Mathf.Epsilon),
                Random.Range(yMin, yMax + Mathf.Epsilon),
                Random.Range(zMin, zMax + Mathf.Epsilon)
            );
            Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
            GameObject newInstance = Instantiate(boidPrefab, randPos, rot);
            Boid newBoid = new Boid();
            newBoid.instance = newInstance;
            currentBoids.Add(newBoid);
            boidNumber = currentBoids.Count;
            increaseBoidNumber = false;
        }
        if (decreaseBoidNumber && currentBoids.Count != 0) {
            Boid removed = currentBoids[currentBoids.Count - 1];
            currentBoids.RemoveAt(currentBoids.Count - 1);
            if (trails) {
                foreach (GameObject obj in removed.trail) {
                    Destroy(obj);
                }
                removed.trail.Clear();
            }
            Destroy(removed.instance);
            boidNumber = currentBoids.Count;
            decreaseBoidNumber = false;
        }
        if (trails != true) {
            foreach (Boid boid in currentBoids) {
                if (boid.trail.Count != 0) {
                    foreach (GameObject obj in boid.trail) {
                        Destroy(obj);
                    }
                    boid.trail.Clear();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            foreach (Boid boid in currentBoids) {
                // transport boid to a random position
                Vector3 randPos = new Vector3(
                    Random.Range(xMin, xMax + Mathf.Epsilon),
                    Random.Range(yMin, yMax + Mathf.Epsilon),
                    Random.Range(zMin, zMax + Mathf.Epsilon)
                );
                boid.instance.transform.position = randPos;
                // remove previous trail
                if (trails) {
                    foreach (GameObject obj in boid.trail) {
                        Destroy(obj);
                    }
                    boid.trail.Clear();
                }
            }
        }
        // simulation
        foreach (Boid boid in currentBoids) {
            boid.netForce = netForceCalc(boid);
        }
        foreach (Boid boid in currentBoids) {
            Vector3 vPrime = boid.velocity + deltat * boid.netForce;
            if (vPrime.magnitude > velocityMax) {
                vPrime = vPrime.normalized * velocityMax;
            } else if (vPrime.magnitude < velocityMin) {
                vPrime = vPrime.normalized * velocityMin;
            }
            boid.velocity = vPrime;
            Vector3 pPrime = boid.instance.transform.position + deltat * vPrime;
            pPrime.x = Mathf.Clamp(pPrime.x, xMin, xMax);
            pPrime.y = Mathf.Clamp(pPrime.y, yMin, yMax);
            pPrime.z = Mathf.Clamp(pPrime.z, zMin, zMax);
            boid.instance.transform.position = pPrime;
            boid.instance.transform.forward = boid.velocity;
            wallBounce(boid);
            
            // add trails
            if (trails) {
                if (boid.trail.Count == trailLength) {
                    GameObject oldest = boid.trail[trailLength - 1];
                    boid.trail.RemoveAt(trailLength - 1);
                    Destroy(oldest);
                }
                GameObject trailNew = Instantiate(trailPrefab, boid.instance.transform.position, Quaternion.Euler(0f, 0f, 0f));
                boid.trail.Insert(0, trailNew);
            }
            
        }
    }

    Vector3 netForceCalc(Boid boid) {
        Vector3 wanderingForce = new Vector3(0f, 0f, 0f);
        Vector3 flockCenteringForce = new Vector3(0f, 0f, 0f);
        Vector3 collisionAvoidanceForce  = new Vector3(0f, 0f, 0f);
        Vector3 velocityMatchingForce  = new Vector3(0f, 0f, 0f);
        Vector3 obstacleAvoidanceForce = new Vector3(0f, 0f, 0f);
        if (wandering) {
            wanderingForce = wanderingForceCalc();
        }
        if (flockCentering) {
            flockCenteringForce = flockCenteringForceCalc(boid);
        }
        if (collisionAvoidance) {
            collisionAvoidanceForce = collisionAvoidanceForceCalc(boid);
        }
        if (velocityMatching) {
            velocityMatchingForce = velocityMatchingForceCalc(boid);
        }
        if (obstacleAvoidance) {
            obstacleAvoidanceForce = avoidStaticObstacles(boid);
        }
        
        return flockCenteringForce * flockCenteringWeight + velocityMatchingForce * velocityMatchingWeight + collisionAvoidanceForce * collisionAvoidanceWeight + wanderingForce * wanderingWeight + obstacleAvoidanceForce;
    }

    Vector3 wanderingForceCalc() {
        // f_w = (r_x, r_y, r_z)
        float rx = Random.Range(-1f, 1f);
        float ry = Random.Range(-1f, 1f);
        float rz = Random.Range(-1f, 1f);
        return new Vector3(rx, ry, rz);
    }

    Vector3 flockCenteringForceCalc(Boid boid) {
        // r_fc = d_max
        float rfc = 20f;
        
        // k = nearby neighbors
        (List<Boid> neighborBoids, List<float> neighborWeights) = getNearbyNeighbors(boid, rfc);
        int k = neighborBoids.Count;
        if (k == 0) {
            return new Vector3(0f, 0f, 0f);
        }

        // p = current boid position]  
        Vector3 p = boid.instance.transform.position;

        
        Vector3 numerator = new Vector3(0f, 0f, 0f);
        float denominator = 0f;
        for (int i = 0 ; i < k ; i++) {
            Vector3 pi = neighborBoids[i].instance.transform.position;
            float wi = neighborWeights[i];
            numerator += wi * (pi - p);
            denominator += wi;
        }
        
        Vector3 ffc = numerator / denominator;

        return ffc;
    }

    Vector3 collisionAvoidanceForceCalc(Boid boid) {
        // collision avoidance - repel when too close
        // tighter radius r_ca (smaller than r_fc)    
        float rca = 10f;

        // k = nearby neighbors
        (List<Boid> neighborBoids, List<float> neighborWeights) = getNearbyNeighbors(boid, rca);
        int k = neighborBoids.Count;
        if (k == 0) {
            return new Vector3(0f, 0f, 0f);
        }
        
        // p = current boid position]  
        Vector3 p = boid.instance.transform.position;

        // f_ca = sum i = 1...k {w_i(p - p_i)}
        Vector3 fca = new Vector3(0f, 0f, 0f);
        for (int i = 0 ; i < k ; i++) {
            Vector3 pi = neighborBoids[i].instance.transform.position;
            float wi = neighborWeights[i];
            fca += (wi * (p - pi));
        }

        return fca;
    }

    Vector3 velocityMatchingForceCalc(Boid boid) {
        // r_vm ~ r_fc
        float rvm = 20f;
    
        // k = nearby neighbors
        (List<Boid> neighborBoids, List<float> neighborWeights) = getNearbyNeighbors(boid, rvm);
        int k = neighborBoids.Count;
        if (k == 0) {
            return new Vector3(0f, 0f, 0f);
        }

        // v = current boid velocity
        Vector3 v = boid.velocity;

        // f_vm = sum i = 1...k {w_i(v_i - v)}
        
        Vector3 fvm = new Vector3(0f, 0f, 0f);
        for (int i = 0 ; i < k ; i++) {
            Vector3 vi = neighborBoids[i].velocity;
            float wi = neighborWeights[i];
            fvm += (wi * (vi - v));
        }

        return fvm;
    }

    (List<Boid> , List<float>) getNearbyNeighbors(Boid current, float radius) {
        List<Boid> returnBoids = new List<Boid>();
        List<float> returnWeights = new List<float>();
        // return list of k nearby flockmates and their weights

        foreach (Boid boid in currentBoids) {
            if (returnBoids.Count != 10) {
                if (boid.instance != current.instance) {
                    Vector3 currentPos = current.instance.transform.position;
                    Vector3 boidPos = boid.instance.transform.position;
                    
                    // d_ij = distance between boids i and j
                    float dij = Vector3.Distance(currentPos, boidPos);

                    if (dij <= radius) {
                        returnBoids.Insert(0, boid);
                        // w_ij = weight between boids i and j
                        // inverse square
                        // w_ij = 1 / ( d_ij^2 + epsilon )  
                        float wij = 1 / (dij * dij + Mathf.Epsilon);
                        returnWeights.Insert(0, wij);
                    }
                }
            }
        }
        return (returnBoids, returnWeights);
    }

    Vector3 avoidStaticObstacles(Boid boid) {
        // steer-to-avoid
        // 1) intersect forward ray w/ world
        // 2) find silhouette pointclosest to intersection
        // 3) aim one body length out from silhouette
        Vector3 position = boid.instance.transform.position;
        Vector3 velocity = boid.velocity;
        RaycastHit hit;
        if (Physics.Raycast(position, velocity.normalized, out hit, obstacleAvoidanceDistance, obstacleLayer)) {
            Vector3 away = position - hit.point;
            float distToObstacle = hit.distance;
            float strength = Mathf.Lerp(
                obstacleAvoidanceWeight, 
                0f, 
                distToObstacle / obstacleAvoidanceDistance
            );
            return away.normalized * strength;
        }
        return new Vector3(0f, 0f, 0f);
    }


    void wallBounce(Boid boid) {
        // bounce x
        Vector3 position = boid.instance.transform.position;
        if (position.x == xMax || position.x == xMin) {
            boid.velocity =  Vector3.Scale(boid.velocity, new Vector3(-2, 1, 1));
        } 

        // bounce y
        if (position.y == yMax || position.y == yMin) {
            boid.velocity =  Vector3.Scale(boid.velocity, new Vector3(1, -2, 1));
        } 

        // bounce z
        if (position.z == zMax || position.z == zMin) {
            boid.velocity =  Vector3.Scale(boid.velocity, new Vector3(1, 1, -2));
        }
        
        // clamp velocity
        if (boid.velocity.magnitude > velocityMax) {
            boid.velocity = boid.velocity.normalized * velocityMax;
        } else if (boid.velocity.magnitude < velocityMin) {
            boid.velocity = boid.velocity.normalized * velocityMin;
        }
    }
}
