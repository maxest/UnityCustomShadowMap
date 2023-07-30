using UnityEngine;

public class MyLight : MonoBehaviour
{
	public const int cShadowMapTextureDim = 1024;

	public float radius = 25.0f;
	public float angle = 0.5f;
	public Color color = Color.white;
	public float intensity = 1.0f;

	void Start()
    {
        
    }

    void Update()
    {
        
    }

	public Vector4 CalcColorMulIntensity()
	{
		return new Vector4(color.r * intensity, color.g * intensity, color.b * intensity, 1.0f);
	}
}
