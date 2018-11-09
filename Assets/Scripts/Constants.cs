using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    //defaults
    public const int MAP_DEFAULT_WIDTH = 10;
    public const int MAP_DEFAULT_HEIGHT = 10;

    public const float GENERATION_DEPTH_MODIFIER = 0.3f;

    //
    public const float LEVEL_Z_POS = 0.2f;
    public const int MINIMAP_Z_POS = 0;
    public const float UI_TOP_OFFSET = 0.1f;

    //global game objects
    public const string GAMEOBJECT_MINIMAP = "Minimap";
    public const string GAMEOBJECT_LEVEL = "Level";
    public const string GAMEOBJECT_PLAYER = "Player";
    public const string GAMEOBJECT_LEVELGEN = "LevelGenerator";

    //room wall gameobject declarations
    public const string BACKGROUND_TOP_DOOR = "top-door";
    public const string BACKGROUND_BOTTOM_DOOR = "bottom-door";
    public const string BACKGROUND_LEFT_DOOR = "left-door";
    public const string BACKGROUND_RIGHT_DOOR = "right-door";

    //minimap values
    public const int MINIMAP_ICON_ROOM = 0;
    public const int MINIMAP_ICON_ROOMCUR = 1;
    public const int MINIMAP_ICON_ROOMDONE = 2;

    //global tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_PROJECTILE = "Projectile";
    public const string TAG_ENEMY = "Enemy";

    //player speed consts
    public const float PLAYER_ROTATE_SPEED_STILL_MOD = 0.8f;
    public const float PLAYER_ROTATE_SPEED_BACKWARDS_MOD = 0.3f;

    //hook speed consts
    public const float HOOK_SPEED_MAX_MOD = 3f;
    public const float HOOK_SPEED_MIN_MOD = 0.8f;
    public const float HOOK_PULL_SPEED_DECAY = 0.9f;
    public const float HOOK_ROPE_SCALE = 0.18f;
    public const float HOOK_MAX_DAMAGE_SPEED = 0.1f;

    //lantern stuff
    public const float LIGHT_TIME_TO_MOUSE = 0.2f;
    public const float LIGHT_SPEED = 0.2f;
    public const float LIGHT_TIME = 10f;

    //enemy consts
    public const float ENEMY_PARTICLE_MAX_EMMISION = 50f;
    public const float ENEMY_PARTICLE_DESTROY_TIME = 15f;

}
