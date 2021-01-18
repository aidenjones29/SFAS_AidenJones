using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    [SerializeField] private GameObject PickupObject;
    [SerializeField] private GameObject BinBagDropOff;
    [SerializeField] private GameObject PaperDropOff;
    [SerializeField] private GameObject SockDropOff;

    private float interactDistance = 100.0f;
    private bool holdingItem = false;

    // Start is called before the first frame update
    void Start()
    {
        //layer = LayerMask.NameToLayer("TrashPickup");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(holdingItem == false)
            {
                RaycastHit hit;
                Ray forwardRay = new Ray(transform.position, transform.forward);

                if (Physics.Raycast(forwardRay, out hit, interactDistance))
                {
                    if (hit.transform.tag == "Pickup")
                    {
                        hit.transform.SetParent(gameObject.transform, false);
                        hit.transform.localPosition = PickupObject.transform.localPosition;
                        holdingItem = true;

                        switch (hit.transform.name)
                        {
                            case "BinBag":
                                BinBagDropOff.SetActive(true);
                                break;
                            case "Paper":
                                PaperDropOff.SetActive(true);
                                break;
                            case "Socks":
                                SockDropOff.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else
            {
                RaycastHit hit;
                Ray forwardRay = new Ray(transform.position, transform.forward);

                if (Physics.Raycast(forwardRay, out hit, interactDistance))
                {
                    if (hit.transform.tag == "DropOff")
                    {
                        GameObject childObject = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
                        Object.Destroy(childObject);
                        holdingItem = false;
                    }
                }
            }
        }
    }
}
