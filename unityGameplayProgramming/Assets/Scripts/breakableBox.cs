using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableBox : MonoBehaviour
{
    Collider playerPunch;

    // Start is called before the first frame update
    void Start()
    {
        playerPunch = GameObject.FindWithTag("Player").GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerPunch && other.enabled)
        {
            Destroy(gameObject);
        }
    }





}
