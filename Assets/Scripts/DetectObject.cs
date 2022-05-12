using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectObject : MonoBehaviour
{    
    private Transform returntransform = null;
    private Node newnode = null;
    private Vector3 mousePos = Vector3.zero;

    public Node ReturnNode()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Tile");
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                newnode = hit.collider.transform.GetComponent<Node>();
            }

            return newnode;
        }
        return null;
    }

    public Transform ReturnTransform(LayerMask layerMask)
    {
        //마우스위치가 UI가 아니었을 때만
        //using으로 EventSystem을 넣어야 사용 가능
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                Debug.DrawLine(new Vector3(15,10,15), hit.point);
                returntransform = hit.collider.transform;
            return returntransform;
            }

        }
        return null;
    }

    public Vector3 ReturnVector3()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                mousePos = hit.collider.transform.position;
            }

            return mousePos;
        }
        return mousePos;
    }

}
