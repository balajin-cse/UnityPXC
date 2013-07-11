using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UsePlugin : MonoBehaviour {
	
	[DllImport("Easy")] private static extern int easyInit();
	[DllImport("Easy")] private static extern void easyReset();
	[DllImport("Easy")] private static extern void easyAcquisitionDistance(int distance);
	[DllImport("Easy")] private static extern void easyScale(float width, float height);
	[DllImport("Easy")] private static extern void easyUpdate();
	[DllImport("Easy")] private static extern float easyGetError();
	[DllImport("Easy")] private static extern void easyShutdown();
	[DllImport("Easy")] private static extern float easyPosX(int bone);
	[DllImport("Easy")] private static extern float easyPosY(int bone);
	[DllImport("Easy")] private static extern float easyPosZ(int bone);
	[DllImport("Easy")] private static extern float easyOriX(int bone);
	[DllImport("Easy")] private static extern float easyOriY(int bone);
	[DllImport("Easy")] private static extern float easyOriZ(int bone);
	[DllImport("Easy")] private static extern float easyOriW(int bone);	
	
	public float gestureScale = 2;
	public Transform gestureCenter;
	public Transform[] bones;
	public Rigidbody[] bodies;
	
	public float scaleWidth=1,scaleHeight=0.9f;
	
	private Vector3[] offsets;
	private readonly int[] parents = new int[]{1,-1,1,2,3,1,5,6,1,8,9,1,11,12,1,14,15};
	
	void Start() 
	{
		var error = easyInit();
		Debug.Log (error);
		easyScale(scaleWidth,scaleHeight);		
		
		offsets = new Vector3[bones.Length];
		for(int i=0; i<bones.Length; ++i) offsets[i] = bones[i].localPosition;
	}
	
	void Update()
	{		
		// Run the tracking engine
		if(Input.GetKeyDown(KeyCode.W)) { scaleHeight += 0.02f; easyScale(scaleWidth, scaleHeight); }
		if(Input.GetKeyDown(KeyCode.A)) { scaleWidth  -= 0.02f; easyScale(scaleWidth, scaleHeight); }
		if(Input.GetKeyDown(KeyCode.S)) { scaleHeight -= 0.02f; easyScale(scaleWidth, scaleHeight); }
		if(Input.GetKeyDown(KeyCode.D)) { scaleWidth  += 0.02f; easyScale(scaleWidth, scaleHeight); }
		if(Input.GetKeyDown(KeyCode.R)) { easyReset(); easyScale(scaleWidth, scaleHeight); }
		easyUpdate();
		
		// Determine the next pose the hand should be in
		var center = new Vector3(easyPosX(1),-easyPosY(1),easyPosZ(1));
		transform.localPosition = (center - gestureCenter.transform.localPosition) * gestureScale;
		var oris = new Quaternion[bones.Length];
		for(int i=0; i<bones.Length; ++i)
		{
			float qx=easyOriX(i), qy=easyOriY(i), qz=easyOriZ(i), qw=easyOriW(i), s=0.707f;
			oris[i] = transform.rotation * new Quaternion((qy-qz)*s, (qw+qx)*s, (qw-qx)*s, (qy+qz)*s);
		}
		
		// Set all bones to their required pose
		for(int i=0; i<bones.Length; ++i)
		{
			if(parents[i] == -1) bones[i].rotation = oris[i];
			else bones[i].localRotation = Quaternion.Inverse(oris[parents[i]]) * oris[i];
			if(parents[i] > i)
			{
				bones[i].localPosition = Matrix4x4.TRS(Vector3.zero, bones[i].localRotation, new Vector3(1,1,1)) * offsets[i];				
			}
		}		
		
		Debug.Log(string.Format("Tracking at {0} with error = {1} (Scale {2},{3})", center, easyGetError(), scaleWidth, scaleHeight));
	}
	
	void FixedUpdate()
	{
		for(int i=0; i<bones.Length; ++i)
		{
			bodies[i].MoveRotation(bones[i].rotation);
			bodies[i].MovePosition(bones[i].position);
		}
	}
	
	void OnDisable () { easyShutdown(); }
}
