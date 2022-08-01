using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected Node node = null;
    public Node SetNode { set => node = value; }

    protected int removePrice = 0;
    public int GetPrice { get => removePrice; }

    public virtual void RemoveThis()
    {
        node.RemoveObs();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            other.GetComponent<Tower>().IsGetCanWork = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            other.GetComponent<Tower>().IsGetCanWork = false;
        }
    }
}
