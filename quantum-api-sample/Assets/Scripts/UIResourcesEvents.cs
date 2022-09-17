using UnityEngine;
using Quantum;
using Photon.Deterministic;

public class UIResourcesEvents : MonoBehaviour
{
  // Start is called before the first frame update
  void OnEnable()
  {
    QuantumEvent.Subscribe<EventOnDamageDealt>(this, OnDamageDealt);
    QuantumEvent.Subscribe<EventOnPickUpHealthPotion>(this, OnPickUpHealthPotion);
    QuantumEvent.Subscribe<EventOnUsedHealthPotion>(this, OnUsedHealthPotion);
    QuantumEvent.Subscribe<EventOnPickUpManaPotion>(this, OnPickUpManaPotion);
    QuantumEvent.Subscribe<EventOnUsedManaPotion>(this, OnUsedManaPotion);
  }

  private void OnDamageDealt(EventOnDamageDealt e)
  {
    UIFloatingTextsManager.Instance.ShowText(PopupTextType.Damage, e.Amount, e.Target);
  }

  private void OnPickUpHealthPotion(EventOnPickUpHealthPotion e)
  {
    UIFloatingTextsManager.Instance.ShowText(PopupTextType.GetHealthPotion, e.Amount, e.Target);
  }

  private void OnUsedHealthPotion(EventOnUsedHealthPotion e)
  {
    UIFloatingTextsManager.Instance.ShowText(PopupTextType.HealthRecover, e.Amount, e.Target);
  }

  private void OnPickUpManaPotion(EventOnPickUpManaPotion e)
  {
    UIFloatingTextsManager.Instance.ShowText(PopupTextType.GetManaPotion, e.Amount, e.Target);

  }

  private void OnUsedManaPotion(EventOnUsedManaPotion e)
  {
    UIFloatingTextsManager.Instance.ShowText(PopupTextType.ManaRecover, e.Amount, e.Target);
  }
}
