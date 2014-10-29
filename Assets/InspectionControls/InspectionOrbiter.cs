using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InspectionOrbiter : SingletonComponent<InspectionOrbiter>
{
    /// <summary>
    /// The Minimum angle of vertical rotation [-90 to 90]
    /// Default: -90
    /// </summary>
    public int MinVerticalRotation = -90;

    /// <summary>
    /// The Maximum angle of vertical rotation [-90 to 90]
    /// Default: 90
    /// </summary>
    public int MaxVerticalRotation = 90;

    /// <summary>
    /// The minimum angle of horizontal rotation [-180 to 180]
    /// Default: -180
    /// </summary>
    public int MinHorizontalRotation = -180;

    /// <summary>
    /// The Maximum angle of horizontal rotation [-180 to 180]
    /// Default: 180
    /// </summary>
    public int MaxHorizontalRotation = 180;

    /// <summary>
    /// The minimum distance away from the object for zoom [Int.MinValue to Int.MaxValue]
    /// Default: 3
    /// </summary>
    public int MinDistance = 3;

    /// <summary>
    /// The maximum distance away from the object for zoom [Int.MinValue to Int.MaxValue]
    /// Default: 10
    /// </summary>
    public int MaxDistance = 10;

    /// <summary>
    /// Multiplier for keyboard camera movement around the y-axis. x/z movement
    /// Default: 250.0f
    /// </summary>
    public float XRotationSpeed = 250.0f;

    /// <summary>
    /// Multiplier for keyboard camera movement around the x/z axis. y movement.
    /// Default: 120.0f
    /// </summary>
    public float YRotationSpeed = 120.0f;

    /// <summary>
    /// Multipler for keyboard camera movement zooming in/out.
    /// Default: 60.0f
    /// </summary>
    public float ZoomSpeed = 60.0f;

    /// <summary>
    /// Multiplier for the mouse camera movement around the y-axis. x/z movement.
    /// Default: 1.0f
    /// </summary>
    public float XMouseMultiplier = 1.0f;

    /// <summary>
    /// Multipler for the mouse camera movement around the x/z axis. y movement.
    /// Default: 1.0f
    /// </summary>
    public float YMouseMultiplier = 1.0f;

    /// <summary>
    /// Multiplier for the mouse camera zoom speed.
    /// Default: 1.0f
    /// </summary>
    public float MouseScrollZoomSpeed = 1.0f;

    /// <summary>
    /// Multiplier for the right-click mouse camera zoom speed.
    /// Default: 1.0f
    /// </summary>
    public float MouseZoomSpeed = 1.0f;

    /// <summary>
    /// Boolean to check if Mouse Input is enabled on this inspector.
    /// Default: true
    /// </summary>
    public bool UseMouseInput = true;

    /// <summary>
    /// Boolean to check is Keyboard Input is enabled on this inspector.
    /// Default: true
    /// </summary>
    public bool UseKeyboardInput = true;

    /// <summary>
    /// Multiplier for the Panning sensitivity when using touch commands.
    /// Default: 1.0f
    /// </summary>
    public float PanSensitivity = 1.0f;

    /// <summary>
    /// Multipler for the Rotation sensitivity when using touch commands. x/z and y axis
    /// </summary>
    public float RotationSensitivity = 1.0f;

	///Multiplier for the Zoom sensitivity when using touch gestures
	public float PinchZoomSensitivity = -0.1f;

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
    /// Offset used to keep track of how many tabs are open from which direction in case multiple tabs can be opened.
    /// </summary>
    public int Offset = 0;

    /// <summary>
    /// Maximum offset + or -
    /// Default: 1
    /// </summary>
    public int MaxOffset = 1;

    /// <summary>
    /// Distance away from the object when zooming
    /// Default: 5
    /// </summary>
    public float Distance = 5f;

    /// <summary>
    /// Offset used to determine how far an object should slide left or right when tabs are open.
    /// Default: 0.8f
    /// </summary>
    public float OffsetFrustumWidth = 0.8f;

    /// <summary>
    /// Default time to take when tweening between positions
    /// Default: 0.5f
    /// </summary>
    public float TransitionTime = 0.5f;

    /// <summary>
    /// List of invalid layers when clicking. Used to determine which layers will stop a click. Should be set to the GUI/NGUI layer
    /// </summary>
    public LayerMask InvalidGrabLayers;

    /// <summary>
    /// Link to the button that will exit out of inspector mode when clicked.
    /// </summary>
    public GameObject CloseButton;

    /// <summary>
    /// Getter/Setter for the object we are inspecting. 
    /// Used to override default values and cache the last known position before going into inspector mode.
    /// </summary>
    private InspectorObject _inspectorObject = null;
    public InspectorObject InspectorObject
    {
        get { return _inspectorObject; }
        set
        {
            if (value == null && _inspectorObject != null)
            {
				_inspectorObject.transform.position = _inspectorObjectHome;
				if (_inspectorObject.GetType () != typeof(InspectorFollowObject)) _inspectorObject.collider.enabled = true;
                _inspectorObject = value;
                _rotationCenter = _homePosition;
                Reset();
            }
            else if (_inspectorObject == null)
            {
                _homePosition = transform.position;
                _homeRotation = transform.rotation;
            }
            
            if (value != null)
            {

				if (value == _inspectorObject) {
					if (RecenterOnClick || RecenterOnSpace)
					{
						ResetView();
					}
					else
					{
                    	return;
                	}
				}
				if (_inspectorObject != null) {
					if (_inspectorObject.GetType () != typeof(InspectorFollowObject)) _inspectorObject.collider.enabled = true;
					_inspectorObject.transform.position = _inspectorObjectHome;
				}
                _rotationCenter = value.transform.position;
                _inspectorObject = value;
				_inspectorObjectHome = _inspectorObject.transform.position;
				if (InspectorObject.GetType () == typeof(InspectorFollowObject)) {
					follow = true;
				}
				else {
					_inspectorObject.collider.enabled = false;
                	Init();
				}
            }
        }
    }

    /// <summary>
    /// Returns the initial position of the orbit object upon view entry used by other scripts to determine tween target (read only)
    /// </summary>
    public Vector3 initPosition
    {
        get
        {
            return InspectorObject.transform.position + (InspectorObject.transform.rotation * new Vector3(InspectorObject.StartDistance, 0f, 0f));
        }
    }

    /// <summary>
    /// This property returns the initial rotation of the orbit object upon view entry used by other scripts to determine tween target (read only)
    /// </summary>
    public Vector3 initRotation
    {
        get
        {
			Vector3 lookRot = Quaternion.LookRotation(InspectorObject.transform.position - initPosition).eulerAngles;
			float tempX, tempY;
			if ((lookRot.x < yMin || lookRot.x > yMax) && InspectorObject.UseVerticalRotation) tempX = lookRot.x - 360f;
			else tempX = lookRot.x;
			if ((lookRot.y < xMin || lookRot.y > xMax) && InspectorObject.UseHorizontalRotation) tempY = lookRot.y - 360f;
			else tempY = lookRot.y;
			return new Vector3 (tempX, tempY, lookRot.z);
        }
    }

    /// <summary>
    /// Boolean value for whether the orbiter code is allowed to run.
    /// </summary>
    public bool AllowOrbiter = true;

    /// <summary>
    /// When clicking on the same object, determines if the rotation/zoom should reset on that object.
    /// Default: false;
    /// </summary>
    public bool RecenterOnClick = false;
	/// <summary>
	/// Whether space bar resets the view.
	/// </summary>
	public bool RecenterOnSpace = true;

    /// <summary>
    /// Boolean used to determine if the Esc key will exit out of Inspector mode.
    /// </summary>
    public bool escToExit = true;

	public RecenterTrigger recenter = RecenterTrigger.rightClick;

    private Vector3 _homePosition;
    private Vector3 _rotationCenter;
    private Quaternion _homeRotation;
	private Vector3 _inspectorObjectHome;
	private Quaternion _initRotation;
    private Camera _myCamera;
    private CharacterController _controller;
    private float _x = 0.0f;
    private float _y = 0.0f;
    private float _zoom = 0.0f;
    private bool _initialized = false;
    private bool _orbitActive;
	private bool _validGrab;
    private bool _validGrabZoom;
	private bool _validGrabOrbit;
	private bool _validGrabPan;
    private bool Pinching;
	private bool StartPinching;
	private bool Panning;
	private bool StartPanning;
	private bool Orbiting;
	private bool StartOrbiting;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private float zMin;
    private float zMax;
	private bool _offsetUpdate;
	private bool follow;
	private float followCameraLinearSpeed = 0.03f;
	private float followCameraAngularSpeed = 10f;
#if UNITY_IPHONE
	private GameObject joystick;
#endif

	/// <summary>
	/// Initializes the Camera, Close button, controller and sets the defaults for x, y, and zoom values.
	/// </summary>
	void Start ()
	{
        yMin = MinVerticalRotation;
        yMax = MaxVerticalRotation;
        xMin = MinHorizontalRotation;
        xMax = MaxHorizontalRotation;
        zMin = MinDistance;
        zMax = MaxDistance;

	    _myCamera = GetComponentInChildren<Camera>();
        //CloseButton = ((ExitInspectorButton)FindObjectOfType(typeof(ExitInspectorButton))).gameObject;
	    _controller = GetComponentInChildren<CharacterController>();
	    _initialized = true;
#if UNITY_IPHONE
		joystick = FindObjectOfType<VCAnalogJoystickNgui>().gameObject;
#endif
	}

    /// <summary>
    /// LateUpdate is called after all other updates.
    /// Used to get keyboard and mouse input to handle rotation/zoom
    /// </summary>
    public void LateUpdate()
    {
		if (follow) {
			Follow ();
		}
		else {
        if (escToExit && Input.GetKeyDown(KeyCode.Escape)) InspectorObject = null;
		if (RecenterOnSpace && Input.GetKeyDown(KeyCode.Space)) InspectorObject = _inspectorObject;
        if (_orbitActive && AllowOrbiter && InspectorObject != null)
        {
            // If we allow keyboard input, handle rotation and zoom
            if (UseKeyboardInput)
            {
				if (_inspectorObject.UseHorizontalRotation) _x -= Input.GetAxis("Horizontal") * XRotationSpeed * 0.02f;
				if (_inspectorObject.UseVerticalRotation) _y += Input.GetAxis("Vertical") * YRotationSpeed * 0.02f;
				if (_inspectorObject.UseZoom) _zoom = Input.GetAxis("Zoom") * ZoomSpeed * 0.02f;
            }
            // Check if we allow mouse input.
            if (UseMouseInput)
            {
                // Left or right-click input
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetAxis ("Mouse ScrollWheel") != 0)
                {
                    _validGrab = true;
                    foreach (Camera cam in Camera.allCameras)
                    {
                        Ray tempRay = cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(tempRay, Mathf.Infinity, InvalidGrabLayers) || !Camera.main.pixelRect.Contains(Input.mousePosition)) _validGrab = false;
                    }
                }
                // If left-click, handle rotation
                if (Input.GetMouseButton(0) && _validGrab)
                {
                    if (_inspectorObject.UseHorizontalRotation) _x += Input.GetAxis("Mouse X") * XMouseMultiplier;
					if (_inspectorObject.UseVerticalRotation) _y -= Input.GetAxis("Mouse Y") * YMouseMultiplier;
                }
                // If right-click, handle zoom
                if (Input.GetMouseButton(1) && _validGrab)
                {
                    if (_inspectorObject.UseZoom) _zoom = -Input.GetAxis("Mouse Y") * MouseZoomSpeed;
                }
                // If scroll wheel, handle zoom.
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    if (_validGrab) _zoom = -Input.GetAxis("Mouse ScrollWheel") * MouseScrollZoomSpeed;
                }
            }
            // Clamp the angle only if the min/max isn't a full circle.
            if (Mathf.Abs(yMin - yMax) < 360)
            {
                _y = ClampAngle(_y, yMin, yMax);
            }
            // Clamp the angle only if the min/max isn't a full circle. Allows for full rotation around the object instead of stopping.
            if (Mathf.Abs(xMin - xMax) < 360)
            {
                _x = ClampAngle(_x, xMin, xMax);
            }

            // Clamp the distance to be between the zoom min and max.
            Distance = Mathf.Clamp(Distance + _zoom, zMin, zMax);

            // Create a rotation based on the x/y movement.
            Quaternion rotation = Quaternion.Euler(_y, _x, 0f);

            // Build a vector moving away from the center of the inspected object along the rotation.
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -Distance) + InspectorObject.transform.position;

            transform.rotation = rotation;
            transform.position = position;

            //if (_zoom != 0f)
            //{
                //OffsetCamera(0);
            //}
        }
		if (_offsetUpdate) UpdateOffset();
		}
    }

	public void Recenter (Transform center) {
		if (AllowOrbiter) {
			iTween.MoveTo(InspectorObject.gameObject, iTween.Hash("position", center.position, "time", TransitionTime, "easetype", "easeInOutQuad"));
		}
	}

	public void Recenter (Vector3 center) {
		if (AllowOrbiter) {
			iTween.MoveTo(InspectorObject.gameObject, iTween.Hash("position", center, "time", TransitionTime, "easetype", "easeInOutQuad"));
		}
	}

    /// <summary>
    /// Takes the angle and ensures it is between the min and max.
    /// </summary>
    /// <param name="value">Value of the angle to clamp</param>
    /// <param name="min">The smallest angle possible</param>
    /// <param name="max">The largest angle possible</param>
    /// <returns></returns>
    private float ClampAngle(float value, float min, float max)
    {
        min %= 360;
        max %= 360;
        value %= 360;
        return Mathf.Clamp(value, min, max);
    }

    /// <summary>
    /// Offsets the camera and allows the offset to be reset to the value passed in.
    /// </summary>
    /// <param name="offset">The value to modify the offset by [-1, 0, 1]</param>
    /// <param name="reset">If true, resets the value to the offset. Defaults to false</param>
    public void OffsetCamera(int offset, bool reset = false)
    {
        if (_initialized)
        {
            if (reset)
            {
                Offset = offset;
            }
            else
            {
                Offset += offset;                
            }
            if (_inspectorObject != null)
            {
				if (Offset == 0) ToggleOffsetUpdate();
                Vector3 newPos = new Vector3(OffsetFrustumWidth * Mathf.Clamp(Offset, -1, 1) * Distance * Mathf.Tan((Camera.main.fieldOfView * Mathf.Deg2Rad) / 2), 0f, 0f);
                iTween.MoveTo(_myCamera.gameObject, iTween.Hash("position", newPos, "time", TransitionTime, "islocal", true, "easetype", "easeInOutQuad","oncomplete","ToggleOffsetUpdate","oncompletetarget",gameObject));
            }
        }
    }

	void ToggleOffsetUpdate() 
	{
		if (Offset != 0) _offsetUpdate = true;
		else _offsetUpdate = false;
	}

	void UpdateOffset()
	{
		if (_inspectorObject != null && _orbitActive)
		{
			Vector3 newPos = new Vector3(OffsetFrustumWidth * Mathf.Clamp(Offset, -1, 1) * Distance * Mathf.Tan((Camera.main.fieldOfView * Mathf.Deg2Rad) / 2), 0f, 0f);
			_myCamera.transform.localPosition = newPos;
		}
	}

    /// <summary>
    /// Resets the camera to its original position and resets the x,y, zoom to the default.
    /// </summary>
    private void Reset()
    {
#if UNITY_IPHONE
		joystick.SetActive (true);
#endif

		if (_initialized)
        {
			follow = false;
            // reset Min/Max Defaults
            yMin = MinVerticalRotation;
            yMax = MaxVerticalRotation;
            xMin = MinHorizontalRotation;
            xMax = MaxHorizontalRotation;
            zMin = MinDistance;
            zMax = MaxDistance;

            if (CloseButton != null) CloseButton.SetActive(false);
            if (_controller != null) _controller.enabled = true;	
			ToggleOrbitActive(false);
            Distance = 0.0f;
            OffsetCamera(0);
            iTween.RotateTo(gameObject, iTween.Hash("rotation", _homeRotation.eulerAngles, "time", TransitionTime, "easetype", "easeInOutQuad"));
            iTween.MoveTo(gameObject, iTween.Hash("position", _homePosition, "time", TransitionTime, "easetype", "easeInOutQuad", "oncomplete", "SwapCameras", "oncompletetarget", gameObject));
        }
    }

    /// <summary>
    /// Runs when going into inspector mode or when the object being inspected changes.
    /// Sets up the x,y,zoom overrides if the object requests an override.
    /// Tweens up close to the object based on the rotation and Start Distance on the object.
    /// </summary>
    private void Init()
    {
        if (_initialized)
        {
            //if (InspectorObject.UseVerticalRotation)
            //{
                yMin = InspectorObject.MinVerticalRotation;
                yMax = InspectorObject.MaxVerticalRotation;
            //}
            //if (InspectorObject.UseHorizontalRotation)
            //{
                xMin = InspectorObject.MinHorizontalRotation;
                xMax = InspectorObject.MaxHorizontalRotation;
            //}
            //if (InspectorObject.UseZoom)
            //{
                zMin = InspectorObject.MinDistance;
                zMax = InspectorObject.MaxDistance;
            //}
#if UNITY_IPHONE
			PanX = InspectorObject.PanX;
			if (PanX) PanBoundsX = InspectorObject.PanBoundsX;
			PanY = InspectorObject.PanY;
			if (PanY) PanBoundsY = InspectorObject.PanBoundsY;
			PanZ = InspectorObject.PanZ;
			if (PanZ) PanBoundsZ = InspectorObject.PanBoundsZ;

			joystick.SetActive (false);
#endif

            if (CloseButton != null) CloseButton.SetActive(true);
            if (_controller != null) _controller.enabled = false;
            ToggleOrbitActive(false);
            iTween.MoveTo(gameObject, iTween.Hash("position", initPosition, "time", TransitionTime, "easetype", "easeInOutQuad"));
            iTween.RotateTo(gameObject, iTween.Hash("rotation", initRotation, "time", TransitionTime, "easetype", "easeInOutQuad", "oncomplete", "ToggleOrbitActive", "oncompletetarget", gameObject, "oncompleteparams", true));
            Distance = _inspectorObject.StartDistance;
            OffsetCamera(0);

            _x = initRotation.y;
            _y = initRotation.x;
            _zoom = 0.0f;
        }
    }

	private void Follow()
	{
		transform.position = Vector3.MoveTowards (transform.position, initPosition, followCameraLinearSpeed);
		transform.eulerAngles = new Vector3 (Mathf.MoveTowardsAngle (transform.eulerAngles.x, initRotation.x, followCameraAngularSpeed), Mathf.MoveTowardsAngle (transform.eulerAngles.y, initRotation.y, followCameraAngularSpeed), Mathf.MoveTowardsAngle (transform.eulerAngles.z, initRotation.z, followCameraAngularSpeed));
	}

	/// <summary>
	/// Resets view to starting position
	/// </summary>
	private void ResetView()
	{
		iTween.MoveTo(gameObject, iTween.Hash("position", _inspectorObjectHome, "time", TransitionTime, "easetype", "easeInOutQuad"));
		iTween.RotateTo(gameObject, iTween.Hash("rotation", initRotation, "time", TransitionTime, "easetype", "easeInOutQuad", "oncomplete", "ToggleOrbitActive", "oncompletetarget", gameObject, "oncompleteparams", true));
		Distance = _inspectorObject.StartDistance;
		OffsetCamera(0);
		
		_x = initRotation.y;
		_y = initRotation.x;
		_zoom = 0.0f;
	}

    /// <summary>
    /// Stops all orbit controls to allow tweening to occur without updating to a specific position.
    /// </summary>
    /// <param name="status"></param>
    private void ToggleOrbitActive(bool status)
    {
        _orbitActive = status;
    }

#if UNITY_IPHONE
    /// <summary>
    /// FingerGestures for drag controls, 1-touch is rotation, 2-touch is panning.
    /// </summary>
    /// <param name="gesture">DragGesture object to control how much dragging has occured and which directions it is being dragged.</param>
    void OnOrb(DragGesture gesture)
    {
		if (InspectorObject != null && AllowOrbiter) {
			if (gesture.Phase == ContinuousGesturePhase.Started)
			{
				_validGrabOrbit = true;
				foreach (Camera cam in Camera.allCameras)
				{
					Ray tempRay = cam.ScreenPointToRay(Input.touches[0].position);
					if (Physics.Raycast(tempRay, Mathf.Infinity, InvalidGrabLayers)) _validGrabOrbit = false;
				}
				if (_validGrabOrbit) {
					Orbiting = true;
					StartOrbiting = true;
				}
			}

			else if (gesture.Phase == ContinuousGesturePhase.Updated && Orbiting && !Pinching && !Panning)
			{
				if (!StartOrbiting) {
            		//Debug.Log ("dragging: "+gesture.DeltaMove);
            		// Drag/displacement since last frame
            		Vector2 deltaMove = gesture.DeltaMove;

            		//Cache the vertical axis.
            		float vertAxis = InspectorObject.UseVerticalRotation ? deltaMove.y : 0f;

            		//Cache the horizontal axis.
            		float horizAxis = InspectorObject.UseHorizontalRotation ? deltaMove.x : 0f;

            		//If there is vertical input, move up or down.
            		if (vertAxis != 0)
            		{
                		_y -= vertAxis * RotationSensitivity;
            		}

            		//If there is horizontal input, move left or right.
            		if (horizAxis != 0)
            		{
                		_x += horizAxis * RotationSensitivity;
            		}

					_y = ClampAngle(_y, yMin, yMax);
					_x = ClampAngle(_x, xMin, xMax);

            		Quaternion rotation = Quaternion.Euler(_y, _x, 0f);
					Vector3 position = rotation * new Vector3(0.0f, 0.0f, -Distance) + InspectorObject.transform.position;

            		transform.rotation = rotation;
            		transform.position = position;
				}
				else StartOrbiting = false;
			}

			else if (Orbiting) Orbiting = false;
		}
    }

	void OnPan(DragGesture gesture)
	{
		//TouchDebug.Instance.Message ("Pan gesture detected");
		if (InspectorObject != null && AllowOrbiter) {
			if (gesture.Phase == ContinuousGesturePhase.Started)
			{
				_validGrabPan = true;
				foreach (Camera cam in Camera.allCameras)
				{
					Ray tempRay = cam.ScreenPointToRay(Input.touches[0].position);
					if (Physics.Raycast(tempRay, Mathf.Infinity, InvalidGrabLayers)) _validGrabPan = false;
				}
				if (_validGrabPan) {
					Panning = true;
					StartPanning = true;
					//TouchDebug.Instance.Message ("Valid pan");
				}
				//TouchDebug.Instance.Message ("Invalid pan!");
			}
			
			else if (gesture.Phase == ContinuousGesturePhase.Updated && Panning)
			{
				if (!StartPanning) {
					// Pan camera
					Vector2 deltaMove = gesture.DeltaMove;
					Quaternion rotation = Quaternion.Euler(_y, _x, 0f);
					Vector3 displacement = rotation * new Vector3(-deltaMove.x, -deltaMove.y, 0f) * PanSensitivity;
					float posZ = PanX ? Mathf.Clamp (InspectorObject.transform.position.z + displacement.z,PanBoundsX.x,PanBoundsX.y) : InspectorObject.transform.position.z;
					float posY = PanY ? Mathf.Clamp (InspectorObject.transform.position.y + displacement.y,PanBoundsY.x,PanBoundsY.y) : InspectorObject.transform.position.y;
					float posX = PanZ ? Mathf.Clamp (InspectorObject.transform.position.x + displacement.x,PanBoundsZ.x,PanBoundsZ.y) : InspectorObject.transform.position.x;
					InspectorObject.transform.position = new Vector3 (posX,posY,posZ);
					Vector3 newPosition = rotation * new Vector3(0.0f, 0.0f, -Distance) + InspectorObject.transform.position;
					transform.position = newPosition;
					//TouchDebug.Instance.Message ("Panning: "+displacement.ToString());
				}
				else StartPanning = false;
			}
			
			else if (Panning) Panning = false;
		}
	}

    /// <summary>
    /// Handles pinching and pulling on touch devices and allows for zooming in and out.
    /// </summary>
    /// <param name="gesture">Pinch Gesture from FingerGestures to control zooming.</param>
    public void OnZoom(PinchGesture gesture)
    {

        if (gesture.Phase == ContinuousGesturePhase.Started)
        {
			_validGrabZoom = true;
			foreach (Camera cam in Camera.allCameras)
			{
				Ray tempRay = cam.ScreenPointToRay(Input.touches[0].position);
				if (Physics.Raycast(tempRay, Mathf.Infinity, InvalidGrabLayers)) _validGrabZoom = false;
			}
           	if (_validGrabZoom) {
				Pinching = true;
				//TouchDebug.Instance.Message ("Zooming started");
				StartPinching = true;
			}
        }
        else if (gesture.Phase == ContinuousGesturePhase.Updated && Pinching)
        {
           	if (!StartPinching)
            {
                Distance = Mathf.Clamp(Distance + (gesture.Delta * PinchZoomSensitivity), zMin, zMax);

                Quaternion rotation = Quaternion.Euler(_y, _x, 0f);
                Vector3 position = rotation * new Vector3(0.0f, 0.0f, -Distance) + InspectorObject.transform.position;

                transform.rotation = rotation;
                transform.position = position;

                OffsetCamera(0);
				//TouchDebug.Instance.Message ("Zooming changed by "+gesture.Delta.ToString ());
			}
			else StartPinching = false;
       	 }
        else
        {
            if (Pinching)
            {
                Pinching = false;
				//TouchDebug.Instance.Message ("Zooming ended");
            }
        }
    }
#endif
}
