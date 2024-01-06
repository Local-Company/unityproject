using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private NetworkPlayer pm_server;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    private void Start()
    {
        pm_server = GetComponent<NetworkPlayer>();
    }
    
    [Client]
    private void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        Camera cam = Camera.main;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxGrappleDistance, Color.red);

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        // if (grappling)
            // lr.SetPosition(0, gunTip.position);
    }

    [Client]
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        GetComponent<SwingingDone>().StopSwing();

        grappling = true;

        Camera cam = Camera.main;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxGrappleDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            Debug.Log(grapplePoint);

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        } else {
            //grapplePoint = cam.transform.position + cam.transform.forward * maxGrappleDistance;

            //Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        // lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm_server.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 0.5f);
    }

    public void StopGrapple()
    {
        pm_server.Cmd_Freeze();

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
}
