using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardText : MonoBehaviour
{
  private Transform _cameraTransform;
  private Quaternion _originalRotation;

  // Start is called before the first frame update
  void Start()
  {
    _cameraTransform = Camera.main.transform;
    _originalRotation = transform.rotation;
  }

  // Update is called once per frame
  void Update()
  {
    transform.rotation = _cameraTransform.rotation * _originalRotation;
  }
}
