using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    private LayerMask layer;
    private int interactDistance = 15;

    [SerializeField] private Camera MainCamera;
    [SerializeField] private GameObject PickupObject;



    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        layer = LayerMask.NameToLayer("TrashPickup");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.E))
        //{
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f))
            {
                Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward, Color.green , 100.0f);
                //hit.transform.SetParent(gameObject.transform);
                //hit.transform.localPosition = PickupObject.transform.localPosition;
            }

            //RaycastHit hit;
            //
            //if (Physics.Raycast(MainCamera.transform.position, transform.TransformDirection(Vector3.forward), out hit,interactDistance))
            //{
            //    if(hit.ga)
            //    hit.transform.SetParent(gameObject.transform);
            //    hit.transform.localPosition = PickupObject.transform.localPosition;
            //}
        //}
    }
}
