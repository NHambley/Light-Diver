using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public Sprite empty, filled;
    public GameObject heartContainer;

    private List<GameObject> container;

    public void SetUp(int max)
    {
        if (this.container != null)
        {
            for (int i = 0; i < this.container.Count; i++)
            {
                Destroy(this.container[i]);
            }
        }

        this.container = new List<GameObject>();

        float width = this.heartContainer.GetComponent<SpriteRenderer>().bounds.extents.x;

        for (int i = 0; i < max; i++)
        {
            GameObject heart = Instantiate(this.heartContainer, this.transform);

            Vector2 pos = new Vector2(this.transform.position.x + ((width * 2) * i), this.transform.position.y);
            heart.transform.position = pos;

            this.container.Add(heart);
        }
    }
	
    public void UpdateHearts(int amount)
    {
        for (int i = 0; i < container.Count; i++)
        {
            Sprite spriteToUse = (i < amount) ? this.filled : this.empty;

            container[i].GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }

}
