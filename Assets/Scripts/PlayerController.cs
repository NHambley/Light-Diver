using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //basic player vars
    private Vector2 velocity;
    private Vector2 direction;
    private Vector2 position;
    private byte moveDir = 0;
    private float rotation;

    //player modifiers
    private float acceleration;
    private float maxSpeed;
    private float rotationSpeed;

    private float range;
    private float shotSpeed;
    private float pullSpeed;

    private float damage;

    private float iFramesAmount;
    private float iFrameStart;
    private bool iFrames;
    private float switchTime;

    private int health;
    private int maxHealth;
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }

    //harpoon stuff
    public GameObject harpoonPrefab;
    private bool hookOut;
    private bool hookBack;
    private GameObject hook = null;
    bool afterFire = false;

    //wall list
    List<GameObject> walls;
    
    // light placing stuff
    public GameObject lantern;
    public GameObject flare;
    private int lightType = 0; 
    private int numLights = 2000;
    
    public HealthManager healthManager;
    public GameObject playerLight;
    public Vector2 roomPosition;
    
    // Use this for initialization
    void Start()
    {
        this.direction = new Vector2(0, -1);
        this.velocity = Vector2.zero;
        this.rotation = 0;

        this.maxSpeed = 0.08f;

        this.acceleration = 0.005f;
        this.rotationSpeed = 3f;

        this.position = this.transform.position;

        this.range = 5;
        this.shotSpeed = 0.1f;
        
        hookOut = false;
        
        this.pullSpeed = 0.01f;
        this.damage = 4;

        this.health = 5;
        this.maxHealth = 5;

        this.iFrames = false;
        this.iFramesAmount = 2f;
        this.iFrameStart = 0;

        hookOut = false;
        this.walls = GameObject.Find(Constants.GAMEOBJECT_LEVELGEN).GetComponent<LevelGeneration>().walls;

        this.healthManager.SetUp(this.maxHealth);
    }

    public void SetRoomPos(Vector2 roomPos)
    {
        this.roomPosition = roomPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) { return; } //TODO: Make a game over state

        this.UpdateMovement();
        this.CheckCollisionList(walls);
        this.CheckFire();
        this.LightDrop();
        if (this.iFrames)
        {
            this.CheckIFrames();
        }
        else 
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
    }

    private void CheckIFrames()
    {
        if (Time.time - this.iFrameStart >= this.iFramesAmount)
        {
            this.iFrames = false;
        }
        if (Time.time - this.switchTime >= 0.02)
        {
            SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            Color colorSet = new Color(1, 1, 1, 0.3f);
            colorSet = (spriteRenderer.color == colorSet) ? new Color(1, 1, 1, 1f) : colorSet;

            this.gameObject.GetComponent<SpriteRenderer>().color = colorSet;
            this.switchTime = Time.time;
        }
    }

    private void UpdateMovement()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            float rotSpeed = this.rotationSpeed;
            if (this.moveDir == 0)
            {
                rotSpeed *= Constants.PLAYER_ROTATE_SPEED_STILL_MOD;
            }
            if (this.moveDir == 2)
            {
                rotSpeed *= Constants.PLAYER_ROTATE_SPEED_BACKWARDS_MOD;
            }
            this.rotation += rotSpeed;
            this.direction = Quaternion.Euler(0, 0, rotSpeed) * this.direction;
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            float rotSpeed = this.rotationSpeed;
            if (this.moveDir == 0)
            {
                rotSpeed *= Constants.PLAYER_ROTATE_SPEED_STILL_MOD;
            }
            if (this.moveDir == 2)
            {
                rotSpeed *= Constants.PLAYER_ROTATE_SPEED_BACKWARDS_MOD;
            }
            this.rotation -= rotSpeed;
            this.direction = Quaternion.Euler(0, 0, -rotSpeed) * this.direction;
        }
        this.transform.rotation = Quaternion.Euler(0, 0, this.rotation);

        //check player input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.velocity += this.acceleration * this.direction;
            this.moveDir = 1;

            //this section is for the pulling of the rope post throwing through movement
            //as long as the hook is out and has stopped moving
            if ((this.hookOut && this.afterFire == false))
            {
                //distance between player and hook
                float distance = Mathf.Sqrt(Mathf.Pow((this.hook.transform.position.x - this.transform.position.x), 2) + Mathf.Pow((this.hook.transform.position.y - this.transform.position.y), 2));
                //while the distance is greater than the range of the hook
                if(distance > this.hook.GetComponent<HarpoonUpdate>().GetRange())
                {
                    //normalize the direction vector
                    Vector2 dir = this.position - new Vector2(this.hook.transform.position.x, this.hook.transform.position.y);
                    dir.Normalize();
                    //pull it, with a little less than pullSpeed
                    this.hook.GetComponent<HarpoonUpdate>().PullBack(dir * (this.pullSpeed * .8f));
                }
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.velocity -= this.acceleration * this.direction;
            this.moveDir = 2;

            //this section is for the pulling of the rope post throwing through movement
            //as long as the hook is out and has stopped moving
            if ((this.hookOut && this.afterFire == false))
            {
                //distance between player and hook
                float distance = Mathf.Sqrt(Mathf.Pow((this.hook.transform.position.x - this.transform.position.x), 2) + Mathf.Pow((this.hook.transform.position.y - this.transform.position.y), 2));
                //while the distance is greater than the range of the hook
                if (distance > this.hook.GetComponent<HarpoonUpdate>().GetRange())
                {
                    //normalize the direction vector
                    Vector2 dir = this.position - new Vector2(this.hook.transform.position.x, this.hook.transform.position.y);
                    dir.Normalize();
                    //pull it, with a little less than pullSpeed
                    this.hook.GetComponent<HarpoonUpdate>().PullBack(dir * (this.pullSpeed * .8f));
                }
            }
        }
        else
        {
            this.moveDir = 0;
            this.velocity *= 0.9f;
            if (this.velocity.magnitude <= 0.005f)
            {
                this.velocity = Vector2.zero;
            }
        }


        this.velocity = Vector2.ClampMagnitude(this.velocity, this.maxSpeed);

        this.position += this.velocity;
        this.transform.position = this.position;

    }

    private void CheckFire()
    {
        if (Input.GetMouseButtonDown(0) && !this.hookOut)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject harpoon = Instantiate(harpoonPrefab);
            harpoon.transform.position = this.transform.position;

            Vector2 dir = mousePos - this.position;
            float rot = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            rot -= 90;

            harpoon.transform.rotation = Quaternion.Euler(0, 0, -rot);

            float velocityAmount = this.velocity.magnitude;

            harpoon.GetComponent<HarpoonUpdate>().SetUp(this.shotSpeed, dir.normalized, this.range, this.velocity, this.damage);

            this.hook = harpoon;
            this.hookOut = true;
            afterFire = true;

            this.playerLight.SetActive(false);
        }
        else if (Input.GetMouseButton(0) && this.hook && this.afterFire == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dir = this.position - new Vector2(this.hook.transform.position.x, this.hook.transform.position.y);
            dir.Normalize();

            this.hook.GetComponent<HarpoonUpdate>().PullBack(dir * this.pullSpeed);
        }
        else if (Input.GetMouseButtonUp(0) && afterFire)
        {
            afterFire = false;
        }
    }

    private void LightDrop()
    {
        // two kinds of lights, flares and lanterns
        if (Input.GetKeyDown(KeyCode.Q)) // select the lantern as active
        {
            lightType = 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))// select the flare as active
        {
            lightType = 2;
        }

        // check if light placement is pressed
        if (Input.GetKeyDown(KeyCode.Space) && numLights > 0)
        {
            if (lightType == 1)
            {
                GameObject lantern = Instantiate(this.lantern, transform.position, Quaternion.identity);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 vel = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);
                float distance = vel.magnitude;
                vel = vel.normalized * Constants.LIGHT_SPEED;
                lantern.GetComponent<Flare>().SetUp(vel, distance);
            }
            else if (lightType == 2)
            {
                GameObject flare = Instantiate(this.flare, transform.position, Quaternion.identity);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 vel = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);
                float distance = vel.magnitude;
                vel = vel.normalized * Constants.LIGHT_SPEED * 0.7f;
                flare.GetComponent<Flare>().SetUp(vel, distance);
                flare.GetComponent<Flare>().DestroyMe(Constants.LIGHT_TIME);
            }
            lightType = 0;
            numLights--;
        }

    }

    private void CheckCollisionList(List<GameObject> objects)
    {
        foreach (GameObject g in objects)
        {
            Vector3 gPosition = g.transform.position;
            Vector3 gExtents = g.GetComponent<Renderer>().bounds.extents;

            Vector3 pExtents = this.GetComponent<Renderer>().bounds.extents;
            float radius = this.GetComponent<Renderer>().bounds.extents.y;

            // AABB
            if(position.y - radius < gPosition.y + gExtents.y && position.y + radius > gPosition.y - gExtents.y)
            {
                if(position.x - radius < gPosition.x + gExtents.x && position.x + radius > gPosition.x - gExtents.x)
                {
                    //top and bottom checks
                    if(position.x - radius < gPosition.x + gExtents.x && position.x + radius > gPosition.x - gExtents.x)
                    {
                        if (position.y > gPosition.y)
                        {
                            position.y += 0.1f;
                        }
                        else
                        {
                            position.y -= 0.1f;
                        }
                        velocity.y *= -1;
                    }


                    //left and right checks
                    if (position.y - radius < gPosition.y + gExtents.y && position.y + radius > gPosition.y - gExtents.y)
                    {
                        if (position.x > gPosition.x)
                        {
                            position.x += 0.1f;
                        }
                        else
                        {
                            position.x -= 0.1f;
                        }
                        velocity.y *= -1;
                        velocity.x *= -1;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == this.hook)
        {
            if (this.hookOut && this.hookBack)
            {
                if (col.gameObject == this.hook)
                {
                    this.hookOut = false;
                    this.hookBack = false;
                    Destroy(this.hook);
                    this.hook = null;
                    this.playerLight.SetActive(true);
                }
            }
        }
        if (col.gameObject.CompareTag(Constants.TAG_ENEMY) && !this.iFrames)
        {
            this.health -= 1;
            this.iFrames = true;
            this.iFrameStart = Time.time;
            this.healthManager.UpdateHearts(this.health);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == this.hook && (!this.hookBack && this.hook != null))
        {
            this.hookBack = true;
        }
    }
}
