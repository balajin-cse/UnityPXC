using UnityEngine;
using System.Collections;

public class FlipMeshFaces : MonoBehaviour 
{	
	void Start () 
	{
		var filter = GetComponent<MeshFilter>();
		if(filter != null) filter.mesh = FlipMeshTriangles(filter.mesh);
		
		var renderer = GetComponent<SkinnedMeshRenderer>();
		if(renderer != null) renderer.sharedMesh = FlipMeshTriangles(renderer.sharedMesh);
	}
	
	private Mesh FlipMeshTriangles(Mesh inMesh)
	{
		var mesh = new Mesh();
		if(inMesh.vertices    != null) mesh.vertices    = inMesh.vertices;
		if(inMesh.normals     != null) mesh.normals     = inMesh.normals;
		if(inMesh.tangents    != null) mesh.tangents    = inMesh.tangents;
		if(inMesh.uv          != null) mesh.uv 		    = inMesh.uv;
		if(inMesh.uv1         != null) mesh.uv1 	    = inMesh.uv1;
		if(inMesh.uv2         != null) mesh.uv2 	    = inMesh.uv2;
		if(inMesh.colors      != null) mesh.colors 	    = inMesh.colors;
		if(inMesh.colors32    != null) mesh.colors32    = inMesh.colors32;
		if(inMesh.boneWeights != null) mesh.boneWeights = inMesh.boneWeights;				
		if(inMesh.triangles   != null) mesh.triangles 	= FlipTriangles(inMesh.triangles);
		for(int i=0; i<inMesh.subMeshCount; ++i) mesh.SetTriangles(FlipTriangles(inMesh.GetTriangles(i)), i);
		if(inMesh.bindposes   != null) mesh.bindposes = inMesh.bindposes;		
		mesh.bounds = inMesh.bounds;
		return mesh;
	}
	
	private int[] FlipTriangles(int[] tris)
	{
		var triangles = new int[tris.Length];
		for(int i=0, n=triangles.Length/3; i<n; ++i)
		{
			triangles[i*3+0] = tris[i*3+2];
			triangles[i*3+1] = tris[i*3+1];
			triangles[i*3+2] = tris[i*3+0];
		}
		return triangles;
	}	
}
