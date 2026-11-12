using TMPro;
using UnityEngine;

public class GunAmmoCount : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoCount;
    [SerializeField] private TMP_Text _effectsList;
    [SerializeField] private Gun _gun;


    private void UpdateAmmoCount()
    {
        string newText = $"{_gun.CurrentAmmo.ToString()} / {_gun.MaxAmmo}";

        _ammoCount.text = newText;
    }
    private void UpdateEffectsList()
    {
        string newText = "(Debug) Active Effects: \n";

        newText += $"BiggerBullets: {ActiveEffects.BiggerBullets}\n";
        newText += $"SlowBullets: {ActiveEffects.SlowBullets}\n";
        newText += $"ExplosiveBullets: {ActiveEffects.ExplosiveBullets}\n";
        newText += $"MultipleBullets: {ActiveEffects.MultipleBullets}\n";
        newText += $"RicochetableBullets: {ActiveEffects.RicochetableBullets}\n";

        _effectsList.text = newText;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gun.OnAmmoUpdate += UpdateAmmoCount;
        _gun.OnAmmoUpdate += UpdateEffectsList;
    }

    private void UnSubscribeToEvents()
    {
        _gun.OnAmmoUpdate -= UpdateAmmoCount;
        _gun.OnAmmoUpdate -= UpdateEffectsList;
    }
}
