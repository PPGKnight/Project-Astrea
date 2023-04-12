using UnityEngine;

[CreateAssetMenu(menuName="TestAsset/Asset")]
public class PlayerPositionSO : ScriptableObject
{
    public Vector3 playerCharacterPosition { get; private set; }
    public Quaternion playerCharacterRotation { get; private set; }
    public Vector3 playerCharacterScale { get; private set; }
    
    public void SetPosition(Vector3 pos, Quaternion rot, Vector3 sca)
    {
        this.playerCharacterPosition = pos;
        this.playerCharacterRotation = rot;
        this.playerCharacterScale = sca;
    }
}
