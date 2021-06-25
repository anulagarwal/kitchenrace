using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance = null;

    public List<StoreItem> items;
    public List<StoreItemButton> itemButtons;
    public int currentCoins;

    [Header("UI Items")]
    [SerializeField] Text buttonCost;
    [SerializeField] Button purchaseButton;
    [SerializeField] Transform cookieGrid;
    [SerializeField] Transform characterGrid;
    [SerializeField] GameObject storeButton;




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

    // Start is called before the first frame update
    void Start()
    {
        currentCoins = PlayerPrefs.GetInt("coins", 0);
       // PopulateStore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Corefunctions

    public void SelectItem(int id)
    {
        buttonCost.text = items.Find(x => x.id == id).cost + "";
        if(items.Find(x => x.id == id).cost > currentCoins)
        {
            purchaseButton.interactable = false;
        }
    }
    public void PurchaseItem(int id)
    {

        foreach(StoreItemButton sib in itemButtons)
        {
            if(id == sib.id)
            {
                sib.availableBorder.SetActive(false);
                sib.purchasedBorder.SetActive(true);
                currentCoins -= sib.cost;
                sib.isPurchased = true;
                PlayerPrefs.SetInt("coins", currentCoins);
                //Purchase here save data
                //Set as new prefab
            }
        }
        LevelUIManager.Instance.UpdateCoinCount(currentCoins);
        RefreshStore();
    }
    public void WatchAdCoins(int coins)
    {
        //currentCoins = PlayerPrefs.GetInt("coins", 0);
        currentCoins = currentCoins + coins;
        PlayerPrefs.SetInt("coins", currentCoins);
        LevelUIManager.Instance.UpdateCoinCount(currentCoins);
    }

    public void PopulateStore()
    {
        foreach(StoreItem si in items)
        {
            if(si.itemType == Item.Character)
            {
                GameObject g = Instantiate(storeButton, characterGrid);
                g.GetComponent<StoreItemButton>().id = si.id;
                g.GetComponent<StoreItemButton>().costText.text = si.cost +"";
                g.GetComponent<StoreItemButton>().cost = si.cost;
                g.GetComponent<Button>().onClick.AddListener(delegate { PurchaseItem(si.id);  });
                itemButtons.Add(g.GetComponent<StoreItemButton>());

                if (si.cost <= currentCoins)
                {
                    g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                }
                else
                {
                    g.GetComponent<Button>().interactable = false;
                }
            }

            else if(si.itemType == Item.Sweet)
            {
                GameObject g = Instantiate(storeButton, cookieGrid);
                g.GetComponent<StoreItemButton>().id = si.id;
                g.GetComponent<StoreItemButton>().costText.text = si.cost + "";
                g.GetComponent<StoreItemButton>().cost = si.cost;
                g.GetComponent<Button>().onClick.AddListener(delegate { PurchaseItem(si.id); });
                itemButtons.Add(g.GetComponent<StoreItemButton>());
                if (si.cost <= currentCoins)
                {
                    g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                }
                else
                {
                    g.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void RefreshStore()
    {
        foreach(StoreItemButton sib in itemButtons)
        {
            if (!sib.isPurchased)
            {
                if (sib.cost <= currentCoins)
                {
                    sib.availableBorder.SetActive(true);
                }
                else
                {
                    sib.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    #endregion
}
