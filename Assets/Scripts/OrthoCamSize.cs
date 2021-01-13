using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoCamSize : MonoBehaviour
{
    [SerializeField] private Camera normalCamera;
    [SerializeField] private Camera dephCamera;

    private void Awake()
    {
        normalCamera.orthographicSize = Camera.main.orthographicSize;
        dephCamera.orthographicSize = Camera.main.orthographicSize;
    }
}
