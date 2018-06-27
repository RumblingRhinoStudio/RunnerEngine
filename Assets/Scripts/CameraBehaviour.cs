using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    [SerializeField]
    //private float lerpSpeed;

    private Transform objectToFollow;

    private Vector3 distanceToObject;

    //private Transform background;

    //private float distanceToBackground;

    // Use this for initialization
    void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        distanceToObject = transform.position - objectToFollow.transform.position;
        //background = GameObject.FindGameObjectWithTag("Background").transform;
        //distanceToBackground = Mathf.Abs(transform.position.z) + background.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //		transform.position = Vector3.Lerp (transform.position, new Vector3(0f,0f,objectToFollow.transform.position.z) + distanceToObject, lerpSpeed);
        transform.position = new Vector3(0f, 0f, objectToFollow.transform.position.z) + distanceToObject;
        //background.position = new Vector3(background.position.x, background.position.y, transform.position.z + distanceToBackground);
    }
}