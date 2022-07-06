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
    float Y = 0;
    float Z = 0;

    NumChange NC = new NumChange();

    private int Block = 0;  //0이면 회피 1이면 데미지

    void Start()
    {
        cam = Camera.main;
        Destroy(this.gameObject, 0.75f);
        text = this.GetComponent<TextMeshProUGUI>();
        


        if (damage <= 0&&Block == 0) text.text = "Miss";
        else if(damage <=0&&Block == 1)
        {
            text.text = "Block";
            text.color = Color.black;

        }
        else
        {
            text.text = NC.StringReturnNum(damage);
        }
        alpha = text.color;


    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;

        text.transform.position = cam.WorldToScreenPoint(new Vector3(X, Y + 1f + speed * 1.5f, Z));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime*1.5f);
        text.color = alpha;
    }

    public void SetUp(float _x, float _y, float _z, float _damage, int _block)
    {
        X = _x;
        Y = _y;
        Z = _z;


        damage = _damage;

        Block = _block;

    }
}
