using System;
using UnityEngine;
using System.Collections;

public class AnimatedSpriteHighlight : MonoBehaviour
{
    public float Diameter = 0.2f;
    public float ObjectDistance = 0f;
    public GameObject HighlightOverlay;

    private float _startTime;
    public float PulsesPerSecond = 1.5f;
    public float ShrinkFactorXY = 0.5f;
    public bool Rotate = false;

    private bool _isHighlighting = true;
    public bool IsHighlighting
    {
        get { return _isHighlighting; }
        set
        {
            _startTime = Time.time;
            _isHighlighting = value;
            HighlightOverlay.SetActive(value);
        }
    }

    // Use this for initialization
    void Start ()
    {
//        IsHighlighting = true;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (_isHighlighting)
	    {
	        var mainCamPosition = Camera.main.transform.position;
	        var objectPosition = gameObject.transform.position;
	        var facingVector = (mainCamPosition - objectPosition).normalized;
	        HighlightOverlay.transform.position = objectPosition + (facingVector*ObjectDistance);

            var timeDifference = Time.time - _startTime;
            var offset = timeDifference * PulsesPerSecond % 2 - 1;
	        var rotationOffset = offset < 0 ? -offset * offset : offset * offset;
            offset *= offset;
            var scaleFactor = (offset * ShrinkFactorXY + (1 - offset)) * Diameter;
            HighlightOverlay.transform.localScale = Vector3.one * scaleFactor;
            HighlightOverlay.transform.LookAt(objectPosition - facingVector);

	        if (Rotate)
	        {
	            var rotationAngles = HighlightOverlay.transform.localRotation.eulerAngles;
	            rotationAngles.z = rotationOffset*180;
	            HighlightOverlay.transform.localRotation = Quaternion.Euler(rotationAngles);
	        }
	    }
    }
}
