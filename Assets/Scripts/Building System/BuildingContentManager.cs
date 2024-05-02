using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContentManager : MonoBehaviour
{
    [SerializeField] BuildingCell[] CellsInGrid;
    [SerializeField] CellNamesSO cellNames;

    private void Start()
    {
        //SendNamesToSO();
    }
    public void SendNamesToSO()
    {
        cellNames.names = new string[CellsInGrid.Length];
        for (int i = 0; i < CellsInGrid.Length; i++)
        {
            cellNames.names[i] = CellsInGrid[i].ObjectNameText.text;
        }
    }
}
