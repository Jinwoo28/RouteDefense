using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField] private Transform CamPos = null;


    private void Update()
    {
        CamControlFunc();
    }
    private void CamControlFunc()
    {
        // Camera CamPos = Camera.main;

        float Mousewheel = Input.GetAxis("Mouse ScrollWheel");
        int layerMask = 1 << LayerMask.NameToLayer("Floor");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, layerMask))
        {
            Vector3 CamMoveDir = hit.point - CamPos.position;
            CamPos.transform.position += CamMoveDir.normalized *Mousewheel;
        }

        //if (CamPos.transform.position.y >= 3 && CamPos.transform.position.y <= 25)
        //{
        //    CamPos.transform.position += new Vector3(0, -Mousewheel * 2, 0);
        //}

        if (CamPos.transform.position.y > 25)
        {
            CamPos.transform.position += new Vector3(0, -1, 0);
        }

        if (CamPos.transform.position.y < 3)
        {
            CamPos.transform.position += new Vector3(0, +1, 0);
        }


        if (Input.GetKey(KeyCode.W)) { CamPos.transform.position += CamPos.transform.forward * Time.deltaTime * 3; }
        if (Input.GetKey(KeyCode.S)) { CamPos.transform.position -= CamPos.transform.forward * Time.deltaTime * 3; }
        if (Input.GetKey(KeyCode.A)) { CamPos.transform.position -= CamPos.transform.right * Time.deltaTime * 3; }
        if (Input.GetKey(KeyCode.D)) { CamPos.transform.position += CamPos.transform.right * Time.deltaTime * 3; }


    }

    public void TurnRIght() { StartCoroutine("CamTurnRight"); }
    public void TurnLeft() { StartCoroutine("CamTurnLeft"); }

    IEnumerator CamTurnLeft()
    {

        while (true)
        {
            this.transform.Rotate(0, +45 * Time.deltaTime, 0);
            if (Input.GetMouseButtonUp(0)) yield break;

            yield return null;
        }

    }

    IEnumerator CamTurnRight()
    {
        while (true)
        {
            this.transform.Rotate(0, -45 * Time.deltaTime, 0);
            if (Input.GetMouseButtonUp(0)) yield break;

            yield return null;
        }

    }

}
