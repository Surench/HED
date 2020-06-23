using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItemController : MonoBehaviour
{
    [SerializeField] GameObject InactiveImage;
    [SerializeField] GameObject ActiveImage;

    public void Deactivate()
    {
        InactiveImage.SetActive(true);
        ActiveImage.SetActive(false);
    }
    public void Acitvate()
    {
        InactiveImage.SetActive(false);
        ActiveImage.SetActive(true);
    }
}
