using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillUiInstantiate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private string skillname;

    [SerializeField] private GameObject SkillUi;

    private GameObject Ui;
    private RectTransform rect;

    private CanvasGroup canvasgroup;

    private Canvas canvas = null;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Ui = Instantiate(SkillUi, eventData.position, Quaternion.identity);
        canvasgroup = Ui.GetComponent<CanvasGroup>();
        rect = Ui.GetComponent<RectTransform>();
        Ui.transform.SetParent(canvas.transform);
        Ui.transform.SetAsLastSibling();
        canvasgroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Ui.transform.parent == canvas.transform)
        {
            Destroy(Ui);
        }
    }
}
