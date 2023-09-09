using UnityEngine;

public class SetManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Managers");
        this.gameObject.transform.SetParent(manager.transform);
    }
}
