using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : SkillParent
{
    [SerializeField] private LayerMask laytermask;
    private bool skillused = false;
    private bool onwater = false;

   


    private bool SetFinish = false;

    private float size;
    private float damage;

    private void Start()
    {
        
        StartCoroutine("FireSkillAct");
        Invoke("Destroythis", 10.0f);
        MultipleSpeed.speedup += SpeedUP;

        damage = SkillSettings.ActiveSkillSearch("Fire").Value;
        this.transform.localScale = this.transform.localScale * (1+ (0.1f*SkillSettings.ActiveSkillSearch("Fire").CurrentLevel));
    }

    private void Update()
    {
        //As.volume = SoundSettings.currentsound;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    private void Destroythis()
    {
        Destroy(this.gameObject);
    }

    IEnumerator FireSkillAct()
    {
        Debug.Log("�۵�");
        while (true)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, laytermask))
            {
                Node node = hit.transform.GetComponent<Node>();
                this.transform.position = new Vector3(node.gridX, node.GetYDepth/2, node.gridY);
                if (Input.GetMouseButtonDown(0))
                {
                    if (!node.GetOnTower&& !onwater)
                    {
                        this.transform.position = new Vector3(node.gridX, node.GetYDepth/2, node.gridY);
                        SetFinish = true;
                        break;
                    }
                }
                 
            }


            yield return null;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            onwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            onwater = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // other.GetComponent<Enemy_Creture>().GetFireDamage = UserInformation.userDataStatic.skillSet[0].GetDamage;
            if (other.GetComponent<Enemy>().GetEnemyType == 0)
            {
                other.GetComponent<Enemy_Creture>().FireAttacked(damage,5);
            }
        }

        if (other.CompareTag("Water")&&SetFinish)
        {
            Destroy(this.gameObject);
        }
    }
}
