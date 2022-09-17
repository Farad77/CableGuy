using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Deterministic;
using Quantum;

public enum PopupTextType { Damage, HealthRecover, ManaRecover, GetHealthPotion, GetManaPotion }

public class UIFloatingTextsManager : MonoBehaviour
{
  public static UIFloatingTextsManager Instance { get; private set; }

  [SerializeField] private GameObject FloatingTextPrefab = null;
  [SerializeField] private int InitialPoolSize = 8;
  [SerializeField] private ColorPalette ColorPalette = null;

  private List<TextMeshPro> _floatingTextPool = new List<TextMeshPro>();

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    for (int i = 0; i < InitialPoolSize; i++)
    {
      AddToPool();
    }
  }

  private TextMeshPro AddToPool()
  {
    GameObject popupObject = Instantiate(FloatingTextPrefab, Vector3.zero, Quaternion.identity, this.transform);
    popupObject.SetActive(false);

    TextMeshPro popupText = popupObject.GetComponent<TextMeshPro>();
    _floatingTextPool.Add(popupText);

    return popupText;
  }

  private TextMeshPro GetFromPool()
  {
    for (int i = 0; i < _floatingTextPool.Count; i++)
    {
      if (_floatingTextPool[i].gameObject.activeSelf == false)
      {
        return _floatingTextPool[i];
      }
    }

    return AddToPool();
  }

  public void ShowText(PopupTextType textType, FP amount, EntityRef target)
  {
    var floatingText = GetFromPool();
    if (floatingText == null) return;

    // CHECK: Using the PredictedPrevious because on the last hit,
    // the target entity will not exist anymore.
    var frame = QuantumRunner.Default.Game.Frames.PredictedPrevious;

    if (frame.Exists(target) == false) return;
    if (frame.TryGet<Transform3D>(target, out var targetTransform) == false) return;

    floatingText.transform.position = targetTransform.Position.ToUnityVector3() + Vector3.one;
    switch (textType)
    {
      case PopupTextType.Damage:
        floatingText.text = "- " + amount;
        floatingText.color = ColorPalette.UIWeakDamage;
        break;
      case PopupTextType.HealthRecover:
        floatingText.text = "+ " + amount;
        floatingText.color = ColorPalette.UIHealth;
        break;
      case PopupTextType.ManaRecover:
        floatingText.text = "+ " + amount;
        floatingText.color = ColorPalette.UIMana;
        break;
      case PopupTextType.GetHealthPotion:
        floatingText.text = "+ " + amount + " Health Potion";
        floatingText.color = ColorPalette.UIHealthPotion;
        break;
      case PopupTextType.GetManaPotion:
        floatingText.text = "+ " + amount + " Mana Potion";
        floatingText.color = ColorPalette.UIManaPotion;
        break;
    }

    floatingText.gameObject.SetActive(true);
  }
}
