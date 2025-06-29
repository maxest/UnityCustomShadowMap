using UnityEngine;

public class MyLight : MonoBehaviour
{
	public const int cShadowMapTextureDim = 1024;

	public float radius = 25.0f;
	public float angle = 0.5f;
	public Color color = Color.white;
	public float intensity = 1.0f;

	public RenderTexture shadowMapRT;

	void Start()
	{
		RenderTextureDescriptor rtd = new RenderTextureDescriptor();
		rtd.width = cShadowMapTextureDim;
		rtd.height = cShadowMapTextureDim;
		rtd.volumeDepth = 1;
		rtd.msaaSamples = 1;
		rtd.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
		rtd.colorFormat = RenderTextureFormat.Shadowmap;
		rtd.depthBufferBits = 32;

		shadowMapRT = new RenderTexture(rtd);
		shadowMapRT.wrapMode = TextureWrapMode.Clamp;
		shadowMapRT.filterMode = FilterMode.Bilinear;

		GetComponent<Camera>().targetTexture = shadowMapRT;
	}

	void Update()
	{
		Camera camera = GetComponent<Camera>();
		camera.fieldOfView = 2.0f * Mathf.Acos(angle) * Mathf.Rad2Deg;

		if (Input.GetKeyDown(KeyCode.E))
		{
			Vector3 pos = transform.position;
			pos.y = 0.25f;
			transform.position = pos;
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			Vector3 pos = transform.position;
			pos.y = 2.5f;
			transform.position = pos;
		}
	}

	public Vector4 CalcColorMulIntensity()
	{
		return new Vector4(color.r * intensity, color.g * intensity, color.b * intensity, 1.0f);
	}

	public Matrix4x4 GetViewProjTransform()
	{
		Camera camera = GetComponent<Camera>();
		return GL.GetGPUProjectionMatrix(camera.projectionMatrix, true) * camera.worldToCameraMatrix;
	}
}
