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
        iniPos.y += 3f;
        float dir = Random.rotation.eulerAngles.z;
        float dist = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0,0, dir) * new Vector3(dist, dist, 0f));
        targetPos.y = iniPos.y + dist;

        //transform.LookAt(2 * transform.position - Camera.main.transform.position);
        transform.Rotate(25f, -90f, 0f);
        transform.localScale = Vector3.zero;

        //TweenMove();
    }

    void TweenMove()
    {
        transform.DOScale(Vector3.one, lifetime);
        transform.DOLocalMoveY(iniPos.y - 1f, lifetime / 2).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            transform.DOLocalMoveY(iniPos.y + Random.Range(minDist, maxDist), lifetime / 2).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });

    }
    void Update()
    {
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        if (timer > lifetime) Destroy(gameObject);
        else if (timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetText(int damage, Color color)
    {
        text.text = damage.ToString();
        text.color = color;
    }
}
