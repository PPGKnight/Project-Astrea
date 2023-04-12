using UnityEngine;

public class GetPosition : MonoBehaviour
{
    public void GetPositiotSO()
    {
        GameManager.Instance.playerPosition.SetPosition(this.transform.position, this.transform.rotation, this.transform.localScale);
    }
}
