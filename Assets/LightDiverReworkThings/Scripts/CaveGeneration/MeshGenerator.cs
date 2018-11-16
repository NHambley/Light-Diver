using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }
   /* public class ControlNode : Node
    {

    }*/
}
