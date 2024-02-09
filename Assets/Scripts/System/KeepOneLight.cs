using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOneLight : MonoBehaviour
{
    static KeepOneLight _instance;

    public KeepOneLight Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        //StartCoroutine(DayNightCycle());
    }

    IEnumerator DayNightCycle()
    {
        Vector3 r = new Vector3(0.2f, 0f, 0f);
        while (true)
        {
            this.gameObject.transform.Rotate(r);
            yield return new WaitForSeconds(1);
        }
    }
}
