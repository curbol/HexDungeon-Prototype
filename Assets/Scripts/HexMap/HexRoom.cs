﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexRoom : MonoBehaviour
{
    public delegate void ClickAction();

    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    [Range(1, 10)]
    private int size = 6;
    public int Size
    {
        get { return size; }
        set { size = value; }
    }

    [SerializeField]
    [Range(.1F, 2F)]
    private float scale = 1;
    public float Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    [SerializeField]
    private HexOrientation hexOrientation = HexOrientation.FlatUp;
    public HexOrientation HexOrientation
    {
        get { return hexOrientation; }
        set { hexOrientation = value; }
    }

    [SerializeField]
    private UnityEvent cellClicked;
    public UnityEvent CellClicked
    {
        get { return cellClicked; }
        set { cellClicked = value; }
    }

    private HexMetrics hexMetrics;
    public HexMetrics HexMetrics
    {
        get
        {
            if (hexMetrics.InnerRadius <= 0)
            {
                float innerRadius = 0.5f * Scale;
                hexMetrics = new HexMetrics(innerRadius, HexOrientation);
            }

            return hexMetrics;
        }
    }

    private HexMap<HexTile> hexMap;
    public HexMap<HexTile> HexMap
    {
        get
        {
            hexMap = hexMap ?? new HexMap<HexTile>(Size);
            return hexMap;
        }
    }

    private void Awake()
    {
        foreach (IHexCoordinate hexCoordinate in HexMap.Coordinates)
        {
            HexMap[hexCoordinate] = new HexTile
            {
                Color = defaultColor,
            };
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }
    }

    public void ColorCell(Vector3 worldPosition, Color color)
    {
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        IHexCoordinate coordinate = localPosition.ToHexCoordinate(HexMetrics);

        if (!HexMap.ContainsCoordinate(coordinate))
            return;

        HexMap[coordinate].Color = color;
        Debug.Log("touched at " + coordinate.ToString());

        if (CellClicked != null)
        {
            CellClicked.Invoke();
        }
    }
}