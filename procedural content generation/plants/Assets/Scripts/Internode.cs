using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Internode
{
    public Node node1;
    public Node node2;
    public float length;
    public Vector3 T_init;
    public bool from_side;

    public Internode(Node node1input, Node node2input, float length_input, bool from_side_in) {
        node1 = node1input;
        node2 = node2input;
        length = length_input;
        from_side = from_side_in;
        T_init = (node2.position - node1.position).normalized;
    }
}
