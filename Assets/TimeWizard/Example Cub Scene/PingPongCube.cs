using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongCube : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start ()
    {
	    while(true)
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time, 3), transform.position.y, transform.position.z);
            yield return null;
        }	
	}
}
