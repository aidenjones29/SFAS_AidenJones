using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    [SerializeField] private GameObject PickupObject;
    [SerializeField] private GameObject BinBagDropOff;
    [SerializeField] private GameObject PaperDropOff;
    [SerializeField] private GameObject ToiletDropOff;
    [SerializeField] private GameObject SockDropOff;

    [SerializeField] private TMP_Text UItrashPercentage;
    [SerializeField] private SimpleHealthBar ProgressBar;

    private float interactDistance = 100.0f;
    private int trashCollected = 0;

    private bool holdingItem = false;

    private string objectHolding = "";


    // Start is called before the first frame update
    void Start()
    {
        
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
                        objectHolding = hit.transform.name;
                        hit.transform.SetParent(gameObject.transform, false);
                        hit.transform.localPosition = PickupObject.transform.localPosition;
                        holdingItem = true;

                        switch (objectHolding)
                        {
                            case "BinBag":
                                BinBagDropOff.SetActive(true);
                                break;
                            case "Can":
                                BinBagDropOff.SetActive(true);
                                break;
                            case "Paper":
                                PaperDropOff.SetActive(true);
                                ToiletDropOff.SetActive(true);
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
                        GameObject childObject = this.gameObject.transform.GetChild(0).gameObject;

                        switch (objectHolding)
                        {
                            case "BinBag":
                                BinBagDropOff.SetActive(false);
                                break;
                            case "Paper":
                                PaperDropOff.SetActive(false);
                                ToiletDropOff.SetActive(false);
                                break;
                            case "Socks":
                                SockDropOff.SetActive(false);
                                break;
                            case "Can":
                                BinBagDropOff.SetActive(false);
                                break;
                        }

                        foreach (Transform child in transform)
                        {
                            Destroy(child.gameObject);
                        }
                        holdingItem = false;
                        UpdateUI();
                    }
                }
            }
        }
    }

    void UpdateUI()
    {
        trashCollected += 2;
        UItrashPercentage.text = trashCollected.ToString() + "%";
        ProgressBar.UpdateBar(trashCollected, 100);
        objectHolding = "";
    }
}
