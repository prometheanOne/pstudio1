----------------------------------------
 Virtual Controls Suite for Unity
 © 2012 Bit By Bit Studios, LLC
 Use of this software means you accept the Unity provided license agreement.  
 Please don't redistribute without permission :)
---------------------------------------------------------------
Thanks very much for purchasing Virtual Controls Suite!

Please see the Virtual Controls Suite homepage listed below for an overview of the package
and links to all the items listed below.

Email: 		support@bitbybitstudios.com
Homepage:	http://bitbybitstudios.com/portfolio/virtual-controls-suite
Forums:		http://bitbybitstudios.com/forum/categories/virtual-controls-suite-support
Reference:	http://bitbybitstudios.com/docs/virtual-controls-suite/
Tutorials: 	http://bitbybitstudios.com/2012/06/tutorials/virtual-controls-suite-tutorials/

-----------------
 Importing / Updating VCS
-----------------------------

Follow the instructions in the ReadFirst-WhichToImport.txt file in this directory.

-----------------
 After importing
-----------------------------

Check out the example scenes under VirtualControls/Examples.
You can Copy / Paste the controls from the example scenes into your own scene 
in order to save time setting up controls.  You can also view the code used to
see examples of how to interact with Virtual Controls in script.

-----------------
 Example Scene Descriptions
-----------------------------
VirtualControls/Examples/Scenes:

	- guiTextureSuite, ngui_v2_Suite, ngui_v3_Suite, ezguiSuite
		Demonstration of Joystick, DPad, and Button to move cube and fire particles (pick the one applicable for your UI solution).
		
	- twinStickSchmup(GuiTextures)
		Framework for a dual stick shoot-em-up game.
		Left side of screen controls movement, right side controls firing.

VirtualControls/Examples/FromCharacterController:

	- 3rdPersonController
		VCS Conversion of 3rdPersonController scene from Unity's CharacterController asset package.
		Includes CharacterController scripts converted from JS to C#.
		
	- FirstPersonController
		VCS Conversion of FirstPersonController scene from Unity's CharacterController asset package.
		Includes CharacterController scripts converted from JS to C#.
		
VirtualControls/Examples/FromStandardAssets(Mobile)/ControlSetups:

	- CameraRelativeSetup
		VCS Conversion of CameraRelativeSetup scene from Unity's Standard Assets (Mobile) asset package.
		Includes Standard Assets (Mobile) scripts converted from JS to C#.
		
	- FirstPersonSetup
		VCS Conversion of CameraRelativeSetup scene from Unity's Standard Assets (Mobile) asset package.
		Includes Standard Assets (Mobile) scripts converted from JS to C#.
		
	- PlayerRelativeSetup
		VCS Conversion of CameraRelativeSetup scene from Unity's Standard Assets (Mobile) asset package.
		Includes Standard Assets (Mobile) scripts converted from JS to C#.
		
	- SidescrollSetup
		VCS Conversion of CameraRelativeSetup scene from Unity's Standard Assets (Mobile) asset package.
		Includes Standard Assets (Mobile) scripts converted from JS to C#.

-----------------
 Note to NGUI users
-----------------------------

The free/distributable version of NGUI does not allow for deployment to an iOS device.  
If you're using the NGUI Trial, you will need to remove it from your project
in order to deploy to an iOS device.  You can either get the full version of NGUI instead, or 
fall back to using Unity's built in GUITextures.

-----------------
 Note to EZGUI users
-----------------------------

If you would like to use EZGUI with VCS, you will need to obtain that package
separately in order for VCS to work with EZGUI.  Also, the ezguiSuite example
scene will not have its scripts linked properly when opening, due to the inability
to distribute EZGUI along with VCS.  You will need to relink scripts in the scene.

-----------------
 Changelog
-----------------------------

2.0:
- Added separate packages for various UI solutions.
- Added toggle option to VCButtonBase.
- Removed Linq usage from project.
- Added 7 new example scenes:
	- All of the control setups from Unity's Standard Assets (Mobile) package converted for VCS.
	- 3rd and FirstPersonController setups from Unity's CharacterController package converted for VCS.
	- TwinStickShmup example demonstrating dual stick shoot-em-up setup.
- Fixed a bug where VCAnalogJoystickNGUI moving part could be moved behind base part when PositionAtTouchLocation was in use.

1.5:
- Added example code of Unity's Standard Assets (mobile) FirstPersonControl.js converted for use with VCS: VCFirstPersonControl.txt
- Added PressBeganThisFrame and PressEndedThisFrame properties to VCButtonBase to increase parity with Unity's Input.GetButton related commands.
- Added public TouchWrapper accessor property to VCButtonBase.
- Fixed a bug where joystick's base location could erroneously set to (0, 0) for 1 frame after multiple 1 frame long touches.
- Fixed a bug where more than 5 new touches occurring in a single frame could trigger a null reference error.
- Fixed a bug where deltaPosition of mouse emulated touch in Editor only was not updated properly.

1.4:
- Added Playmaker Updater scripts for Playmaker support via GetProperty actions.
- Moved VCAnalogJoystickBase's Dragging property from protected to public, returns true when Joystick is pressed
- Fixed a bug where NGUI joysticks would in some cases sort behind their "base part" graphic during drag
- Fixed a bug where reloading a scene at runtime would cause controls with VCNames to erroneously report duplicates

1.3.1:
- Included more detailed note in readme for users using neither NGUI or EZGUI.

1.3:
- Added deltaPosition property to VCTouchWrapper class.
- Added positionAtTouchLocationAreaMin and Max properties for VCAnalogJoystickBase.
- Added useLateUpdate property to VCAnalogJoystickBase.
- Added RequireExclusiveTouch property to VCAnalogJoystickBase.

1.2:
- Added distributable version of NGUI to package
- Added TapCount and OnDoubleTap callback to VCAnalogJoystickBase

1.1:
- Added debug keys to all controls, see video here: http://youtu.be/JByiuqGlYKE

1.0:
- Initial implementation