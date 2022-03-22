using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger_ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            other.GetComponent<Tower>().SetTowerCanWork = false;
        }

        else if (other.CompareTag("TowerPreview"))
        {
            Debug.Log("dddsdfsdf");
            other.GetComponent<TowerPreview>().SetWeater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            other.GetComponent<Tower>().SetTowerCanWork = true;
        }
        else if (other.CompareTag("TowerPreview"))
        {
            other.GetComponent<TowerPreview>().SetWeater = false;
        }
    }
}
