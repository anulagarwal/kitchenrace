
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StoreItemButton : MonoBehaviour,IPointerClickHandler
{
    public GameObject availableBorder;
    public GameObject purchasedBorder;
    public GameObject selectedBorder;

    public Text costText;
    public Text names;

    public Image img;
    public int id;
    public int cost;
    public bool isPurchased;

    private void Start()
    {
        costText.text = "" + cost;
    }
   

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GetComponent<Button>().interactable)
        PurchaseManager.Instance.PurchaseItem(id);
      
//        throw new System.NotImplementedException();
    }
}
