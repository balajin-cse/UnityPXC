using UnityEngine;
using System.Collections;

public class DieLow : MonoBehaviour
{
	void Update()
	{
		if(transform.position.y < -10)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
