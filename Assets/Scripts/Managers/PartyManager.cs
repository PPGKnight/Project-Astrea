using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBarMain, hpBarFollower1, hpBarFollower2, hpBarFollower3, minimap;

    private List<GameObject> hpBars = new List<GameObject>();

    public void Awake()
    {
        hpBars.Clear();
        hpBars.Add(hpBarMain);
        hpBars.Add(hpBarFollower1);
        hpBars.Add(hpBarFollower2);
        hpBars.Add(hpBarFollower3);

        minimap.SetActive(false);

        foreach (GameObject hpBar in hpBars)
            hpBar.SetActive(false);
    }

    public void SetActiveBar(int party)
    {
        switch (party)
        {
            case 0:
                foreach (GameObject h in hpBars)
                    h.SetActive(false);
                break;
            case 1:
                hpBars[0].SetActive(true);
                break;
            case 2:
            case 3:
            case 4:
                foreach (GameObject h in hpBars)
                    h.SetActive(true);
                break;
        }
    }

    public void SetActiveMinimap(bool b)
    {
        minimap.SetActive(b);
    }
}
