using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour
{
    public int seed = 12;

    private int max_order = 3;
    private List<Internode> internodes = new List<Internode>();
    private List<Node> nodes = new List<Node>();
    private List<Node> nodes_archive = new List<Node>();
    private List<Node> nodes_to_add = new List<Node>();
    private List<Node> nodes_to_remove = new List<Node>();
    private int branchCount;
    private float growthTimer;

    public enum GrowthType {
        Orthotropic,
        Plagiotropic,
        Normal
    }

    public enum Season {
        Spring,
        Summer, 
        Fall,
        Winter
    }

    void Start()
    {
        // general set up
        Random.InitState(seed);
        Vector3 start_T = Vector3.up;
        Bud start_apical_bud = new Bud(start_T);

        // tree 1
        Vector3 tree1_position = new Vector3(-50,0,0);
        Node tree1_start_node = new Node(tree1_position, start_apical_bud, false, 1);
        nodes.Add(tree1_start_node);
        grow_tree(5);
        grow_tree(3);
        create_cylinders(internodes, 40, GrowthType.Orthotropic, Season.Summer);

        clean();

        // tree 2
        Vector3 tree2_position = new Vector3(0,0,0);
        Node tree2_start_node = new Node(tree2_position, start_apical_bud, false, 1);
        nodes.Add(tree2_start_node);
        grow_tree(5);
        grow_tree(3);
        create_cylinders(internodes, 40, GrowthType.Plagiotropic, Season.Spring);

        clean();

        // tree 3
        Vector3 tree3_position = new Vector3(50,0,0);
        Node tree3_start_node = new Node(tree3_position, start_apical_bud, false, 1);
        nodes.Add(tree3_start_node);
        grow_tree(5);
        grow_tree(3);
        create_cylinders(internodes, 40, GrowthType.Normal, Season.Fall);
        
        clean();
    }

    void clean() {
        nodes.Clear();
        nodes_to_add.Clear();
        nodes_to_remove.Clear();
        internodes.Clear();
    }

    void grow_tree(int size) {
        for (int i = 1; i <= size ; i++) {
            foreach (Node node in nodes) {
                create_internode(node, false);
                if (Random.value < node.branch_prob && node.order + 1 <= max_order) {
                    create_internode(node, true);
                }
                nodes_to_remove.Add(node);
            }
            foreach (Node node in nodes_to_remove) {
                nodes.Remove(node);
            }
            nodes_to_remove.Clear();
            foreach (Node node in nodes_to_add) {
                nodes.Add(node);
                nodes_archive.Add(node);
            }
            nodes_to_add.Clear();
        }
    }

    void create_internode(Node node, bool branch) {
        Vector3 P = node.position;
        Vector3 T = node.apical_bud.direction;
        int new_order = node.order;
        if (branch && node.side) {
            T = node.side_bud.direction;
            new_order++;
        } 
        Vector3 arbitrary_norm = new Vector3(1, 0, 0);
        Vector3 N = Vector3.Cross(T, arbitrary_norm).normalized;
        Vector3 B = Vector3.Cross(T, N).normalized;
        float d = 5f;
        Vector3 P_new = P + d * T.normalized;
        Bud new_apical_bud = new Bud(T);
        Node new_node = new Node(P_new, new_apical_bud, Random.Range(0, 2) == 0, node.order);
        if (branch) {
            new_node.order++;
        }
        nodes_to_add.Add(new_node);
        Internode new_internode = new Internode(node, new_node, d, branch);
        internodes.Add(new_internode);
    }

    void create_cylinders(List<Internode> internodes , int num_cylinders, GrowthType type, Season season) {
        for (int j = 0; j < internodes.Count; j++) {
            Vector3 origin = internodes[0].node1.position;
            Internode internode = internodes[j];
            Vector3 P = internode.node1.position;
            Vector3 T = internode.T_init;
            foreach (Node node in nodes_archive) {
                if (node.id == internode.node1.id) {
                    P = node.position;
                    if (internode.from_side && node.side) {
                        T = node.side_bud.direction;
                    } else {
                        T = node.apical_bud.direction;
                    }
                    break;
                }
            }
            Vector3 U = new Vector3 (0, 1, 0);
            float k = 0.05f;
            float epsilon = 0.05f;
            float segment_length = internode.length / num_cylinders;
            
            for (int i = 0; i < num_cylinders; i++) {
                Vector3 P_new = P + segment_length * T.normalized;
                
                float origin_dist = Vector3.Distance(origin, P_new);

                float max_scalefactor = 0.4f; 
                float min_scalefactor = 0.05f; 
                float scalefactor = Mathf.Max(min_scalefactor, max_scalefactor * (1 - origin_dist / 25f));
                 
                if (type == GrowthType.Orthotropic && internode.node2.order != 1) { // orthotropic growth     
                    T = (T + k * U).normalized;
                } else if (type == GrowthType.Plagiotropic && internode.node2.order != 1) { // plagiotropic growth
                    Vector3 T_proj = new Vector3(T.x, 0, T.z);
                    Vector3 H = (T_proj).normalized;
                    T = (T + k * H).normalized;
                }
                // randomness
                int rand_x = Random.Range(0, 2) == 0 ? -1 : 1;
                int rand_y = Random.Range(0, 2) == 0 ? -1 : 1;
                int rand_z = Random.Range(0, 2) == 0 ? -1 : 1;
                Vector3 T_rand = new Vector3(T.x + epsilon * rand_x, T.y + epsilon * rand_y, T.z + epsilon * rand_z);
                T = T_rand.normalized;
                
                GameObject internode_obj = new GameObject("Internode");
                internode_obj.AddComponent<MeshFilter>();
                internode_obj.AddComponent<MeshRenderer>();
                Mesh internode_mesh = create_internode_mesh(P, P_new, T, scalefactor);
                internode_mesh.RecalculateBounds();
                internode_mesh.RecalculateNormals();
                internode_obj.GetComponent<MeshFilter>().mesh = internode_mesh;
                Material internode_mat = new Material(Shader.Find("Standard")); 
                internode_mat.color = new Color(0.588f, 0.294f, 0f);  
                internode_obj.GetComponent<MeshRenderer>().material = internode_mat;

                Vector3 P_shifted = P_new + T.normalized * 0.1f; 
                GameObject shifted_internode_obj = new GameObject("ShiftedInternode");
                shifted_internode_obj.AddComponent<MeshFilter>();
                shifted_internode_obj.AddComponent<MeshRenderer>();
                Mesh shifted_internode_mesh = create_internode_mesh(P_new, P_shifted, T, scalefactor);
                shifted_internode_mesh.RecalculateBounds();
                shifted_internode_mesh.RecalculateNormals();
                shifted_internode_obj.GetComponent<MeshFilter>().mesh = shifted_internode_mesh;
                shifted_internode_obj.GetComponent<MeshRenderer>().material = internode_mat; 
                P = P_new;

                // LEAVES
                if (season == Season.Spring) {
                    if (internode.node1.order != 1 && Random.value <= 0.1f) {
                        Leaf leaf = new Leaf(P);
                    } 
                } else if (season == Season.Summer) {
                    if (internode.node1.order != 1 && Random.value <= 0.15f) {
                        Leaf leaf = new Leaf(P);
                    } 
                } else if (season == Season.Fall) {
                    if (internode.node1.order != 1 && Random.value < 0.1) {
                        Leaf leaf = new Leaf(P, true);
                    } else if (internode.node1.order != 1 && Random.value < 0.2) {
                        Leaf leaf = new Leaf(new Vector3 (P.x, 0.1f, P.z), true);
                    }
                }
            }
            foreach (Node node in nodes_archive) {
                if (internode.node2.id == node.id) {
                    node.position = P;
                    node.apical_bud.direction = T;
                    if (node.side) {
                        node.side_bud.direction = (T + new Vector3(1f, -0.75f, 0f)).normalized;
                    }
                    
                }
            }
        }
        // FLOWERS
        if (season == Season.Spring) {
            foreach (Node node in nodes) {
                Flower flower = new Flower(node.position, node.apical_bud.direction);
            }
        }
    }

    Mesh create_internode_mesh(Vector3 P, Vector3 P_new, Vector3 T, float scalefactor) {
        Vector3 arbitrary_norm = new Vector3(1, 0, 0);
        Vector3 N = Vector3.Cross(T, arbitrary_norm).normalized;
        Vector3 B = Vector3.Cross(T, N).normalized;
        N.Normalize();
        B.Normalize();
        int faces = 16;
        Vector3[] verts = new Vector3[faces * 2];
        int[] tris = new int[faces * 6 * 2];

        // vert
        for (int i = 0; i < faces; i++)
        {
            float angle = (360f / faces) * i * Mathf.Deg2Rad;
            // bottom
            verts[i] = P + N * Mathf.Cos(angle) * scalefactor + B * Mathf.Sin(angle) * scalefactor;
            // top
            verts[i + faces] = P_new + N * Mathf.Cos(angle) * scalefactor + B * Mathf.Sin(angle) * scalefactor;
        }

        // tris
        int tri_count = 0;
        for (int i = 0; i < faces; i++)
        {
            int next = (i + 1) % faces;
            tris[tri_count++] = i; 
            tris[tri_count++] = next + faces;
            tris[tri_count++] = i + faces;

            tris[tri_count++] = i;
            tris[tri_count++] = next;
            tris[tri_count++] = next + faces;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }
}