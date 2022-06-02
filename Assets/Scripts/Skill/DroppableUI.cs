using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour,IPointerEnterHandler,IDropHandler,IPointerExitHandler
{

    private RectTransform rect = null;
    private Image image;
    private Color origincolor;

    [SerializeField] private int SlotNum;


    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag.CompareTag("OriginUi"))
        {
            return;
        }

        else if (!eventData.pointerDrag.GetComponent<DraggableUI>().canUseSlot)
        {
            return;
        }

        else if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            eventData.pointerDrag.GetComponent<DraggableUI>().SetSlotNum(SlotNum);
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0.6f, 0.6f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = origincolor;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        origincolor = image.color;
    }
}
