using UnityEngine;
using System.Collections;
///To be added to all tab buttons whose associated tabs are intended to affect the ofsetting of the inspection camera view

public class TabButton : MonoBehaviour
{
	private enum tabSide {
		left = -1,
		right = 1
	}
	///Disable inspection camera offsetting for this tab
	[SerializeField]
	private bool doNotOffsetCamera;
	///Which side of the screen this tab is on
	[SerializeField]
	private tabSide tabScreenSide;
	private bool tabState;
	[SerializeField]
	private bool tabStartsOpen;
	
	void Start() {
		if (tabStartsOpen) tabState = true;
	}
	
	void OnClick() {
		if (!doNotOffsetCamera) {
            //if (!tabState) MouseOrbit.Instance.offsetDirection += (int)tabScreenSide;
            //else MouseOrbit.Instance.offsetDirection -= (int)tabScreenSide;
            //tabState = !tabState;
            if (!tabState)
            {
                InspectionOrbiter.Instance.OffsetCamera((int)tabScreenSide);
            }
            else
            {
                InspectionOrbiter.Instance.OffsetCamera(-(int)tabScreenSide);
            }
            tabState = !tabState;
		}
	}
}
