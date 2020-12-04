using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;


    private enum portalDirection {eDown, eLeft, eUp, eRight };
    private portalDirection currentPortalDirection;

    // Start is called before the first frame update
    void Start()
    {
        currentPortalDirection = portalDirection.eDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            switch (currentPortalDirection)
            {
                case portalDirection.eDown:
                    currentPortalDirection = portalDirection.eLeft;
                    break;
                case portalDirection.eLeft:
                    currentPortalDirection = portalDirection.eUp;
                    break;
                case portalDirection.eUp:
                    currentPortalDirection = portalDirection.eRight;
                    break;
                case portalDirection.eRight:
                    currentPortalDirection = portalDirection.eDown;
                    break;
                default:
                    break;
            }

            crosshair.transform.Rotate(0.0f, 0.0f, -90.0f);
        }

        if(Input.GetMouseButtonDown(0))
        {

        }

    }
}
