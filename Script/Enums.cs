using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ITEMRANK
{
    NORMAL,
    RARE,
    LEGEND,
    END
}


public enum E_CHARACTER_TYPE
{
    MONSTER,
    NPC,
    PLAYER,
    DUMMY_PLAYER,
    END
}

public enum E_LAYER
{
    Character = 8,
    Ground,
    Monster,
    Bullet,
    END
}

public enum E_DAMAGE_TYPE
{
    NONE,
    NORMAL,
    HEAL,
    DEFENCE_IGNORE,
    INVIN_IGNORE
}

public enum E_SKILL_BEHAVIOR_TYPE
{
    RAYCAST,
    PROJECTILE,
    INSTANCE,
    
}

public enum E_SKILL_TYPE
{
    DOT_DMG,
    DMG,
    HEAL,
    STUN,
    DOT_HEAL,
    MOVE_SLOW,
    ATTACK_SLOW,
    MOVE_SPEEDUP,
    ATTACK_SPEEDUP,
    ATTACK_UP,
    DEFEND_UP,
    INVIN

}


public enum E_ELEMENTAL_TYPE
{
    NONE, // 무속성
    NATURE, // 독
    FIRE, // 불
    WATER, //  
    LIGHTING, // 기절
    LIGHT,  // 
    DARK, 
    END
}

public enum E_TILE_TYPE
{
    groundCube,
    groundCubeGrass,
    waterCube,
    END
}

public enum E_STAT_TYPE
{
    Attack,
    Health,
    HPRegen,
    Mana,
    ManaRegen,
    Defence,
    LightDefence,
    DarkDefence,
    NatureDefence,
    FireDefence,
    WaterDefence,
    LightingDefence,
    AttackSpeed,
    MoveSpeed,
    Vampire,
    GoldBonus,
    CriticalChance,
    CriticalDMG
}

public enum E_TILEDIR
{
    NONE, // 사방이 막혀있다.
    TO_BOTTOM, // 바닥을 향한
    TO_TOP,
    TO_LEFT,
    TO_RIGHT,
    TOP_RIGHT,
    TOP_LEFT,
    BOTTOM_RIGHT,
    BOTTOM_LEFT,
    END
}

public enum E_EFFECT
{
    NONE,
    EarthExplosionTiny,
    END
}

public enum E_EQUIPMENT_TYPE
{
    NONE,
    HEAD,
    WEAPON,
    ARMOR_BODY,
    ARMOR_BOTTOM,
    ARMOR_BOOT,
    ACC1,
    ACC2,
    END
}

public enum E_UIEVENT
{
    NONE,
    BEGIN_DRAG,
    DRAG,
    EXIT_DRAG,
    CLICK,
    DOWN,
    UP,
    END
}


public enum E_CAMERA_TYPE
{
    NONE,
    FIXED_CHARACTER,
    END
}

public enum E_SCENE_UI_TYPE
{
    NONE,
    TITLE,
    INGAME,
    SHOP,
    DEAD,
    VICTORY,
    LOADING,
    INVENTORY,
    MY_HEROES,
    MAIN

}

public enum E_SCENE_TYPE
{
    NONE,
    TITLE,
    IN_GAME,
    LOADING,
    SHOP,
    MAIN
}

#region Sound
public enum E_SFX
{
    NONE,
    CLICK,
    COUNT_DOWN_BEEP,
    COUNT_DOWN_END,
    DEFEAT,
    SPEED_UP,
    VICTORY,
    DEAD,
    COLLECT,
    JUMP,
    ITEM_EVENT,
    GET_COSTUME,
    KILL,
    POP_WOOD,
    END
}

public enum E_BGM
{
    NONE,
    TITLE,
    IN_GAME,
    SHOP,
    END
}
#endregion

public enum E_ITEMCATEGORY
{
    NONE,
    CONSUMPTION,
    WEAPON,
    ARMOR,
    ACC1,
    ACC2,
    QUEST,
    END
}

public enum E_INVEN_CATEGORY
{
    NONE,
    EQUIPMENT,
    QUICKABLE,
    ETC,
    END
}

public enum E_ITEMGRADE
{
    NONE,
    NORMAL,
    RARE,
    UNIQUE,
    LEGEND,
    END
}

