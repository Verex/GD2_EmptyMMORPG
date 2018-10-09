using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameText : MonoBehaviour
{
	private TextMesh textMesh;
    private TextMesh TextMesh
	{
		get
		{
			if (textMesh == null)
			{
				textMesh = GetComponent<TextMesh>();
			}

			return textMesh;
		}
	}

    public void SetText(string text)
    {
        TextMesh.text = text;
    }

    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
