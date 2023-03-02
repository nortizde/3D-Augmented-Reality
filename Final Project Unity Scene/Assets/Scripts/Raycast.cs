using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    int length = 100;
    [SerializeField]
    LineRenderer laserLineRenderer;

    static public RaycastHit hit;
    static public bool hasHit;

    const int CAPSULE = 10;
    const int WALL = 8;

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.TransformDirection(Vector3.forward) * length;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Color lineColor = Color.red;

            switch (hit.transform.gameObject.layer)
            {
                case CAPSULE:
                    lineColor = Color.yellow;
                    break;
                case WALL:
                    lineColor = Color.red;
                    break;
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, lineColor);
            Debug.Log("Did Hit");
            Debug.Log(layerMask);
            laserLineRenderer.SetColors(lineColor, lineColor);

            //endPosition = transform.TransformDirection(Vector3.forward) * hit.distance;
            endPosition = hit.point;
            hasHit = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * length, Color.green);
            Debug.Log("Did not Hit");

            //endPosition = transform.TransformDirection(Vector3.forward) * length;
            laserLineRenderer.SetColors(Color.green, Color.green);
            hasHit = false;
        }

        laserLineRenderer.SetPosition(0, startPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
}
