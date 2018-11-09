using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUI : MonoBehaviour {

    public GameObject minimap;
    public GameObject health;
    public GameObject healthPrefab;

	// Use this for initialization
	void Start () {
        //minimap
        Bounds minimapBounds = this.minimap.GetComponent<SpriteRenderer>().bounds;
        Vector2 minimapPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        minimapPos.x -= minimapBounds.extents.x + Constants.UI_TOP_OFFSET;
        minimapPos.y -= minimapBounds.extents.y + Constants.UI_TOP_OFFSET;

        this.minimap.transform.position = minimapPos;

        Bounds healthBounds = this.healthPrefab.GetComponent<SpriteRenderer>().bounds;
        Vector2 healthPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        healthPos.x += healthBounds.extents.x + Constants.UI_TOP_OFFSET;
        healthPos.y -= healthBounds.extents.y + Constants.UI_TOP_OFFSET;

        this.health.transform.position = healthPos;
    }
	
}
