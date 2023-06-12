using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="TestAsset/Player Position")]
public class PlayerPositionSO : ScriptableObject
{
    [SerializeField]
    Vector3 playerCharacterPosition;
    [SerializeField]
    Quaternion playerCharacterRotation;
    [SerializeField]
    Vector3 playerCharacterScale;
    
    public string returnToScene;

    public void SetOnlyPosition(Vector3 pos)
    {
        this.playerCharacterPosition = pos;
    }

    public void SetPosition(Vector3 pos, Quaternion rot, Vector3 sca)
    {
        this.playerCharacterPosition = pos;
        this.playerCharacterRotation = rot;
        this.playerCharacterScale = sca;
    }

    public Vector3 GetPlayerPosition()
    {
        return this.playerCharacterPosition;
    }

    public Quaternion GetPlayerRotation()
    {
        return this.playerCharacterRotation;
    }

    public Vector3 GetPlayerLocalScale()
    {
        return this.playerCharacterScale;
    }

    public Dictionary<string, Vector3> spawnPoint = new Dictionary<string, Vector3>()
    {
        {"Inn", new Vector3(-3.738917f, 1f, 1.199796f) },
        {"Inn_SleepingRooms", new Vector3(-3.831394f, 1f, -1.413297f) },
        {"HendleyVillage", new Vector3(-487.86f, -4.188f, -638.3361f) },
        {"RoadToHendley", new Vector3(-453.2354f, -4.18767f, -527.0212f) },
    };
}
