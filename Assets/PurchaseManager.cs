using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        bool isLoaded = LoadData();
        if (isLoaded)
        {

        }
        else
        {

        }
        PopulateStore();
        RefreshStore();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Corefunctions

    public void SelectItem(int id)
    {
       if(items.Find(x=> x.id == id).itemType == Item.Character)
        {
            PlayerPrefs.SetInt("character", id);
            items.Find(x => x.id == id).isSelected = true;
            foreach(StoreItem si in items)
            {
                if(si.itemType == Item.Character)
                {
                    if(si.id == id)
                    {
                        si.isSelected = true;
                    }
                    else
                    {
                        si.isSelected = false;
                    }
                }

                else if (si.itemType == Item.Sweet)
                {
                    if (si.id == id)
                    {
                        si.isSelected = true;
                    }
                    else
                    {
                        si.isSelected = false;
                    }
                }
            }
        }
        else if(items.Find(x => x.id == id).itemType == Item.Sweet)
        {
            PlayerPrefs.SetInt("sweet", id);
            foreach (StoreItem si in items)
            {
                if (si.itemType == Item.Character)
                {
                    if (si.id == id)
                    {
                        si.isSelected = true;
                    }
                    else
                    {
                        si.isSelected = false;
                    }
                }

                else if (si.itemType == Item.Sweet)
                {
                    if (si.id == id)
                    {
                        si.isSelected = true;
                    }
                    else
                    {
                        si.isSelected = false;
                    }
                }
            }
        }
        SaveData();

    }
    public void PurchaseItem(int id)
    {

        foreach(StoreItemButton sib in itemButtons)
        {
            if(id == sib.id && !items.Find(x => x.id == sib.id).isPurchased)
            {
                print(id);
                sib.availableBorder.SetActive(false);
                sib.purchasedBorder.SetActive(true);
                currentCoins -= sib.cost;
                sib.isPurchased = true;
                items.Find(x => x.id == sib.id).isPurchased = true;
                items.Find(x => x.id == sib.id).isSelected = true;
                PlayerPrefs.SetInt("coins", currentCoins);
                LevelUIManager.Instance.UpdateCoinCount(currentCoins);
                SelectItem(sib.id);
                RefreshStore();                
                return;
                
            }
            else if(id == sib.id && sib.isPurchased)
            {
                SelectItem(sib.id);
                return;
            }
        }
        
    }
    public void WatchAdCoins(int coins)
    {
        //currentCoins = PlayerPrefs.GetInt("coins", 0);
        currentCoins = currentCoins + coins;
        PlayerPrefs.SetInt("coins", currentCoins);
        LevelUIManager.Instance.UpdateCoinCount(currentCoins);
        RefreshStore();
    }

    public void PopulateStore()
    {        
        foreach(StoreItem si in items)
        {
            if(si.itemType == Item.Character)
            {
                GameObject g = Instantiate(storeButton, characterGrid);
                print(g.transform.parent);

                g.GetComponent<StoreItemButton>().id = si.id;
                g.GetComponent<StoreItemButton>().costText.text = si.cost +"";
                g.GetComponent<StoreItemButton>().cost = si.cost;
                g.GetComponent<Button>().onClick.AddListener(delegate { PurchaseItem(4);  });
                itemButtons.Add(g.GetComponent<StoreItemButton>());

                if (si.isPurchased == false)
                {
                    if (si.cost <= currentCoins)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                    }
                    else
                    {
                        g.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<StoreItemButton>().isPurchased = true;
                    g.GetComponent<Button>().interactable = true;

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

                if (si.isPurchased == false)
                {
                    if (si.cost <= currentCoins)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                    }
                    else
                    {
                        g.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<Button>().interactable = true;
                    g.GetComponent<StoreItemButton>().isPurchased = true;


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
                    //sib.availableBorder.SetActive(true);
                    sib.GetComponent<Button>().interactable = true;
                }
                else
                {
                    sib.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();        
        foreach (StoreItem targetGameObject in items)
        {
            save.items.Add(targetGameObject);
        }       
        return save;
    }
    public void SaveData()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved");
    }

    public bool LoadData()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            items.Clear();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();     
            for (int i = 0; i < save.items.Count; i++)
            {
                items.Add(save.items[i]);
            }
            return true;          
        }
        else
        {
            return false;
        }
    }
    #endregion
}
