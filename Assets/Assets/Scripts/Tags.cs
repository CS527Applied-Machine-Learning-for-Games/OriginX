using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis
{
    public const string VERTICAL_AXIS = "Vertical"; // when you press up arrow key or 'W' key for forward and when you press the down arrow key or the 'S' key for -ve
    public const string HORIZONTAL_AXIS = "Horizontal"; // when you press 'A' key for forward + and when you press 'S' key for -ve
}

public class Tags
{
    public const string PLAYER_TAG = "Player";
}

public class AnimationTags
{
    public const string WALK_PARAMETER = "Walk";
    public const string DEFEND_PARAMETER = "Defend";
    public const string ATTACK_TRIGGER_1 = "Attack1";
    public const string ATTACK_TRIGGER_2 = "Attack2";
}