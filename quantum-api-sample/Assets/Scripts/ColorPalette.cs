using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ActionRPG/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
  public Color UIHealth;
  public Color UIMana;
  public Color UIHealthPotion;
  public Color UIManaPotion;
  public Color UIWeakDamage;
}
