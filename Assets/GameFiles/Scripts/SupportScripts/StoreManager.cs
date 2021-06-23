using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    #region Properties
    public static StoreManager Instance = null;

    [Header("Attributes")]
    [SerializeField] internal List<int> characterPrices = new List<int>();
    [SerializeField] private Dictionary<int, bool> characterStoreData = new Dictionary<int, bool>() { { 0, true }, { 1, false }, { 2, false } };

    [Header("Components Reference")]
    [SerializeField] private TextMeshProUGUI buyingPriceTxt = null;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    #endregion

    #region Public Core Functions
    public bool IsPlayable(int characterIndex)
    {
        return characterStoreData[characterIndex];
    }

    public void UpdatePrice(int characterIndex, bool enablePriceTag = false)
    {
        buyingPriceTxt.gameObject.SetActive(enablePriceTag);
        buyingPriceTxt.SetText(characterPrices[characterIndex].ToString());
    }
    #endregion
}
