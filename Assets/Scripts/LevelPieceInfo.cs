using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPieceInfo : MonoBehaviour{

    private int pieceMapX;
    private int pieceMapY;
    public bool roomCompleted = false;
    private bool playerInside = false;
    MinimapManager minimapManager;

    public int PieceMapX
    {
        get { return this.pieceMapX; }
    }

    public int PieceMapY
    {
        get { return this.pieceMapY; }
    }

    public void SetUp(int xPos, int yPos)
    {
        this.pieceMapX = xPos;
        this.pieceMapY = yPos;
        minimapManager = GameObject.Find(Constants.GAMEOBJECT_MINIMAP).GetComponent<MinimapManager>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == Constants.TAG_PLAYER && !playerInside)
        {
            playerInside = true;
            minimapManager.UpdateMinimapObj(pieceMapX, pieceMapY, Constants.MINIMAP_ICON_ROOMCUR);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == Constants.TAG_PLAYER)
        {
            playerInside = false;
            if (roomCompleted)
            {
                minimapManager.UpdateMinimapObj(pieceMapX, pieceMapY, Constants.MINIMAP_ICON_ROOMDONE);
            }
            else
            {
                minimapManager.UpdateMinimapObj(pieceMapX, pieceMapY, Constants.MINIMAP_ICON_ROOM);
            }
        }
    }
}
