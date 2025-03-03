using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public Bud apical_bud;
    public Bud side_bud;
    public bool side;
    public float die_prob;
    public float branch_prob;
    public int order;
    public static int id_counter = 0;
    public int id;

    public Node(Vector3 position_input, Bud apical_bud_input, bool side_input, int order_input){
        position = position_input;
        apical_bud = apical_bud_input;
        side = side_input;
        if (side) {
            side_bud = new Bud((apical_bud.direction + new Vector3(Random.value, Random.value, Random.value)).normalized);
        }
        branch_prob = 1f / order;
        order = order_input;
        id = id_counter;
        id_counter++;
    }
}
