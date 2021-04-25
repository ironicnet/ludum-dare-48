using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boundary {
    public Vector3 Center;
    public Vector3 Size;
    public Quaternion Rotation;
}
public class Level : MonoBehaviour
{
    public Transform ExitPosition;
    public Transform[] StartPositions;
    
    public Transform[] CameraLocations;

    public Boundary[] Boundaries;

    public UnityEngine.Events.UnityEvent OnEnter;
    public UnityEngine.Events.UnityEvent OnExit;
    public bool DisableOnExit;

    private GameObject[] deaths;
    // Start is called before the first frame update
    void Start()
    {
        deaths = new GameObject[Boundaries.Length];
        for (int i = 0; i < Boundaries.Length; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position + Boundaries[i].Center;
            cube.transform.localScale = Boundaries[i].Size;
            cube.GetComponent<MeshRenderer>().enabled = false;
            cube.GetComponent<BoxCollider>().isTrigger = true;
            cube.name = $"Boundary {i}";
            cube.transform.SetParent(transform);
            var death = cube.AddComponent<Death>();
            death.Level = this;
            deaths[i] = cube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finish() {
        
        OnExit.Invoke();
        for (int i = 0; i < deaths.Length; i++)
        {
            deaths[i].GetComponent<BoxCollider>().enabled = false;
        }
        ExitPosition.gameObject.GetComponent<BoxCollider>().enabled =false;
    }
    public void Begin() {
        
        OnEnter.Invoke();
        if (deaths !=null ) {
            for (int i = 0; i < deaths.Length; i++)
            {
                deaths[i].GetComponent<BoxCollider>().enabled = true;
            }
        }
        ExitPosition.gameObject.GetComponent<BoxCollider>().enabled =true;
    }
}
