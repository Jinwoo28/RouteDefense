using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpNum : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    private float speed = 0f;
    private Color alpha;
    private Camera cam = null;
    private float damage = 0;
    private Transform followenemy = null;
    float X = 0;
    float Z = 0;
    void Start()
    {
        cam = Camera.main;
        Destroy(this.gameObject, 0.75f);
        text = this.GetComponent<TextMeshProUGUI>();
        alpha = text.color;
        text.text = damage.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;
        if (followenemy != null)
        {
            text.transform.position = cam.WorldToScreenPoint(new Vector3(X, followenemy.position.y + 1f + speed * 1.5f, Z));
        }
        else
        {
            return;
        }
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime*1.5f);
        text.color = alpha;
    }

    public void SetUp(Transform _transform, float _damage)
    {
        followenemy = _transform;
        X = followenemy.position.x;
        Z = followenemy.position.z;
        damage = _damage;

    }
}
