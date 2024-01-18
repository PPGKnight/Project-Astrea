using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{
    private void Update()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
     
        if(results.Count <= 0) return;
        if (results[0].gameObject == this.gameObject)
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
