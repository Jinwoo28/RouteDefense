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

    private DetectObject detect = new DetectObject();

    private Obstacle obs = null;

    private void Update()
    {
        LayerMask layer = 1 << LayerMask.NameToLayer("Obstacle");

        if (Input.GetMouseButtonDown(0))
        {
            if (detect.ReturnTransform(-1) != null)
            {
                if (detect.ReturnTransform(-1).GetComponent<Obstacle>() != null)
                {
                    obs = detect.ReturnTransform(-1).GetComponent<Obstacle>();
                    sellinfo.SetActive(true);
                    price.text = "제거비용 : " + obs.GetPrice.ToString();
                }

                else
                {
                    sellinfo.SetActive(false);
                    obs = null;
                }
            }

            //else if(obs == null)
            //{
               
            //    obs = null;
            //}
        }

        if (obs != null)
        {
            Vector3 pos = obs.gameObject.transform.position;
            sellinfo.transform.position = Camera.main.WorldToScreenPoint(new Vector3(pos.x + 1, pos.y + 2, pos.z));
        }
    }

    public void ObsRemove()
    {
       if(obs.GetPrice <= playerstate.GetSetPlayerCoin)
       {
           playerstate.GetSetPlayerCoin = obs.GetPrice;
           obs.RemoveThis();
           sellinfo.SetActive(false);
       }
    }

}
