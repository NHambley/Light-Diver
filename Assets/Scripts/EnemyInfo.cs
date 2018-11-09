using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour {

    public int damage;
    public float speed;
    public int indexX;
    public int indexY;

    public void SetUp(int x, int y)
    {
        indexX = x;
        indexY = y;
    }
}
