using System.Collections.Generic;
using UnityEngine;

public static class TransitionSpawns
{
    static Dictionary<string, Vector3> safeSpawns = new Dictionary<string, Vector3>()
    {
        {"InnToInn_SleepingRooms", new Vector3(0f, 1f, -1.5f)},
        {"Inn_SleepingRoomsToInn", new Vector3(5.3f, 3f, 1f)},
        {"InnToHendleyVillage", new Vector3(-490.5736f, -1.550712f, -693.4078f)},
        {"HendleyVillageToInn", new Vector3(-9.897679f, 1f, 6.759513f)},
        {"HendleyVillageToRoadToHendley", new Vector3(-230f, -3f, -275f)},
        {"RoadToHendleyToHendleyVillage", new Vector3(76f, 2f, 76f)},
        {"HendleyMarket", new Vector3(-501.52f,-5.159f,-672.67f)},
        {"HendleySmith", new Vector3(-491.68f,-5.159f,-664.34f)},
        {"HendleyMainGate", new Vector3(-486.17f,-5.159f,-639.91f)},
        {"HendleyOldTree", new Vector3(-486.17f,-5.159f,-639.91f)},
        {"HendleyDistrict", new Vector3(-528.31f,5.505f,-701.98f)},
    };

    public static Vector3 ReturnSpawn(string location)
    {
        return safeSpawns[location];
    }
}
