using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {
    // inspector assigned
    [SerializeField] private Transform _trackedObject;
    [SerializeField] [Range(1f, 20f)] private float _updateSpeed = 2f;

    // internal variables
    private Vector3 _startPosition;
    
    private void Start() {
        _startPosition = transform.position;
    }
    
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position,
                                            _trackedObject.position + _startPosition,
                                    _updateSpeed * Time.deltaTime);
    }
}