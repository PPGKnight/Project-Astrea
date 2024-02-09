using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingNumbers : MonoBehaviour
{
    public TextMeshPro text;
    public float lifetime;
    public float minDist;
    public float maxDist;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    void Start()
    {
        iniPos = transform.position;
        iniPos.y += 1f;
        targetPos = iniPos +  new Vector3(0f, 0.5f, 0f);

        transform.Rotate(0f, 180f, 0f);
        transform.localScale = Vector3.zero;
    }
    void Update()
    {
        Camera cam = Camera.main;
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        if (timer > lifetime) Destroy(gameObject);
        else if (timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));

        transform.localPosition = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.25f, 0.25f, 0.25f), Mathf.Sin(timer / lifetime));

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }

    public void SetText(int _damage, Color _color)
    {
        text.text = _damage.ToString();
        text.color = _color;
    }
    public void SetText(string _text, Color _color)
    {
        text.text = _text;
        text.color = _color;
    }
}
