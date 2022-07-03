using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour,IScrollHandler
{
    public ScrollRect ParendSr = null;

    //https://blog.naver.com/PostView.naver?blogId=ihaneter&logNo=222448395541
    public void OnScroll(PointerEventData eventData)
    {
        //Debug.Log("Check");
        ParendSr.OnScroll(eventData);
        
    }

    void Awake()
    {
        ParendSr = this.transform.GetComponentInParent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
