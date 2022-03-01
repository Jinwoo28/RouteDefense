using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoute : MonoBehaviour
{

    [SerializeField] private GameObject[] Arrow;
    Animator Anima;
    
    void Start()
    {
        Anima = this.GetComponent<Animator>();
    }

    public void ShowArrow(int num)
    {

        for(int i = 0; i < 2; i++)
        {
            Arrow[i].SetActive(true);
        }

        switch (num)
        {
            case 1:
                Anima.SetTrigger("RIght");
                break;
            case 2:
                Anima.SetTrigger("Left");
                break;
            case 3:
                Anima.SetTrigger("Stright");
                break;
            case 4:
                Anima.SetTrigger("Back");
                break;

        }
        
    }

    public void ReturnArrow()
    {
        for (int i = 0; i < 2; i++)
        {
            Arrow[i].SetActive(false);
            this.GetComponentInParent<Node>().ReturnColor();
        }
    }



}

