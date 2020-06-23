using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{

    [SerializeField] GridItemController[] GridItemController;

    [SerializeField] RectTransform rectTransform;
    
    public void InitSider(int maxAmount)
    {
        for (int i = 0; i < GridItemController.Length; i++)
        {
            GridItemController[i].Deactivate();
            GridItemController[i].gameObject.SetActive(i<maxAmount);
        }

        rectTransform.sizeDelta = new Vector2(95+((maxAmount-1) * 100),rectTransform.sizeDelta.y);
    }

    public void UpdateSlider(int val)
    {
        for (int i = 0; i < GridItemController.Length; i++)
        {
            if(i < val)
            {
                GridItemController[i].Acitvate();
            }
        }
    }

}
