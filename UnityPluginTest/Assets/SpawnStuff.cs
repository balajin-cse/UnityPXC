using UnityEngine;
using System.Collections;

public class SpawnStuff : MonoBehaviour 
{
	public GameObject[] prefabs;
	public Transform target;
	public float variance;
	public float rate;
	
	private float cooldown = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cooldown -= Time.deltaTime;
		while(cooldown < 0)
		{
			cooldown += 1.0f/rate;
			var obj = GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation) as GameObject;
			if(target != null) obj.rigidbody.velocity = target.position - transform.position;
			if(variance > 0) obj.rigidbody.velocity = obj.rigidbody.velocity + Random.insideUnitSphere * variance;
		}
	}
}
