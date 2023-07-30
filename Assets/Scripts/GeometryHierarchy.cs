using UnityEngine;

public class GeometryHierarchy : MonoBehaviour
{
	public MyLight myLight;

	void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			Transform t = gameObject.transform.GetChild(i);
			MeshRenderer mr = t.GetComponent<MeshRenderer>();

			Material[] newMats = new Material[mr.sharedMaterials.Length];
			for (int j = 0; j < mr.sharedMaterials.Length; j++)
			{
				Material m = mr.sharedMaterials[j];
				Texture mainTex = m.GetTexture("_MainTex");

				newMats[j] = new Material(Shader.Find("CustomShadowMap/ForwardLit"));
				newMats[j].SetTexture("_MainTex", mainTex);
			}
			mr.sharedMaterials = newMats;
		}
    }

	void Update()
	{
		Vector3 lightPos = myLight.transform.position;
		Vector3 lightDir = myLight.transform.forward;

		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			Transform t = gameObject.transform.GetChild(i);
			MeshRenderer mr = t.GetComponent<MeshRenderer>();

			for (int j = 0; j < mr.sharedMaterials.Length; j++)
			{
				Material m = mr.sharedMaterials[j];

				m.SetVector("_LightPosAndRadius", new Vector4(lightPos.x, lightPos.y, lightPos.z, myLight.radius));
				m.SetVector("_LightDirAndAngle", new Vector4(lightDir.x, lightDir.y, lightDir.z, myLight.angle));
				m.SetVector("_LightColor", myLight.CalcColorMulIntensity());
			}
		}
	}
}
