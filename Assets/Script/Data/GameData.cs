using UnityEngine;

public static class GameData
{
    public static string LevelId;
    public static ItemData FirstItem;
    public static PlayerData PlayerSelected;

    public static ItemData[] PassifWeaponList;
    public static ItemData[] ActiveWeaponsList;
    public static ItemData[] ModuleList;
    public static ItemData[] Item;
    


    // VARIABLE OBTIMISATION

    public static float updateInterval = 0.05f;
}