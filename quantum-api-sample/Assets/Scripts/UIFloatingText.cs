using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloatingText : MonoBehaviour
{
  public float TTL;

  private float _timer;

  private void OnEnable()
  {
    _timer = TTL;
  }

  private void Update()
  {
    _timer -= Time.deltaTime;

    if (_timer <= 0) this.gameObject.SetActive(false);
  }
}
