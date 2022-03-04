using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectObject : MonoBehaviour
{
    private Transform returntransform = null;

    public Transform ReturnTransform()
    {
        //마우스위치가 UI가 아니었을 때만
        //using으로 EventSystem을 넣어야 사용 가능
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                returntransform = hit.collider.transform;
            }

            return returntransform;
        }
        return null;
    }


}
