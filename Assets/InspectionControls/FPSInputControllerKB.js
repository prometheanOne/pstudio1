private var motor : CharacterMotor;
private var controller : CharacterController;
var moveSensitivity : float;
var turnSensitivity : float;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
	controller = GetComponent(CharacterController);
}

function FixedUpdate() {
		
	// Get the input vector from keyboard
		var direction : float = Input.GetAxis ("Vertical");
		var rotation : Vector3 = turnSensitivity * new Vector3(0, Input.GetAxis ("Horizontal"), 0);
	
		var directionVector : Vector3;
		if (direction > 0) directionVector = new Vector3(0, 0, 1);
		else if (direction < 0) directionVector = new Vector3(0, 0, -1);
		else directionVector = Vector3.zero;
		if (motor.canControl && controller.enabled) transform.eulerAngles += rotation;
	
		// Apply the direction to the CharacterMotor
		if (controller.enabled) motor.inputMoveDirection = transform.rotation * directionVector * moveSensitivity;
	}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
