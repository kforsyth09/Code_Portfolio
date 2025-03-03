using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    private Vector3[] verts;  
	private int[] tris;  
    private int tri_count = 0; 
    private Color[] greens = new Color[]
        {
            new Color(0.0f, 0.5f, 0.0f), 
            new Color(0.0f, 0.75f, 0.0f),
            new Color(0.5f, 1.0f, 0.5f),
            new Color(0.3f, 0.5f, 0.2f), 
            new Color(0.1f, 0.3f, 0.0f), 
            new Color(0.0f, 0.6f, 0.4f), 
            new Color(0.5f, 0.7f, 0.5f), 
            new Color(0.2f, 0.5f, 0.2f) 
        };
    private Color[] fall_colors = new Color[]
        {
            new Color(0.8f, 0.2f, 0.0f),  
            new Color(1.0f, 0.4f, 0.0f),
            new Color(1.0f, 0.6f, 0.2f),
            new Color(1.0f, 0.8f, 0.3f),
            new Color(0.9f, 0.5f, 0.0f),
            new Color(0.6f, 0.3f, 0.1f),
            new Color(0.8f, 0.4f, 0.0f),
            new Color(0.6f, 0.1f, 0.0f),
            new Color(0.7f, 0.5f, 0.1f),
            new Color(0.4f, 0.2f, 0.0f),
            new Color(0.9f, 0.7f, 0.0f),
            new Color(0.5f, 0.1f, 0.0f),
            new Color(0.7f, 0.4f, 0.1f),
            new Color(0.6f, 0.2f, 0.1f),
            new Color(0.9f, 0.3f, 0.1f)
        };


    public Leaf(Vector3 position, bool fall = false) {
        GameObject leaf_obj = new GameObject("Leaf");
        leaf_obj.AddComponent<MeshFilter>();
        leaf_obj.AddComponent<MeshRenderer>();
        Mesh leaf_mesh = create_mesh();
        leaf_obj.GetComponent<MeshFilter>().mesh = leaf_mesh;
        leaf_obj.transform.position = position;
        leaf_obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        leaf_obj.transform.rotation = Random.rotation;
        Material leaf_mat = new Material(Shader.Find("Standard"));  
        int rand_ind = Random.Range(0, greens.Length);
        leaf_mat.color = greens[rand_ind]; 
        if (fall) {
            rand_ind = Random.Range(0, fall_colors.Length);
            leaf_mat.color = fall_colors[rand_ind]; 
        }
        leaf_obj.GetComponent<MeshRenderer>().material = leaf_mat;
        
    }

    Mesh create_mesh() {
        Vector3 v1 = new Vector3(-0.5f, 0f, 0.5f);
        Vector3 v2 = new Vector3(0.5f, 0f, 0.5f);
        Vector3 v3 = new Vector3(-0.5f, 4f, 0.5f);
        Vector3 v4 = new Vector3(0.5f, 4f, 0.5f);
        Vector3 v5 = new Vector3(-4.0f, 8f, 0.5f);
        Vector3 v6 = new Vector3(4.0f, 8f, 0.5f);
        Vector3 v7 = new Vector3(0f, 15f, 0.5f);

        Vector3 v8 = new Vector3(-0.5f, 0f, -0.5f);
        Vector3 v9 = new Vector3(0.5f, 0f, -0.5f);
        Vector3 v10 = new Vector3(-0.5f, 4f, -0.5f);
        Vector3 v11 = new Vector3(0.5f, 4f, -0.5f);
        Vector3 v12 = new Vector3(-4.0f, 8f, -0.5f);
        Vector3 v13 = new Vector3(4.0f, 8f, -0.5f);
        Vector3 v14 = new Vector3(0f, 15f, -0.5f);
        
        verts = new Vector3[72];

        // front face
        verts[0] = v4;
        verts[1] = v2;
        verts[2] = v1;

        verts[3] = v4;
        verts[4] = v1;
        verts[5] = v3;

        verts[6] = v5;
        verts[7] = v4;
        verts[8] = v3;

        verts[9] = v6;
        verts[10] = v4;
        verts[11] = v5;

        verts[12] = v7;
        verts[13] = v6;
        verts[14] = v5;

        // back face 
        verts[15] = v10;
        verts[16] = v8;
        verts[17] = v9;

        verts[18] = v10;
        verts[19] = v9;
        verts[20] = v11;

        verts[21] = v12;
        verts[22] = v10;
        verts[23] = v13;

        verts[24] = v10;
        verts[25] = v11;
        verts[26] = v13;

        verts[27] = v14;
        verts[28] = v12;
        verts[29] = v13;

        // left face
        verts[30] = v11;
        verts[31] = v9;
        verts[32] = v2;

        verts[33] = v11;
        verts[34] = v2;
        verts[35] = v4;

        verts[36] = v13;
        verts[37] = v11;
        verts[38] = v4;

        verts[39] = v13;
        verts[40] = v4;
        verts[41] = v6;

        verts[42] = v14;
        verts[43] = v13;
        verts[44] = v6;

        verts[45] = v14;
        verts[46] = v6;
        verts[47] = v7;

        // right face
        verts[48] = v3;
        verts[49] = v1;
        verts[50] = v8;

        verts[51] = v3;
        verts[52] = v8;
        verts[53] = v10;

        verts[54] = v5;
        verts[55] = v3;
        verts[56] = v10;

        verts[57] = v5;
        verts[58] = v10;
        verts[59] = v12;

        verts[60] = v7;
        verts[61] = v5;
        verts[62] = v12;

        verts[63] = v7;
        verts[64] = v12;
        verts[65] = v14;

        // bottom face
        verts[66] = v1;
        verts[67] = v2;
        verts[68] = v9;

        verts[69] = v1;
        verts[70] = v9;
        verts[71] = v8;

        tris = new int[72];

        MakeTri(2, 1, 0); 
        MakeTri(5, 4, 3); 
        MakeTri(8, 7, 6); 
        MakeTri(11, 10, 9); 
        MakeTri(14, 13, 12);

        MakeTri(17, 16, 15);
        MakeTri(20, 19, 18);
        MakeTri(23, 22, 21); 
        MakeTri(26, 25, 24);
        MakeTri(29, 28, 27);

        MakeTri(32, 31, 30); 
        MakeTri(35, 34, 33); 
        MakeTri(38, 37, 36); 
        MakeTri(41, 40, 39);  
        MakeTri(44, 43, 42); 
        MakeTri(47, 46, 45); 

        MakeTri(50, 49, 48);  
        MakeTri(53, 52, 51);  
        MakeTri(56, 55, 54);  
        MakeTri(59, 58, 57); 
        MakeTri(62, 61, 60); 
        MakeTri(65, 64, 63); 

        MakeTri(68, 67, 66); 
        MakeTri(71, 70, 69); 

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();  
		return (mesh);
    }

    void MakeTri(int i1, int i2, int i3) {
		int index = tri_count * 3;  
		tri_count++;
		tris[index]     = i1;
		tris[index + 1] = i2;
		tris[index + 2] = i3;
	}

}
