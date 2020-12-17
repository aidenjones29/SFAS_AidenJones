using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject[] portalObjects;

    private enum portalDirection {eDown = 0, eLeft = -90, eUp = 180, eRight = 90};
    private enum Portals { Green, Red };

    private portalDirection currentPortalDirection;
    private Portals currentPortal;

    // Start is called before the first frame update
    void Start()
    {
        currentPortal = Portals.Green;
        currentPortalDirection = portalDirection.eDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            rotatePortalDirection();
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray forwardRay = new Ray(transform.position, transform.forward);

            Debug.DrawRay(transform.position, transform.forward, Color.green);

            if (Physics.Raycast(forwardRay, out hit))
            {
                if(hit.transform.tag != "Portal")
                {
                    portalObjects[(int)currentPortal].SetActive(true);
                    portalObjects[(int)currentPortal].transform.position = hit.point;
                    portalObjects[(int)currentPortal].transform.rotation = hit.transform.rotation;
                    float currentRotation = (int)currentPortalDirection + (Mathf.Round(transform.eulerAngles.z));
                    portalObjects[(int)currentPortal].transform.Rotate(90.0f, 0.0f, (int)currentPortalDirection);

                    if (currentPortal == Portals.Green) currentPortal = Portals.Red;
                    else if (currentPortal == Portals.Red) currentPortal = Portals.Green;
                }
            }
        }

    }


    void rotatePortalDirection()
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
}
