using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour
{
    public bool IsOccupied = false;

    private void Update()
    {
        if (IsOccupied)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
