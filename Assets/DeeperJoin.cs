using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeeperJoin : MonoBehaviour
{
    public GameObject Deeper1;
    public Vector3 Deeper1Offset;
    public GameObject Deeper2;
    public Vector3 Deeper2Offset;

    public Transform[] hands;

    // private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        // lineRenderer = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Deeper1 == null) {
            hands[0].gameObject.SetActive(false);
        }
        if (Deeper2 == null) {
            hands[1].gameObject.SetActive(false);
        }
        if (Deeper1 != null && Deeper2 !=null) {
            var Deeper1Position = Deeper1.transform.position + Deeper1Offset;
            var Deeper2Position = Deeper2.transform.position + Deeper2Offset;

            transform.position = Deeper1Position + (Deeper2Position - Deeper1Position) / 2;
            // lineRenderer.SetPosition(0, start);
            // lineRenderer.SetPosition(1, end);
            hands[0].GetComponent<LineRenderer>().SetPosition(0, hands[0].position);
            hands[0].GetComponent<LineRenderer>().SetPosition(1, Deeper1Position);
            hands[0].LookAt(Deeper1Position);
            hands[1].GetComponent<LineRenderer>().SetPosition(0, hands[1].position);
            hands[1].GetComponent<LineRenderer>().SetPosition(1, Deeper2Position);
            hands[1].LookAt(Deeper2Position);
        } else {
            
            if (Deeper1 != null) {
                transform.position = Deeper1.transform.position + Deeper1.transform.right;
                var Deeper1Position = Deeper1.transform.position + Deeper1Offset;
                hands[0].GetComponent<LineRenderer>().SetPosition(0, hands[0].position);
                hands[0].GetComponent<LineRenderer>().SetPosition(1, Deeper1Position);
            }
            if (Deeper2 != null) {
                transform.position = Deeper2.transform.position + Deeper2.transform.right;
                var Deeper2Position = Deeper2.transform.position + Deeper2Offset;
                hands[1].GetComponent<LineRenderer>().SetPosition(0, hands[1].position);
                hands[1].GetComponent<LineRenderer>().SetPosition(1, Deeper2Position);
            }
        }
    }
}
