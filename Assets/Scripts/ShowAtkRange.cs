using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAtkRange : MonoBehaviour
{

    [SerializeField] private GameObject rangePrefab = null;
    private GameObject[] rangesprite = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowRange(Transform _transform, float _Range)
    {
        Transform tf = _transform;
        for (int i = 0; i < rangesprite.Length; i++)
        {
            Ray ray;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(tf.position.x, tf.position.y + 50f, tf.position.z + _Range), Vector3.down, out hit))
            {
                rangesprite[i].transform.position = hit.point;
                tf.rotation = Quaternion.Euler(0, 20, 0);
            }
        }
    }
}
