using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;

    public float maxSpeed;
    private Vector2 velocity;
    private Vector2 direction;
    private Vector2 position;
    private Vector2 force;
    private float health;
    public float maxHealth;

    private GameObject hitBy;
    private SpriteRenderer spriteRenderer;

    private EnemyInfo enemyInfo;
    public GameObject bloodParticles;
    private ParticleSystem particleSystem;

    public bool flipX;

    private bool allowedToMove;

    // Use this for initialization
    void Start ()
    {
        this.player = GameObject.Find(Constants.GAMEOBJECT_PLAYER);
        this.playerController = this.player.GetComponent<PlayerController>();
        this.position = this.transform.position;
        
        this.health = this.maxHealth;

        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.enemyInfo = this.gameObject.GetComponent<EnemyInfo>();
        this.particleSystem = this.bloodParticles.GetComponent<ParticleSystem>();

        this.allowedToMove = false;

        ParticleSystem.EmissionModule emission = this.particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.allowedToMove == false) {
            if (playerController.roomPosition.x == enemyInfo.indexX &&
                playerController.roomPosition.y == enemyInfo.indexY)
            {
                this.allowedToMove = true;
            }
            this.UpdateMovement();
            return;
        }

        this.Seek();
        this.UpdateMovement();

    }

    private void UpdateMovement()
    {
        this.velocity += this.force * Time.deltaTime;
        this.direction = velocity.normalized;

        Vector2.ClampMagnitude(this.velocity, this.maxSpeed);
        this.position += this.velocity;
        this.transform.position = this.position;

        float rot = Mathf.Atan2(this.direction.x, this.direction.y) * Mathf.Rad2Deg - 90;

        if (this.flipX)
        {
            if (this.direction.x < 0)
            {
                this.spriteRenderer.flipX = true;
                rot += 180;
            }
            else if (this.direction.x > 0)
            {
                this.spriteRenderer.flipX = false;
            }
        }

        this.transform.rotation = Quaternion.Euler(0, 0, -rot);
    }

    private void Seek()
    {
        Vector2 desiredVel = this.player.transform.position - this.transform.position;

        desiredVel = desiredVel.normalized * maxSpeed;
        this.force = desiredVel - this.velocity;
        this.force = Vector2.ClampMagnitude(this.force, maxSpeed);
    }

    private void UpdateHealthLook()
    {
        float percent = this.health / this.maxHealth;

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(percent, 0, 0);

        float burstCount = Constants.ENEMY_PARTICLE_MAX_EMMISION * (1f - percent);
        ParticleSystem.EmissionModule emission = this.particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(burstCount);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(Constants.TAG_PROJECTILE) && col.gameObject != hitBy)
        {
            float damageTaken = col.gameObject.GetComponent<HarpoonUpdate>().GetDamage();
            this.health -= damageTaken;

            if (this.health <= 0)
            {
                this.particleSystem.transform.parent = this.gameObject.transform.parent;
                ParticleSystem.EmissionModule emission = this.particleSystem.emission;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
                Destroy(this.gameObject);
                Destroy(this.bloodParticles, Constants.ENEMY_PARTICLE_DESTROY_TIME);
            }
            else
            {
                this.UpdateHealthLook();
            }
            this.hitBy = col.gameObject;
        }
    }
}
