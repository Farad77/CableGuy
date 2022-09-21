using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 Offset = Vector3.zero;
    //[SerializeField] private float RotationSpeed = 5.0f;
    [SerializeField] private float FollowSpeed = 3.0f;
    
    private Transform _target = null;
    
    
    public void Initialize(Transform transform)
    {
        _target = transform;
       

    }
    public void PositionCam()
    {
        Quaternion rotation = Quaternion.LookRotation(_target.forward, Vector3.up);
        var cameraPos = rotation * Offset;
        transform.position = Vector3.Lerp(transform.position, _target.position + cameraPos, 1);
        transform.LookAt(_target.position);
       // transform.parent = _target.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (_target == null) return;
        transform.position= new Vector3(_target.position.x,transform.position.y,_target.position.z-5);

        /*  Quaternion rotation = Quaternion.LookRotation(_target.forward, Vector3.up);
         var cameraPos = rotation * Offset;
          transform.position = Vector3.Lerp(transform.position, _target.position + cameraPos, Time.deltaTime * FollowSpeed);
          transform.LookAt(_target.position);*/
        // transform.position = Vector3.Lerp(transform.position, _target.position , Time.deltaTime * FollowSpeed);

    }
}
