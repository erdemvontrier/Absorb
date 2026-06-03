using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject; //takip edilecek obje
    public float rotationOffset;    

    public float smoothTime;

    private Vector3 _vel;


    private void FixedUpdate()
    {
        var followPos = Vector3.SmoothDamp(transform.position, 
            followObject.position + followObject.forward * rotationOffset, 
            ref _vel, smoothTime); //Smooth takip 
        transform.position = followPos;
    }
}
