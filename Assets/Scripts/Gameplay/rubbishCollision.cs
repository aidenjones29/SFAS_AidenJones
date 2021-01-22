using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rubbishCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Building") //If rubbish is colliding with furniture then they will be moved.
        {
            gameObject.GetComponentInParent<spawnBags>().moveObject(gameObject.GetComponentInParent<spawnBags>().AreaScale, gameObject);
        }
    }
}
