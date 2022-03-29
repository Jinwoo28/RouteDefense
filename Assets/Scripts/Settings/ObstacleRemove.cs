using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleRemove : MonoBehaviour
{
    [SerializeField] PlayerState playerstate = null;

    [SerializeField] private GameObject sellinfo = null;

    [SerializeField] private LayerMask layermask;

    [SerializeField] private TextMeshProUGUI price = null;

    private Obstacle obs = null;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
              
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                    
                        sellinfo.SetActive(true);
                        sellinfo.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position + Vector3.up * 2);
                        obs = hit.collider.GetComponent<Obstacle>();
                        price.text = obs.GetPrice.ToString();
                    }
                    else
                    {
                        Debug.Log("พฦดิ");
                        sellinfo.SetActive(false);
                        obs = null;
                    }
                }
            }

        }
    }

    public void ObsRemove()
    {
        if(obs != null)
        {
            if(obs.GetPrice <= playerstate.GetSetPlayerCoin)
            {
                sellinfo.SetActive(false);
                playerstate.GetSetPlayerCoin = obs.GetPrice;
                obs.RemoveThis();
            }
        }
    }

}
