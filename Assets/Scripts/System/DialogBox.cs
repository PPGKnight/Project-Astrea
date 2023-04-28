using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogBox : MonoBehaviour
{
    public Transform box;

    private void OnEnable ()
    {
        box.localPosition = new Vector2(-250, 0);
        box.LeanMoveLocalX(250, 1f);
    }
    public void CloseDialog()
    {
        box.LeanMoveLocalX(-Screen.width, 0.75f);
    }
}