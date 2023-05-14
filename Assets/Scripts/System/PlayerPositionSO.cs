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
}
