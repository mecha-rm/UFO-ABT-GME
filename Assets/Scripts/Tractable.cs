using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put on objects that can be pulled in by the tractor beam.
public class Tractable : MonoBehaviour
{
    // if 'true', then the object can be pulled by the tractor beam.
    public bool isTractable = true;

    // the object's rigid body.
    public Rigidbody rigidBody;

    // the value for gravity when not in the beam.
    public bool outsideBeamGravity = true;

    // the beam the object is trapped in.
    public TractorBeam tractorBeam;


    // Start is called before the first frame update
    void Start()
    {
        // if no rigidbody was set, look for one.
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody>();

        // grabs value for using gravity.
        if (rigidBody != null)
            outsideBeamGravity = rigidBody.useGravity;
    }

    // called when the item is tracted by the tractor beam.
    public void OnTractorBeamEnter(TractorBeam beam)
    {
        // given beam
        tractorBeam = beam;

        // no longer using gravity since in tractor beam.
        if (rigidBody != null)
            rigidBody.useGravity = false;
    }

    // called when the item is let go by the tractor beam.
    public void OnTractorBeamExit(TractorBeam beam)
    {
        // it's the beam the object was trapped in.
        if(beam == tractorBeam)
            tractorBeam = null;

        // may now use gravity since not in tractor beam.
        if (rigidBody != null)
            rigidBody.useGravity = outsideBeamGravity;
    }

    // Exit Tractor Beam ()

    // Update is called once per frame
    void Update()
    {
        // moves the object upwards.
        if(isTractable && tractorBeam != null)
        {
            // the tractor beam object is enabled (i.e. it is in use)
            if(tractorBeam.isActiveAndEnabled)
            {
                Vector3 direc = tractorBeam.transform.position - transform.position; // movement direction
                direc.y = Mathf.Abs(direc.y); // should always be moving upwards

                // rigid body was found.
                if (rigidBody != null)
                {
                    rigidBody.AddForce(Vector3.Scale(direc.normalized, tractorBeam.TractorSpeed) * Time.deltaTime, ForceMode.Acceleration);
                }
                else // no rigid body, so translate object.
                {
                    rigidBody.transform.Translate(Vector3.Scale(direc.normalized, tractorBeam.TractorSpeed) * Time.deltaTime);
                }
            }
            else // not in use, so leave effect.
            {
                tractorBeam = null;

                if(rigidBody != null)
                    rigidBody.useGravity = outsideBeamGravity; // return gravity back to normal
            }
            

        }
        else if(isTractable && tractorBeam == null) // no tractor beam set (possibly deleted).
        {
            if (rigidBody != null)
                rigidBody.useGravity = outsideBeamGravity; // return gravity back to normal
        }
    }
}