using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour {

    public Sprite room;
    public Sprite roomCur;
    public Sprite roomDone;

    public void UpdateIcon(int cur)
    {
        Sprite spriteToPick = null;

        switch (cur)
        {
            case Constants.MINIMAP_ICON_ROOM:
                spriteToPick = room;
                break;
            case Constants.MINIMAP_ICON_ROOMCUR:
                spriteToPick = roomCur;
                break;
            case Constants.MINIMAP_ICON_ROOMDONE:
                spriteToPick = roomDone;
                break;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteToPick;
    }

}
