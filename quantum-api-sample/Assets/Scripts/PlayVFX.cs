using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] private GameObject _vfxPrefab = null;
    [SerializeField] private float _vfxLifetime = 1.0f;
    private void Play()
    {
        var temp = Instantiate(_vfxPrefab, transform.position, Quaternion.identity);
        Destroy(temp, _vfxLifetime);
    }

    public void PlayAndDestroy()
    {
        Play();
        Destroy(gameObject);
    }
}
