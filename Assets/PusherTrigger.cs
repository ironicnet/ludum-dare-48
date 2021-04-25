using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherTrigger : MonoBehaviour
{
    public Pusher pusher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other) {
        pusher.Push(other.attachedRigidbody);
    }
    private void OnTriggerStay(Collider other) {
        pusher.Push(other.attachedRigidbody);
    }
}
