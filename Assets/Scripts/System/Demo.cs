using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public void Finish()
    {
        GameManager.Instance.worldTime = 0;
        GameObject go = Resources.Load<GameObject>("Prefabs/ThankYouScreen");
        Instantiate(go);
    }
}
