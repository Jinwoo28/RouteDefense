using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum enemyState
{
    Nomal,
    Electric,
    Fire,
    Water,
    Iced
}

public class EnemyHpbar : MonoBehaviour
{
    [SerializeField] private GameObject ElecIcon = null;
    [SerializeField] private GameObject FireIcon = null;
    [SerializeField] private GameObject IceIcon = null;

    private enemyState enemyState;

    private Transform followenemy = null;
    private Enemy enemyinfo = null;
    private Camera cam = null;
    private Slider hpvalue = null;

    private float MaxHp = 0;
    private float hpmiddle = 0;
    private float hplow = 0;

    private bool setupfinish = false;

    [SerializeField]private Image image = null;

    public void StateChange(enemyState state)
    {
        switch (state)
        {
            case enemyState.Electric:
                ElecIcon.SetActive(true);
                break;
            case enemyState.Fire:
                FireIcon.SetActive(true);
                IceIcon.SetActive(false);
                break;
            case enemyState.Iced:
                IceIcon.SetActive(true);
                FireIcon.SetActive(false);
                break;
        }
    }

    public void ReturnIcon(enemyState state)
    {
        switch (state)
        {
            case enemyState.Electric:
                ElecIcon.SetActive(false);
                break;
            case enemyState.Fire:
                FireIcon.SetActive(false);
                break;
            case enemyState.Iced:
                IceIcon.SetActive(false);
                break;
        }
    }

    private void Start()
    {
        cam = Camera.main;
        hpvalue = this.GetComponent<Slider>();
        
    }

    void Update()
    {
        this.transform.position = cam.WorldToScreenPoint(new Vector3(followenemy.position.x, followenemy.position.y + 1.0f, followenemy.position.z));
        hpvalue.value = enemyinfo.GetHp/MaxHp;


            if (enemyinfo.GetHp > hpmiddle)
            {
                image.color = Color.green;
            }
            else if (enemyinfo.GetHp > hplow)
            {
                image.color = Color.yellow;
            }
            else if (enemyinfo.GetHp > 0)
            {
                image.color = Color.red;
            }
        
        


    }

    public void SetUpEnemy(Enemy _enemyinfo, Transform _followenemy)
    {
        followenemy = _followenemy;
        enemyinfo = _enemyinfo;

        MaxHp = enemyinfo.GetHp;
        hpmiddle = MaxHp* 0.66f;
        hplow = MaxHp * 0.33f;
    }



}
