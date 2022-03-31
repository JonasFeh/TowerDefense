using Assets.Utility.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float panSpeed = 30f;
    [SerializeField] public float panBorderThickness = 10f;
    [SerializeField] public float scrollSpeed = 10f;
    [SerializeField] public float maxScrollDistance = 100f;
    [SerializeField] public float minScrollDistance = 10f;
    private const int SCROLL_SPEED_COMPENSATION = 1000;

    void Update()
    {
        // forward
        var aMultiplicator = GetKeyAndMouseCombined("w");
        if (aMultiplicator > 0)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime * aMultiplicator, Space.World);
        }

        // left
        aMultiplicator = GetKeyAndMouseCombined("a");
        if (aMultiplicator > 0)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime * aMultiplicator, Space.World);
        }

        // backward
        aMultiplicator = GetKeyAndMouseCombined("s");
        if (aMultiplicator > 0)
            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime * aMultiplicator, Space.World);
            }

        // right
        aMultiplicator = GetKeyAndMouseCombined("d");
        if (aMultiplicator > 0)
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime * aMultiplicator, Space.World);
            }

        float aScroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 aPosition = transform.position;
        aPosition.y -= aScroll * scrollSpeed * Time.deltaTime * SCROLL_SPEED_COMPENSATION;
        aPosition.y = Mathf.Clamp(aPosition.y, minScrollDistance, maxScrollDistance );

        transform.position = aPosition;
    }
    private int GetKeyAndMouseCombined(string theKey)
    {
        switch (theKey)
        {

            case "w":
                return Convert.ToInt32(Input.GetKey(theKey)) + Convert.ToInt32(Input.mousePosition.y >= Screen.height - panBorderThickness);
            case "a":
                return Convert.ToInt32(Input.GetKey(theKey)) + Convert.ToInt32(Input.mousePosition.x <= panBorderThickness);
            case "s":
                return Convert.ToInt32(Input.GetKey(theKey)) + Convert.ToInt32(Input.mousePosition.y <= panBorderThickness);
            case "d":
                return Convert.ToInt32(Input.GetKey(theKey)) + Convert.ToInt32(Input.mousePosition.x >= Screen.width - panBorderThickness);
            default:
                return 0;
        }
    }
}
