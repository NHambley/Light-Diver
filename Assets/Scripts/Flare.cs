using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    private Vector2 velocity;
    private Vector2 position;
    private float totalDistance;
    private bool continueGoing;
    private float stopDistance;

	// Use this for initialization
	public void SetUp(Vector2 vel, float stopDist)
    {
        this.position = this.transform.position;
        this.velocity = vel;
        this.totalDistance = 0f;
        this.stopDistance = stopDist;
        this.continueGoing = true;
    }

    public void Update()
    {
        if (continueGoing)
        {
            this.totalDistance += this.velocity.magnitude;
            if (this.totalDistance <= this.stopDistance)
            {
                this.position += this.velocity;
                this.transform.position = this.position;
            }
            else
            {
                this.continueGoing = false;
            }
        }
    }

    public void DestroyMe(float time)
    {
        Destroy(this.gameObject, time);
    }
}
