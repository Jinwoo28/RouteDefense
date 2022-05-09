using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    [SerializeField] private LayerMask laytermask;
    private bool skillused = false;
    private bool onwater = false;

    private AudioSource As = null;


    private bool SetFinish = false;

    private void Start()
    {
        As = this.GetComponent<AudioSource>();
        StartCoroutine("FireSkillAct");
        Invoke("Destroythis", 10.0f);
        MultipleSpeed.speedup += SpeedUP;
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
        Debug.Log("¿€µø");
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
            other.GetComponent<Enemy_Creture>().GetFireDamage = UserInformation.userDataStatic.skillSet[0].damage;
            other.GetComponent<Enemy_Creture>().FireAttacked();
        }

        if (other.CompareTag("Water")&&SetFinish)
        {
            Destroy(this.gameObject);
        }
    }
}
