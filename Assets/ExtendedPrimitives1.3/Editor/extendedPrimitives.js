/*
	** extendedPrimitives.js for extendedPrimitives's formation facile software
	** 
	** Made by Anthony Cardinale
	** For the extendedPrimitives software
	** 
	** Started on  Sun Jan 23 14:30 2011 
	** Last update Sat Jan 29 00;55 2011
	*/

import System.IO; 																							class extendedPrimitives 																																																																										extends EditorWindow {	@MenuItem ("FormationFacile/Extended Primitives")   																														 static function ShowWindow () {        EditorWindow.GetWindow (extendedPrimitives);    }	var showPrimitives : boolean = true;	private var primitivesScrollPosition : Vector2;
	
	
	private var rectangularScrollPosition : Vector2;
	static var tooltip : String;
	private var pr1_8faces : Texture = Resources.Load("pr1_8faces");	private var pr1_12faces : Texture = Resources.Load("pr1_12faces");	private var pr1_20faces : Texture = Resources.Load("pr1_20faces");	private var pr1_angle : Texture = Resources.Load("pr1_angle");																															private var pr1_anneau : Texture = Resources.Load("pr1_anneau");	private var pr1_arche : Texture = Resources.Load("pr1_arche");	private var pr1_demi_anneau : Texture = Resources.Load("pr1_demi_anneau");	private var pr1_demi_tube : Texture = Resources.Load("pr1_demi_tube");
							
																																																																																								private var pr1_escalier : Texture = Resources.Load("pr1_escalier");	private var pr1_exagone : Texture = Resources.Load("pr1_exagone");	private var pr1_pointe : Texture = Resources.Load("pr1_pointe");	private var pr1_pointe4 : Texture = Resources.Load("pr1_pointe4");																												private var pr1_pointe6 : Texture = Resources.Load("pr1_pointe6");	private var pr1_toit : Texture = Resources.Load("pr1_toit");	private var pr1_tube : Texture = Resources.Load("pr1_tube");	private var pr1_tunel : Texture = Resources.Load("pr1_tunel");
	private var createCollider : boolean = true;
	function OnGUI() {
        showPrimitives = EditorGUILayout.Foldout(showPrimitives, "Extended Primitives"); 
        if (showPrimitives)			{ 						
		rectangularScrollPosition = EditorGUILayout.BeginScrollView (rectangularScrollPosition); 
		GUILayout.BeginHorizontal ();
			
			if (GUILayout.Button (pr1_8faces, GUILayout.Width(40),GUILayout.Height(40) )) {	
 

	

obj_8faces = Instantiate(Resources.Load("8faces"), Vector3(0, 0, 0), Quaternion.identity);				obj_8faces.name = "Des 8 faces";
				if(createCollider){					obj_8faces.AddComponent ("MeshCollider");
				}				Selection.activeGameObject = obj_8faces.gameObject;
			}			if (GUILayout.Button (pr1_12faces, GUILayout.Width(40),GUILayout.Height(40))) {				obj_12faces = Instantiate(Resources.Load("12faces"), Vector3(0, 0, 0), Quaternion.identity);
				obj_12faces.name = "Des 12 faces";				if(createCollider){					obj_12faces.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_12faces.gameObject;			}				if (GUILayout.Button (pr1_20faces, GUILayout.Width(40),GUILayout.Height(40))) {				obj_20faces = Instantiate(Resources.Load("20faces"), Vector3(0, 0, 0), Quaternion.identity);
				obj_20faces.name = "Des 20 faces";				if(createCollider){					obj_20faces.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_20faces.gameObject;
			}					if (GUILayout.Button (pr1_angle, GUILayout.Width(40),GUILayout.Height(40))) {				obj_angle = Instantiate(Resources.Load("angle"), Vector3(0, 0, 0), Quaternion.identity);				obj_angle.name = "Angle";
				if(createCollider){					obj_angle.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_angle.gameObject;			}			
				

				
				if (GUILayout.Button (pr1_anneau, GUILayout.Width(40),GUILayout.Height(40))) {				obj_anneau = Instantiate(Resources.Load("anneau"), Vector3(0, 0, 0), Quaternion.identity);				obj_anneau.name = "Anneau";				if(createCollider){					obj_anneau.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_anneau.gameObject;			}			if (GUILayout.Button (pr1_arche, GUILayout.Width(40),GUILayout.Height(40))) {				obj_arche = Instantiate(Resources.Load("arche"), Vector3(0, 0, 0), Quaternion.identity);
				obj_arche.name = "Arche";
				if(createCollider){					obj_arche.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_arche.gameObject;			}			if (GUILayout.Button (pr1_demi_anneau, GUILayout.Width(40),GUILayout.Height(40))) {				obj_demi_anneau = Instantiate(Resources.Load("demi_anneau"), Vector3(0, 0, 0), Quaternion.identity);				obj_demi_anneau.name = "Demi anneau";
				if(createCollider){					obj_demi_anneau.AddComponent ("MeshCollider");
				}
				Selection.activeGameObject = obj_demi_anneau.gameObject;			}			if (GUILayout.Button (pr1_demi_tube, GUILayout.Width(40),GUILayout.Height(40))) {				obj_demi_tube = Instantiate(Resources.Load("demi_tube"), Vector3(0, 0, 0), Quaternion.identity);				obj_demi_tube.name = "Demi tube";				if(createCollider){					obj_demi_tube.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_demi_tube.gameObject;			}			
				
				
				
				
				GUILayout.EndHorizontal ();				GUILayout.BeginHorizontal ();
			
				if (GUILayout.Button (pr1_escalier, GUILayout.Width(40),GUILayout.Height(40))) {				obj_escalier = Instantiate(Resources.Load("escalier"), Vector3(0, 0, 0), Quaternion.identity);				obj_escalier.name = "Escalier";
				if(createCollider){					obj_escalier.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_escalier.gameObject;			}					if (GUILayout.Button (pr1_exagone, GUILayout.Width(40),GUILayout.Height(40))) {				obj_exagone = Instantiate(Resources.Load("exagone"), Vector3(0, 0, 0), Quaternion.identity);				obj_exagone.name = "Hexagone";				if(createCollider){					obj_exagone.AddComponent ("MeshCollider");
				}				Selection.activeGameObject = obj_exagone.gameObject;			}
				if (GUILayout.Button (pr1_pointe, GUILayout.Width(40),GUILayout.Height(40))) {				obj_pointe = Instantiate(Resources.Load("pointe"), Vector3(0, 0, 0), Quaternion.identity);
				obj_pointe.name = "Pointe";				if(createCollider){					obj_pointe.AddComponent ("MeshCollider");				}
				Selection.activeGameObject = obj_pointe.gameObject;			}				if (GUILayout.Button (pr1_pointe4, GUILayout.Width(40),GUILayout.Height(40))) {				obj_pointe4 = Instantiate(Resources.Load("pointe4"), Vector3(0, 0, 0), Quaternion.identity);				obj_pointe4.name = "Pointe4cotés";				if(createCollider){					obj_pointe4.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_pointe4.gameObject;
			}			
			

			
			if (GUILayout.Button (pr1_pointe6, GUILayout.Width(40),GUILayout.Height(40))) {				obj_pointe6 = Instantiate(Resources.Load("pointe6"), Vector3(0, 0, 0), Quaternion.identity);				obj_pointe6.name = "Pointe6cotés";
				if(createCollider){					obj_pointe6.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_pointe6.gameObject;			}			if (GUILayout.Button (pr1_toit, GUILayout.Width(40),GUILayout.Height(40))) {				obj_toit = Instantiate(Resources.Load("toit"), Vector3(0, 0, 0), Quaternion.identity);				obj_toit.name = "Toit";
				if(createCollider){					obj_toit.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_toit.gameObject;			}			if (GUILayout.Button (pr1_tube, GUILayout.Width(40),GUILayout.Height(40))) {				obj_tube = Instantiate(Resources.Load("tube"), Vector3(0, 0, 0), Quaternion.identity);				obj_tube.name = "Tube";				if(createCollider){					obj_tube.AddComponent ("MeshCollider");				}
				Selection.activeGameObject = obj_tube.gameObject;			}
			if (GUILayout.Button (pr1_tunel, GUILayout.Width(40),GUILayout.Height(40))) {				obj_tunel = Instantiate(Resources.Load("tunel"), Vector3(0, 0, 0), Quaternion.identity);				obj_tunel.name = "Tunnel";
				if(createCollider){					obj_tunel.AddComponent ("MeshCollider");				}				Selection.activeGameObject = obj_tunel.gameObject;
			}
						GUILayout.EndHorizontal ();			EditorGUILayout.EndScrollView();				}	}}