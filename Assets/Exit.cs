using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Controllable controllable = other.GetComponent<Controllable>();
        if (controllable != null) {
            GameManager.Instance.OnExitEnter(controllable, this);
        }
    }
    private void OnTriggerExit(Collider other) {
        Controllable controllable = other.GetComponent<Controllable>();
        if (controllable != null) {
            GameManager.Instance.OnExitExit(controllable, this);
        }
    }
}
