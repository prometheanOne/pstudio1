using UnityEngine;

public class SingletonComponent<T> : MonoBehaviour where T : Component
{
	#region Singleton Instance
	public static T instance = null;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)GameObject.FindObjectOfType(typeof(T));
				if (instance == null) Debug.LogWarning("Instance is not found.");
			}
		return instance;
		}
	}
	#endregion
}
