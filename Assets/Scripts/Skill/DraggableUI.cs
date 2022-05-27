using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private Transform[] SlotPos;

    [SerializeField] private SkillSettings skillsettings;

    private Transform canvas;

    private Transform previousParent;

    private Transform originPos;

    private RectTransform rect;
    private CanvasGroup canvasgroup;

    [SerializeField] private string SkillName = null;
    public string GetName => SkillName;

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasgroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }

        canvasgroup.blocksRaycasts = true;
    }

    private void Awake()
    {
        originPos = transform.parent;
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasgroup = GetComponent<CanvasGroup>();
        
    }

    private void Start()
    {
        SetSlot();
    }

    public void ReturnUi()
    {
        transform.SetParent(originPos);
        rect.position = originPos.GetComponent<RectTransform>().position;
        skillsettings.ChangeSkillSlotNum(0, SkillName);
    }

    public void SetSlotNum(int slotnum)
    {
        skillsettings.ChangeSkillSlotNum(slotnum, SkillName);
    }

    public Transform GetOriginPos => originPos;

    public void SetSlot()
    {

        switch (skillsettings.ASearchSkill(SkillName).Slot)
        {
            case 1:
                transform.SetParent(SlotPos[0]);
                rect.position = SlotPos[0].GetComponent<RectTransform>().position;
                break;
            case 2:
                transform.SetParent(SlotPos[1]);
                rect.position = SlotPos[1].GetComponent<RectTransform>().position;
                break;
            case 3:
                transform.SetParent(SlotPos[2]);
                rect.position = SlotPos[2].GetComponent<RectTransform>().position;
                break;
            case 4:
                transform.SetParent(SlotPos[3]);
                rect.position = SlotPos[3].GetComponent<RectTransform>().position;
                break;
        }
    }


}
