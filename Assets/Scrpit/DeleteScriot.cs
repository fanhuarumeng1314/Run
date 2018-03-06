using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScriot : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DeltPlan());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DeltPlan()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        yield return null;
    }
}
