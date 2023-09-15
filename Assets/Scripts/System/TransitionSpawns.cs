using System.Collections.Generic;
using UnityEngine;

public static class TransitionSpawns
{
    static Dictionary<string, Vector3> safeSpawns = new Dictionary<string, Vector3>()
    {
        {"InnToInn_SleepingRooms", new Vector3(0f, 1f, -1.5f)},
        {"Inn_SleepingRoomsToInn", new Vector3(5.3f, 3f, 1f)},
        {"InnToHendleyVillage", new Vector3(-489.1f, -5.159f, -687.34f)},
        {"HendleyVillageToInn", new Vector3(-4.118f, 0f, 1.77f)},
        {"HendleyVillageToRoadToHendley", new Vector3(-230f, -3f, -275f)},
        {"RoadToHendleyToHendleyVillage", new Vector3(76f, 2f, 76f)},
        {"HendleyMarket", new Vector3(-499.209991f,-5.19700003f,-664.940002f)},
        {"HendleySmith", new Vector3(-491.68f,-5.159f,-664.34f)},
        {"HendleyMainGate", new Vector3(-486.17f,-5.159f,-639.91f)},
        {"HendleyOldTree", new Vector3(-549.150024f,5.63999987f,-651.98999f)},
        {"HendleyDistrict", new Vector3(-528.31f,5.505f,-701.98f)},
    };

    public static Vector3 ReturnSpawn(string location)
    {
        return safeSpawns[location];
    }
}
