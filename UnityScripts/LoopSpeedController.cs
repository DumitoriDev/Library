using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class LoopSpeedController : MonoBehaviour {

    private Animation loopAnimation;
    [SerializeField] float speed=1;
	void Start () {
        loopAnimation = GetComponent<Animation>();
        loopAnimation["LoopMover"].speed = speed;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
