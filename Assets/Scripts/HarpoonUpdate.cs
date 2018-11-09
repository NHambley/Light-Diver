using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUpdate : MonoBehaviour {

    private float maxSpeed;
    private float speed;
    private Vector2 direction;
    private float range;
    private float traveledDistance;
    public AnimationCurve speedCurve;
    private Vector2 pull;
    private float damage;

    public void SetUp(float speedSet, Vector2 direction, float range, Vector2 playerVelocity, float damage)
    {
        Vector2 hookSpeed = direction * speedSet;
        float newSpeed = (hookSpeed + playerVelocity).magnitude;

        float speedMod = (newSpeed / speedSet);
        if (speedMod > Constants.HOOK_SPEED_MAX_MOD) { speedMod = Constants.HOOK_SPEED_MAX_MOD; }
        if (speedMod < Constants.HOOK_SPEED_MIN_MOD) { speedMod = Constants.HOOK_SPEED_MIN_MOD; }
        this.maxSpeed = speedSet * speedMod;
        
        this.range = range * speedMod;

        this.direction = direction.normalized;
        this.traveledDistance = 0;

        this.damage = damage;

        this.speed = this.speedCurve.Evaluate(0f) * this.maxSpeed;
    }

    public float GetDamage()
    {
        float damagePercent = ((this.speed + this.pull.magnitude) / Constants.HOOK_MAX_DAMAGE_SPEED);
        float damageGive = (damagePercent > 1) ? this.damage : this.damage * damagePercent;

        return damageGive;
    }

    public float GetRange()
    {
        return range;
    }

    public void PullBack(Vector2 pullForce)
    {
        this.pull += pullForce;
    }

    public void HitEnemy()
    {
        this.pull *= 0.8f;
        this.traveledDistance += (this.range * 0.2f);
    }

    // Update is called once per frame
    void Update ()
    {
        if (this.speed > 0)
        {
            this.speed = this.speedCurve.Evaluate(this.traveledDistance / this.range) * this.maxSpeed;
            if (this.speed < 0.0001f)
            {
                this.speed = 0;
            }
        }

        Vector3 newPos = new Vector3(this.direction.x * this.speed, this.direction.y * this.speed, 0);

        if (this.pull != Vector2.zero)
        {
            newPos.x += this.pull.x;
            newPos.y += this.pull.y;

            this.pull *= Constants.HOOK_PULL_SPEED_DECAY;
            if (this.pull.magnitude <= 0.001f)
            {
                this.pull = Vector2.zero;
            }
        }

        this.transform.position += newPos;

        this.traveledDistance += this.speed;

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_ENEMY))
        {
            this.HitEnemy();
        }
    }
}
