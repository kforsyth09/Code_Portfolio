using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingType2 : MonoBehaviour
{
    private Vector3[] verts;  
	private int[] tris;  
    private int tri_count = 0; 
    public GameObject wing_obj = new GameObject("WingType2");


    public WingType2(Vector3 position, int rot, float size, Color color) {
        wing_obj.AddComponent<MeshFilter>();
        wing_obj.AddComponent<MeshRenderer>();
        Mesh wing_mesh = create_mesh();
        wing_obj.GetComponent<MeshFilter>().mesh = wing_mesh;
        wing_obj.transform.position = position;
        wing_obj.transform.localScale = new Vector3(size, size, size);
        wing_obj.transform.rotation = Quaternion.Euler(0, rot - 90, 0);
        Material wing_mat = new Material(Shader.Find("Standard"));  
        wing_mat.color = color; 
        wing_obj.GetComponent<MeshRenderer>().material = wing_mat;
        
    }

    Mesh create_mesh() {
        Vector3 v1 = new Vector3(0f, -1f, 0.5f);
        Vector3 v2 = new Vector3(7f, 1f, 0.5f);
        Vector3 v3 = new Vector3(10f, 0f, 0.5f);
        Vector3 v4 = new Vector3(14f, -7f, 0.5f);
        Vector3 v5 = new Vector3(14f, 4f, 0.5f);
        Vector3 v6 = new Vector3(7f, 9f, 0.5f);
        Vector3 v7 = new Vector3(2f, 7f, 0.5f);
        Vector3 v8 = new Vector3(0f, 1f, 0.5f);
        
        Vector3 v9 = new Vector3(0f, -1f, -0.5f);
        Vector3 v10 = new Vector3(7f, 1f, -0.5f);
        Vector3 v11 = new Vector3(10f, 0f, -0.5f);
        Vector3 v12 = new Vector3(14f, -7f, -0.5f);
        Vector3 v13 = new Vector3(14f, 4f, -0.5f);
        Vector3 v14 = new Vector3(7f, 9f, -0.5f);
        Vector3 v15 = new Vector3(2f, 7f, -0.5f);
        Vector3 v16 = new Vector3(0f, 1f, -0.5f);

        
        verts = new Vector3[84];

        // front face

        verts[0] = v2;
        verts[1] = v1;
        verts[2] = v8;

        verts[3] = v7;
        verts[4] = v2;
        verts[5] = v8;

        verts[6] = v6;
        verts[7] = v2;
        verts[8] = v7;

        verts[9] = v5;
        verts[10] = v2;
        verts[11] = v6;

        verts[12] = v5;
        verts[13] = v3;
        verts[14] = v2;

        verts[15] = v5;
        verts[16] = v4;
        verts[17] = v3;

        // back face 

        verts[18] = v11;
        verts[19] = v12;
        verts[20] = v13;

        verts[21] = v10;
        verts[22] = v11;
        verts[23] = v13;

        verts[24] = v14;
        verts[25] = v10;
        verts[26] = v13;

        verts[27] = v15;
        verts[28] = v10;
        verts[29] = v14;

        verts[30] = v15;
        verts[31] = v16;
        verts[32] = v10;

        verts[33] = v16;
        verts[34] = v9;
        verts[35] = v10;

        // left face

        verts[36] = v14;
        verts[37] = v6;
        verts[38] = v15;

        verts[39] = v6;
        verts[40] = v7;
        verts[41] = v15;

        verts[42] = v7;
        verts[43] = v16;
        verts[44] = v15;

        verts[45] = v7;
        verts[46] = v8;
        verts[47] = v16;

        verts[48] = v8;
        verts[49] = v9;
        verts[50] = v16;

        verts[51] = v8;
        verts[52] = v1;
        verts[53] = v9;
        
        // right face

        verts[54] = v14;
        verts[55] = v13;
        verts[56] = v6;

        verts[57] = v13;
        verts[58] = v5;
        verts[59] = v6;

        verts[60] = v13;
        verts[61] = v12;
        verts[62] = v5;

        verts[63] = v12;
        verts[64] = v4;
        verts[65] = v5;

        // bottom face

        verts[66] = v4;
        verts[67] = v12;
        verts[68] = v11;

        verts[69] = v4;
        verts[70] = v11;
        verts[71] = v3;

        verts[72] = v3;
        verts[73] = v11;
        verts[74] = v10;

        verts[75] = v3;
        verts[76] = v10;
        verts[77] = v2;

        verts[78] = v2;
        verts[79] = v10;
        verts[80] = v9;

        verts[81] = v2;
        verts[82] = v9;
        verts[83] = v1;


        tris = new int[84];

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
        MakeTri(74, 73, 72);
        MakeTri(77, 76, 75);
        MakeTri(80, 79, 78);
        MakeTri(83, 82, 81);

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
