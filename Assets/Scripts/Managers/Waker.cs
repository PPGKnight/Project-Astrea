using UnityEngine;

public class Waker : MonoBehaviour
{
    private void Awake()
    {
        GameObject managers = Resources.Load("Prefabs/Managers") as GameObject;
        if (!GameObject.Find("Managers"))
            Instantiate(managers);
    }
}
