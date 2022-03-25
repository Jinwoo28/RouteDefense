using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFunc : MonoBehaviour
{
    [SerializeField] private GameObject FireObj = null;
    private GameObject fireobj = null;
    private bool fireskillAct = false;

    private bool skillused = false;


    private void Update()
    {
        if (fireskillAct)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(fireobj);
                fireskillAct = false;
            }
        }
    }

    public void FireSkill()
    {
        fireobj =  Instantiate(FireObj);
        fireskillAct = true;
    }

    public void TowerUpSkill()
    {
        if(!skillused)
        StartCoroutine("TowerUp");
    }

    IEnumerator TowerUp()
    {
        
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Transform obj = hit.transform;
                if (obj.CompareTag("Tower"))
                {
                    Tower tower = obj.GetComponent<Tower>();
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (tower.GetStep == 1)
                        {
                            skillused = true;
                            tower.TowerUpSkill();
                            break;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                break;
                skillused = false;
            }

            yield return null;
        }
    }

}
