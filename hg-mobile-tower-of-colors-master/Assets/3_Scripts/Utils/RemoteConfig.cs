using System;
using System.Collections.Generic;
using UnityEngine;

public static class RemoteConfig
{
    public static int INT_LEVEL_GENERATION_BASE_SEED = 15;
    public static bool BOOL_EXPLOSIVE_BARRELS_ENABLED = true;
    public static int INT_EXPLOSIVE_BARRELS_MIN_LEVEL = 2;
    public static bool BOOL_LEVEL_TIMER_ON = true;
    public static float FLOAT_LEVEL_TIMER_SECONDS = 60;
    public static bool BOOL_COLOR_BLIND_ALT_ENABLED = true; 
    public static bool BOOL_PAUSE_BUTTON_ENABLED = true;

    public static int POWER_UP_MULTIBALLS_AMOUNT = 5;
    public static bool TOWER_BOX_SHAPE_ENABLE = false;
    
    // found this data struct to made it more scalable
    public static Dictionary<String,bool> powerUpsEnabled = new Dictionary<String, bool>()
    {
        { "POWER_UP_ONE_ENABLED", true },
        { "POWER_UP_TWO_ENABLED", false },
        { "POWER_UP_THREE_ENABLED", false }
    };
 
    public static Dictionary<String,int> powerUpsUses = new Dictionary<String, int>()
    {
        { "POWER_UP_ONE_USES", 3 },
        { "POWER_UP_TWO_USES", 0 },
        { "POWER_UP_THREE_USES", 0 }
    };
    
}


