using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance = null;

    public List<StoreItem> sweetItems;
    public List<StoreItem> characterItems;

    public List<StoreItemButton> itemButtons;
    public List<IconItemID> icons;
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
        //File.Delete(Application.persistentDataPath + "/gamesave.save");
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
        if (sweetItems.Find(x => x.id == id) != null)
        {
            PlayerPrefs.SetInt("sweet", id);

            foreach (StoreItem si in sweetItems)
            { 
                if (si.itemType == Item.Sweet)
                {
                    if (si.id == id)
                    {
                        si.isSelected = true;
                        SweetsManager.Instance.SetSweetType(sweetItems.FindIndex(x => x.id == si.id));                    
                        itemButtons.Find(x => x.id == si.id).selectedBorder.SetActive(true);
                    //    itemButtons.Find(x => x.id == si.id).availableBorder.SetActive(false);
                   //     itemButtons.Find(x => x.id == si.id).purchasedBorder.SetActive(false);
                    }
                    else
                    {
                        si.isSelected = false;
                        itemButtons.Find(x => x.id == si.id).selectedBorder.SetActive(false);
                   //     itemButtons.Find(x => x.id == si.id).availableBorder.SetActive(false);
                   //     itemButtons.Find(x => x.id == si.id).purchasedBorder.SetActive(false);

                    }
                }
            }
        }
        else if(characterItems.Find(x => x.id == id) != null)
        {
            PlayerPrefs.SetInt("character", id);

            foreach (StoreItem si in characterItems)
            {
                if (si.itemType == Item.Character)
                {
                    if (si.id == id)
                    {
                        si.isSelected = true;
                        
                        itemButtons.Find(x => x.id == si.id).selectedBorder.SetActive(true);
                  //      itemButtons.Find(x => x.id == si.id).availableBorder.SetActive(false);
                  //      itemButtons.Find(x => x.id == si.id).purchasedBorder.SetActive(false);


                        PlayerCharacterManager.Instance.EnablePlayerCharacter(characterItems.FindIndex(x=> x.id == id));

                    }
                    else
                    {
                        si.isSelected = false;
                        itemButtons.Find(x => x.id == si.id).selectedBorder.SetActive(false);
                //        itemButtons.Find(x => x.id == si.id).availableBorder.SetActive(false);
                //        itemButtons.Find(x => x.id == si.id).purchasedBorder.SetActive(false);

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
            if(id == sib.id && sib.isPurchased)
            {
                SelectItem(sib.id);
                return;
            }
            else if(sib.id==id)
            {                
                
                if (sweetItems.Find(x => x.id == id) != null)
                {
                    sib.availableBorder.SetActive(false);
                    sib.purchasedBorder.SetActive(true);
                    currentCoins -= sib.cost;
                    sib.isPurchased = true;

                    sweetItems.Find(x => x.id == id).isPurchased = true;
                    sweetItems.Find(x => x.id == id).isSelected = true;
                    PlayerPrefs.SetInt("coins", currentCoins);
                    LevelUIManager.Instance.UpdateCoinCount(currentCoins);
                    SelectItem(sib.id);
                    RefreshStore();
                    return;
                }
                else if(characterItems.Find(x => x.id == id) != null)
                
                {
                    sib.availableBorder.SetActive(false);
                    sib.purchasedBorder.SetActive(true);
                    currentCoins -= sib.cost;
                    sib.isPurchased = true;
                    characterItems.Find(x => x.id == id).isPurchased = true;
                    characterItems.Find(x => x.id == id).isSelected = true;
                    PlayerPrefs.SetInt("coins", currentCoins);
                    LevelUIManager.Instance.UpdateCoinCount(currentCoins);
                    SelectItem(sib.id);
                    RefreshStore();
                    return;
                }
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
        
        foreach (StoreItem si in sweetItems)
        {
             if (si.itemType == Item.Sweet)
            {
                GameObject g = Instantiate(storeButton, cookieGrid);
                g.GetComponent<StoreItemButton>().id = si.id;
                g.GetComponent<StoreItemButton>().costText.text = si.cost + "";
                g.GetComponent<StoreItemButton>().cost = si.cost;
                g.GetComponent<StoreItemButton>().name = si.itemName;
                g.GetComponent<Image>().sprite = icons.Find(x => x.id == si.id).icon;
                itemButtons.Add(g.GetComponent<StoreItemButton>());

                if (si.isPurchased == false)
                {
                    if (si.cost <= currentCoins && currentCoins - si.cost > 0)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                        g.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                        g.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<Button>().interactable = true;
                    g.GetComponent<StoreItemButton>().isPurchased = true;
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                    g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                    if (si.isSelected)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(true);
                        SweetsManager.Instance.SetSweetType(sweetItems.FindIndex(x => x.id == si.id));
                    }

                }
            }
        }
        foreach (StoreItem si in characterItems)
        {
            if (si.itemType == Item.Character)
            {
                GameObject g = Instantiate(storeButton, characterGrid);

                g.GetComponent<StoreItemButton>().id = si.id;
                g.GetComponent<StoreItemButton>().costText.text = si.cost + "";
                g.GetComponent<StoreItemButton>().cost = si.cost;
                g.GetComponent<StoreItemButton>().name = si.itemName;
                g.GetComponent<Image>().sprite = icons.Find(x => x.id == si.id).icon;

                itemButtons.Add(g.GetComponent<StoreItemButton>());
                if (si.isPurchased == false)
                {
                    if (si.cost <= currentCoins && currentCoins - si.cost > 0)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(true);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                        g.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                        g.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<StoreItemButton>().isPurchased = true;
                    g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                    g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                    g.GetComponent<StoreItemButton>().selectedBorder.SetActive(false);

                    g.GetComponent<Button>().interactable = true;
                    if (si.isSelected)
                    {
                        g.GetComponent<StoreItemButton>().availableBorder.SetActive(false);
                        g.GetComponent<StoreItemButton>().purchasedBorder.SetActive(true);
                        g.GetComponent<StoreItemButton>().selectedBorder.SetActive(true);
                        PlayerCharacterManager.Instance.EnablePlayerCharacter(characterItems.FindIndex(x => x.id == si.id));
                    }
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
                if (sib.cost <= currentCoins && currentCoins - sib.cost > 0)
                {
                    //sib.availableBorder.SetActive(true);
                    sib.GetComponent<Button>().interactable = true;
                    sib.availableBorder.SetActive(true);
                    sib.purchasedBorder.SetActive(false);
                    sib.selectedBorder.SetActive(false);
                }
                else
                {
                    sib.GetComponent<Button>().interactable = false;
                    sib.availableBorder.SetActive(false);
                    sib.purchasedBorder.SetActive(false);
                    sib.selectedBorder.SetActive(false);
                }
            }
         
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();        
       
        foreach (StoreItem targetGameObject in sweetItems)
        {
            save.sweetItems.Add(targetGameObject);
        }
        foreach (StoreItem targetGameObject in characterItems)
        {
            save.characterItems.Add(targetGameObject);
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
        //File.Delete(Application.persistentDataPath + "/gamesave.save");
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
           
            sweetItems.Clear();
            characterItems.Clear();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();     
          
            for (int i = 0; i < save.sweetItems.Count; i++)
            {
                sweetItems.Add(save.sweetItems[i]);
            }
            for (int i = 0; i < save.characterItems.Count; i++)
            {
                characterItems.Add(save.characterItems[i]);
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
