using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRope : MonoBehaviour {

    public GameObject hook;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find(Constants.GAMEOBJECT_PLAYER);
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 dir = this.player.transform.position - this.hook.transform.position;

        Vector2 midPoint = ((dir)/2) + (Vector2)(this.hook.transform.position);

        float rot = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 90;

        this.transform.rotation = Quaternion.Euler(0, 0, -rot);
        this.transform.position = midPoint;
        this.transform.localScale = new Vector3(dir.magnitude * Constants.HOOK_ROPE_SCALE*12, this.transform.localScale.y, 0);
    }
}
