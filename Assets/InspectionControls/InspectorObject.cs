using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// This script adds overrides for the default behavior of the InspectorOrbiter.
/// Attach this script to any object that needs to be able to be inspected.
/// Sets up the Orbiter with the override information
/// </summary>
public class InspectorObject : MonoBehaviour
{
    /// <summary>
    /// Default starting distance when entering inspector mode for this object
    /// Default: 5
    /// </summary>
    public float StartDistance = 1.0f;

    /// <summary>
    /// Boolean to determine if Horizontal override should occur
    /// Default: true
    /// </summary>
    public bool UseHorizontalRotation = true;

    /// <summary>
    /// Override for minimum horizontal rotation [-180 to 180]
    /// Default: -90
    /// </summary>
    public int MinHorizontalRotation = -90;

    /// <summary>
    /// Override for the maximum horizontal rotation [-180 to 180]
    /// Default: 90
    /// </summary>
    public int MaxHorizontalRotation = 90;

    /// <summary>
    /// Boolean to determine if Vertical override should occur
    /// Default: true
    /// </summary>
    public bool UseVerticalRotation = true;

    /// <summary>
    /// Override for minimum vertical rotation [-90 to 90]
    /// Default: -45
    /// </summary>
    public int MinVerticalRotation = -45;

    /// <summary>
    /// Override for maximum vertical rotation [-90 to 90]
    /// Default: -45
    /// </summary>
    public int MaxVerticalRotation = 45;

    /// <summary>
    /// Boolean to determine if Zoom override should occur
    /// Default: true
    /// </summary>
    public bool UseZoom = true;

    /// <summary>
    /// Override for minimum zoom [Int.MinValue to Int.MaxValue]
    /// Default: 3
    /// </summary>
    public float MinDistance = 0.1f;

    /// <summary>
    /// Override for maximum zoom [Int.MinValue to Int.MaxValue]
    /// Default: 10
    /// </summary>
    public float MaxDistance = 1.0f;

	///Whether to pan in X axis
	public bool PanX;
	///World coordinates where panning should stop (X axis)
	public Vector2 PanBoundsX;
	///Whether to pan in Y axis
	public bool PanY;
	///World coordinates where panning should stop (Y axis)
	public Vector2 PanBoundsY;
	///Whether to pan in Z axis
	public bool PanZ;
	///World coordinates where panning should stop (Z axis)
	public Vector2 PanBoundsZ;

    /// <summary>
    /// Called when a camera has UICamera from NGUI attached. Sets up Inspection Orbiter's Inspection Object
    /// This gets the ball rolling.
    /// </summary>
    public void OnMouseUpAsButton()
    {
		Debug.Log ("Inspection object clicked");
        if (InspectionOrbiter.Instance != null)
        {
            InspectionOrbiter orbiter = InspectionOrbiter.Instance;
            if (orbiter.AllowOrbiter)
            {
                orbiter.InspectorObject = this;
            }
        }
    }

    /// <summary>
    /// Draw Gizmos to show what rotations are allowed by this object.
    /// </summary>
    public void OnDrawGizmos()
    {
    //    Gizmos.color = Color.green;
    //    float midPointHor = (MinHorizontalRotation + MaxHorizontalRotation) / 2f;
    //    float midPointVer = (MinVerticleRotation + MaxVerticleRotation) / 2f;
    //    Vector3 start = Quaternion.Euler(new Vector3(-midPointVer, MinHorizontalRotation, 0)) * Vector3.forward;
    //    //Vector3 start = Quaternion.AngleAxis(midPointVer, Vector3.right) * Quaternion.AngleAxis(MinHorizontalRotation, Vector3.up) * Vector3.forward;
    //    //start =  * start;
    //    start += gameObject.transform.position;

    //    for (int i = MinHorizontalRotation + 10; i < MaxHorizontalRotation; i += 10)
    //    {
    //        Vector3 end = Quaternion.Euler(new Vector3(-midPointVer, i, 0))*Vector3.forward;
    //        //end = Quaternion.AngleAxis(midPointVer, Vector3.right) * end;
    //        end += gameObject.transform.position;
    //        Gizmos.DrawLine(start, end);
    //        start = end;
    //    }
    //    Vector3 last = Quaternion.Euler(new Vector3(-midPointVer, MaxHorizontalRotation, 0)) * Vector3.forward;

    //    //Vector3 last = Quaternion.AngleAxis(midPointVer, Vector3.right) * Quaternion.AngleAxis(MaxHorizontalRotation, Vector3.up) * Vector3.forward;
    //    //last = Quaternion.AngleAxis(midPointVer, Vector3.right) * last;
    //    last += gameObject.transform.position;

    //    Gizmos.DrawLine(start, last);

    //    Gizmos.color = Color.red;
    //    start = (Quaternion.AngleAxis(MinVerticleRotation, -Vector3.right) * Vector3.forward);
    //    start = Quaternion.AngleAxis(midPointHor, Vector3.up) * start;
    //    start += gameObject.transform.position;

    //    for (int i = MinVerticleRotation + 10; i < MaxVerticleRotation; i += 10)
    //    {
    //        Vector3 end = (Quaternion.AngleAxis(i, -Vector3.right) * Vector3.forward);
    //        end = Quaternion.AngleAxis(midPointHor, Vector3.up) * end;
    //        end += gameObject.transform.position;
    //        Gizmos.DrawLine(start, end);
    //        start = end;
    //    }
    //    last = (Quaternion.AngleAxis(MaxVerticleRotation, -Vector3.right) * Vector3.forward);
    //    last = Quaternion.AngleAxis(midPointHor, Vector3.up) * last;
    //    last += gameObject.transform.position;

    //    Gizmos.DrawLine(start, last);

    }
}
