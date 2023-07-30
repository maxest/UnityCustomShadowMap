using UnityEngine;

public class Main : MonoBehaviour
{
	public MyLight myLight;
	public GeometryHierarchy geometryHierarchy;

    void Start()
    {
		geometryHierarchy.myLight = myLight;

	}
}
