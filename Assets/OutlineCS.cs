using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCS : MonoBehaviour
{
    public void AlphaOn()
    {
        Material m = GetComponent<SkinnedMeshRenderer>().material;
        m.SetFloat("_Alpha", 255);
    }

    public void AlphaOff()
    {
        Material m = GetComponent<SkinnedMeshRenderer>().material;
        m.SetFloat("_Alpha", 0);
    }
}
