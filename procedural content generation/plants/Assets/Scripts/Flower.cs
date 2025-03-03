using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    private Vector3[] verts;  
	private int[] tris;  
    private int tri_count = 0;    
    private Color[] colors = new Color[]
        {
            new Color(0.0f, 0.0f, 1.0f),
            new Color(0.0f, 0.5f, 1.0f),
            new Color(0.0f, 0.0f, 0.5f),
            new Color(0.0f, 0.25f, 0.75f),
            new Color(0.5f, 0.75f, 1.0f),
            new Color(0.2f, 0.6f, 0.8f),
            new Color(0.0f, 0.3f, 0.6f),
            new Color(0.3f, 0.6f, 0.9f),
            new Color(0.1f, 0.5f, 0.8f),
            new Color(0.0f, 0.4f, 0.8f)
        };

    public Flower(Vector3 position, Vector3 tangent) {
        GameObject flower_obj = new GameObject("Flower");
        flower_obj.AddComponent<MeshFilter>();
        flower_obj.AddComponent<MeshRenderer>();
        Mesh flower_mesh = create_mesh();
        flower_obj.GetComponent<MeshFilter>().mesh = flower_mesh;
        flower_obj.transform.position = position;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, tangent);
        flower_obj.transform.rotation = rotation;
        // flower_obj.transform.rotation = Random.rotation;
        Material flower_mat = new Material(Shader.Find("Standard"));
        int rand_ind = Random.Range(0, colors.Length);
        flower_mat.color = colors[rand_ind]; 
        flower_obj.GetComponent<MeshRenderer>().material = flower_mat;
        flower_obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    Mesh create_mesh() {
        Vector3 t1 = new Vector3(0.125f, 3.5f, -2.9375f);
        Vector3 t2 = new Vector3(1.125f, 0.5f, -0.9375f);
        Vector3 t3 = new Vector3(3.125f, 3.5f, 0.0625f);
        Vector3 t4 = new Vector3(1.125f, 0.5f, 1.0625f);
        Vector3 t5 = new Vector3(0.125f, 3.5f, 3.0625f);
        Vector3 t6 = new Vector3(-0.875f, 0.5f, 1.0625f);
        Vector3 t7 = new Vector3(-2.875f, 3.5f, 0.0625f);
        Vector3 t8 = new Vector3(-0.875f, 0.5f, -0.9375f);

        Vector3 b1 = new Vector3(0.125f, 0.5f, -2.1875f);
        Vector3 b2 = new Vector3(0.625f, -0.5f, -0.4375f);
        Vector3 b3 = new Vector3(2.375f, 0.5f, 0.0625f);
        Vector3 b4 = new Vector3(0.625f, -0.5f, 0.5625f);
        Vector3 b5 = new Vector3(0.125f, 0.5f, 2.3125f);
        Vector3 b6 = new Vector3(-0.375f, -0.5f, 0.5625f);
        Vector3 b7 = new Vector3(-2.125f, 0.5f, 0.0625f);
        Vector3 b8 = new Vector3(-0.375f, -0.5f, -0.4375f);

        verts = new Vector3[84];

        // top face
        verts[0] = t5;
        verts[1] = t4;
        verts[2] = t6;

        verts[3] = t3;
        verts[4] = t2;
        verts[5] = t4;

        verts[6] = t2;
        verts[7] = t1;
        verts[8] = t8;

        verts[9] = t6;
        verts[10] = t8;
        verts[11] = t7;

        verts[12] = t2;
        verts[13] = t8;
        verts[14] = t6;

        verts[15] = t4;
        verts[16] = t2;
        verts[17] = t6;

        // bottom face

        verts[18] = b5;
        verts[19] = b6;
        verts[20] = b4;

        verts[21] = b7;
        verts[22] = b8;
        verts[23] = b6;

        verts[24] = b8;
        verts[25] = b1;
        verts[26] = b2;

        verts[27] = b4;
        verts[28] = b2;
        verts[29] = b3;

        verts[30] = b6;
        verts[31] = b2;
        verts[32] = b4;

        verts[33] = b6;
        verts[34] = b8;
        verts[35] = b2;

        // sides

        verts[36] = t1;
        verts[37] = t2;
        verts[38] = b1;
        verts[39] = t2;
        verts[40] = b2;
        verts[41] = b1;

        verts[42] = t2;
        verts[43] = t3;
        verts[44] = b2;
        verts[45] = t3;
        verts[46] = b3;
        verts[47] = b2;

        verts[48] = t3;
        verts[49] = t4;
        verts[50] = b3;
        verts[51] = t4;
        verts[52] = b4;
        verts[53] = b3;

        verts[54] = t4;
        verts[55] = t5;
        verts[56] = b4;
        verts[57] = t5;
        verts[58] = b5;
        verts[59] = b4;

        verts[60] = t5;
        verts[61] = t6;
        verts[62] = b5;
        verts[63] = t6;
        verts[64] = b6;
        verts[65] = b5;

        verts[66] = t6;
        verts[67] = t7;
        verts[68] = b6;
        verts[69] = t7;
        verts[70] = b7;
        verts[71] = b6;

        verts[72] = t7;
        verts[73] = t8;
        verts[74] = b7;
        verts[75] = t8;
        verts[76] = b8;
        verts[77] = b7;

        verts[78] = t8;
        verts[79] = t1;
        verts[80] = b8;
        verts[81] = t1;
        verts[82] = b1;
        verts[83] = b8;

        tris = new int[84];

        MakeTri(0, 1, 2); 
        MakeTri(3, 4, 5); 
        MakeTri(6, 7, 8); 
        MakeTri(9, 10, 11); 
        MakeTri(12, 13, 14);
        MakeTri(15, 16, 17);

        MakeTri(18, 19, 20);
        MakeTri(21, 22, 23); 
        MakeTri(24, 25, 26);
        MakeTri(27, 28, 29);
        MakeTri(30, 31, 32); 
        MakeTri(33, 34, 35); 

        MakeTri(36, 37, 38); 
        MakeTri(39, 40, 41);  

        MakeTri(42, 43, 44); 
        MakeTri(45, 46, 47); 

        MakeTri(48, 49, 50);  
        MakeTri(51, 52, 53); 

        MakeTri(54, 55, 56);  
        MakeTri(57, 58, 59); 

        MakeTri(60, 61, 62); 
        MakeTri(63, 64, 65); 

        MakeTri(66, 67, 68); 
        MakeTri(69, 70, 71); 

        MakeTri(72, 73, 74); 
        MakeTri(75, 76, 77); 

        MakeTri(78, 79, 80); 
        MakeTri(81, 82, 83);

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
