using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSkill : SkillParent
{
    [SerializeField] private LayerMask enemylayer;
    private int Damage = 0;
    [SerializeField] private float Range = 0;

    [SerializeField] private GameObject ParticleEffect = null;

    private Vector3 target = Vector3.zero;
    public Vector3 SetTarget{ set { target = value; } }


    private float size;
    private float damage;
    private float fireDamage;

    private void Start()
    {
        Damage = (int)SkillSettings.ActiveSkillSearch("Meteor").Value;
        fireDamage = (int)(Damage / 3);
        MultipleSpeed.speedup += SpeedUP;
        this.transform.localScale = this.transform.localScale * (1 + (0.1f * SkillSettings.ActiveSkillSearch("Meteor").CurrentLevel));
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    private void Update()
    {
        //As.volume = SoundSettings.currentsound;
        Vector3 Dir = target - this.transform.position;
        this.transform.position += Dir.normalized * Time.deltaTime * 5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile") || other.CompareTag("Enemy"))
        {
            Boomb();
        }
    }

    public void Boomb()
    {
        Collider[] enemy = Physics.OverlapSphere(this.transform.position, Range, enemylayer);
        {

            Instantiate(ParticleEffect, this.transform.position, Quaternion.identity);
            for(int i = 0; i < enemy.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, enemy[i].transform.position) < Range * 0.3f)
                {
                    enemy[i].GetComponent<Enemy>().EnemyAttacked(Damage);
                    enemy[i].GetComponent<Enemy_Creture>().FireAttacked((int)(Damage/2),5);
                }
                else if(Vector3.Distance(this.transform.position, enemy[i].transform.position) < Range * 0.6f)
                {
                    enemy[i].GetComponent<Enemy>().EnemyAttacked(Damage*0.6f);
                    enemy[i].GetComponent<Enemy_Creture>().FireAttacked((int)(Damage / 3),5);
                }
                else
                {
                    enemy[i].GetComponent<Enemy>().EnemyAttacked(Damage * 0.3f);
                    enemy[i].GetComponent<Enemy_Creture>().FireAttacked((int)(Damage / 4),5);
                }
            }
        }
        
        Destroy(this.gameObject);
    }
}
