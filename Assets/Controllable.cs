using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public float speed = 1;
    public bool inputEnabled = false;
    public float MaxForce = 5;
    public float MinDistance = 0.2f;
    public float MaxDistance = 2f;
    public GameObject other = null;
    public ParticleSystem DeathParticles;
    public Status Status;
    private Rigidbody rb;

    public AudioSource Scream;
    public AudioSource Pop;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Status != Status.Dead) {
            if (other != null)
            {
                var otherRelativePosition = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                if (!inputEnabled)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(otherRelativePosition, Vector3.up), Time.deltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Status != Status.Dead) {
            var otherRelativePosition = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            var distance = Vector3.Distance(transform.position, otherRelativePosition);
            var force = Mathf.Lerp(0, MaxForce, (distance - MinDistance) / MaxDistance);
            var vectorForce = (transform.position - otherRelativePosition) * force;
            vectorForce.y = other.transform.position.y;
            other.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.Force);

            UnityEngine.Debug.DrawLine(other.transform.position, other.transform.position + vectorForce);
            if (rb.velocity.magnitude > 0)
            {
                rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position + new Vector3(rb.velocity.x, 0, rb.velocity.z), Vector3.up), Time.deltaTime));
            }
        }
    }

    internal void Move(Vector3 inputForce)
    {
        var force = inputForce * Time.deltaTime * speed;

        if (rb != null)
            rb.velocity += force;
    }

    public void SelfDestruct()
    {
        rb.constraints = RigidbodyConstraints.None;
        Scream.Play();
        StartCoroutine(SelfDestructCoroutine());
    }
    IEnumerator SelfDestructCoroutine()
    {
        UnityEngine.Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        DeathParticles.transform.SetParent(null);
        DeathParticles.gameObject.SetActive(true);
        Pop.Play();
        UnityEngine.Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        GameObject.Destroy(gameObject);
    }
}
