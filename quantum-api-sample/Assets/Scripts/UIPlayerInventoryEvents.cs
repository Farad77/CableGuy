using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using TMPro;
using UnityEngine;

public class UIPlayerInventoryEvents : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthPotionsCounter = null;
    [SerializeField] private TextMeshProUGUI _energyCounter = null;
    [SerializeField] private TextMeshProUGUI _manaPotionsCounter = null;
    [SerializeField] private TextMeshProUGUI _compteur = null;

    // [SerializeField] private TextMeshProUGUI _coinsCounter = null;

    private EntityRef _player = default;
    
    // Start is called before the first frame update
    void Start()
    {
        _healthPotionsCounter.text = "";
        _manaPotionsCounter.text = "";
        _compteur.text = "";
        // _coinsCounter.text = "";

        QuantumEvent.Subscribe<EventOnPickUpHealthPotion>(this, OnHealthPotionPickUp);
        QuantumEvent.Subscribe<EventOnPickUpManaPotion>(this, OnManaPotionPickUp);
             QuantumEvent.Subscribe<EventOnRegenTick>(this, OnRegenTick);
        StartCoroutine(compte());
        // EventOnPickUpCoins.OnRaised += OnCoinsPickUp;
    }
    IEnumerator compte()
    {
        yield return new WaitForSeconds(1f);
        _compteur.text = UISpawnEnemy.compteur+"";
        StartCoroutine(compte());
    }
    public unsafe void Initialize(EntityRef player)
    {
        _player = player;

        var inventory = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<CharacterInventory>(player);
        
        _healthPotionsCounter.text = inventory->PotionsHealth.AsInt.ToString();
        _manaPotionsCounter.text = inventory->PotionsMana.AsInt.ToString();
        // _coinsCounter.text = inventory->Wallet.AsInt.ToString();
    }
    private void OnRegenTick(EventOnRegenTick e)
    {
        if (e.Target != _player) return;
        _energyCounter.text = "Energie:"+e.Amount.ToString();

    }
        private void OnHealthPotionPickUp(EventOnPickUpHealthPotion e)
    {
        if (e.Target != _player) return;
        _healthPotionsCounter.text = e.Amount.ToString();
    }
    
    private void OnManaPotionPickUp(EventOnPickUpManaPotion e)
    {
        if (e.Target != _player) return;
        _manaPotionsCounter.text = e.Amount.ToString();
    }

    // private void OnCoinsPickUp(EventOnPickUpCoins e)
    // {
    //     if (e.Target != _player) return;
    //     _coinsCounter.text = e.Amount.ToString();
    // }
}
