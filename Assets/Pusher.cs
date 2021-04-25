using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public ParticleSystem Particles;
    public BoxCollider Trigger;
    public float Force;
    public bool Enabled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward);
    }

    public void Push(Rigidbody body) {
        body.AddForce(transform.forward * Force, ForceMode.Force);
    }
}
