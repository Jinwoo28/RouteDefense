using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public GameObject BoombEffect;
    public GameObject Lightning;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100f))
            {
                StartCoroutine(makeeffect(hit.point));
                var item = Instantiate(Lightning);
                item.GetComponent<lightbolt>().SetPos(hit.point, new Vector3(hit.point.x, hit.point.y + 30, hit.point.z));

                Debug.Log(hit.point);


            }
        }
    }

    IEnumerator makeeffect(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(BoombEffect, pos + new Vector3(0,1.5f,0),Quaternion.identity);
    }
}
