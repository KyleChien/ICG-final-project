using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class NavmeshBuilder : MonoBehaviour
{
    private void Awake()
    {
        NavMeshBuilder.BuildNavMesh();
    }
}

