using UnityEngine;
using System.Collections;

public class ResetMe : MonoBehaviour {

	private Vector3 startPos;
	private Quaternion startOri;
	
	// Use this for initialization
	void Start () {
		startPos = transform.position;
		startOri = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z))
		{
			transform.position = startPos;
			transform.rotation = startOri;
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}
