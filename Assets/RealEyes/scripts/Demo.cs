using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {
	public Renderer eye;
 	public float sizeValue = 1.0F;
	public float dilationValue = 0.5F;
	public float depthValue = 0.1F;
	public float rValue = 0.5F;
	public float gValue = 0.5F;
	public float bValue = 0.6F;
	
	
	void Update () {
		Vector3 mousePos = Input.mousePosition; 
    	
		float xPos = Mathf.Lerp(.5f,-.5f, mousePos.x/Screen.width);
		float yPos = Mathf.Lerp(-.5f,.5f, mousePos.y/Screen.height);
		
    	transform.position = new Vector3(xPos,yPos,.45f);
		
		eye.sharedMaterial.SetFloat("_IrisScale", sizeValue);
		eye.sharedMaterial.SetFloat("_Dilation", dilationValue);
		eye.sharedMaterial.SetFloat("_Parallax", depthValue);
		eye.sharedMaterial.SetColor("_Color", new Color(rValue,gValue,bValue));
	}
	void OnGUI() {
		GUI.Label(new Rect(Screen.width - 175, 25, 100, 20), "Eye Size");
        sizeValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 50, 150, 30), sizeValue, .5F, 1.5F);
		
		GUI.Label(new Rect(Screen.width - 175, 75, 100, 20), "Dilation");
        dilationValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 100, 150, 30), dilationValue, -.5F, 2.0F);
		
		GUI.Label(new Rect(Screen.width - 175, 125, 100, 20), "Eye Depth");
        depthValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 150, 150, 30), depthValue, 0.0F, 0.2F);
		
		GUI.Label(new Rect(Screen.width - 175, 175, 100, 20), "Eye Color R");
		rValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 200, 150, 30), rValue, 0.0F, 1.0F);
		
		GUI.Label(new Rect(Screen.width - 175, 225, 100, 20), "Eye Color G");
		gValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 250, 150, 30), gValue, 0.0F, 1.0F);
		
		GUI.Label(new Rect(Screen.width - 175, 275, 100, 20), "Eye Color B");
		bValue = GUI.HorizontalSlider(new Rect(Screen.width - 175, 300, 150, 30), bValue, 0.0F, 1.0F);	
    }
}
