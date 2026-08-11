using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OverlapType
{
    Box,
    Sphere
}

[Serializable]
public class OverlapSettings
{
    [Header("Masks")]
    [SerializeField] private LayerMask _searchLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask;

    [Header("Overlap Area")]
    [SerializeField] private Transform _overlapStartPoint;
    [SerializeField] private OverlapType _overlapType;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _boxSize = Vector3.one;
    [SerializeField, Min(0f)] private float _sphereRadius = 1f;

    [Header("Obstacles")]
    [SerializeField] private bool _considerObctacles; //учитывать препятсвия или нет

    [Header("Gizmos")]
    //[SerializeField] private DrawGizmosType _drawGizmosType;
    [SerializeField] private Color _gizmosColor = Color.blue;

    public LayerMask SearchLayerMask { get { return _searchLayerMask; } }
    public LayerMask ObstacleLayerMask { get { return _obstacleLayerMask; } }
    public Transform OverlapStartPoint { get { return _overlapStartPoint; } }
    public OverlapType OverlapType { get { return _overlapType; } }
    public Vector3 Offset { get { return _offset; } }
    public Vector3 BoxSize { get { return _boxSize; } }
    public float SphereRadius { get { return _sphereRadius; } }
    public bool ConsiderObctacles { get { return _considerObctacles; } }
    public Color GizmosColor { get { return _gizmosColor; } }

    public readonly Collider[] OverlapResults = new Collider[32];
}
