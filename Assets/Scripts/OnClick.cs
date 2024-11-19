using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    public GameObject StoreManager;
    public GameObject onClickPopup;    
    public List<string> Items = new List<string>() {"Grocery_CocoCrunch", "Grocery_Danisa", "Grocery_iHop", "Grocery_JollyTime", "Grocery_Mamon", "Grocery_Wafer"};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateStore()
    {
        StoreManager.GetComponent<StoreManager>().DestroyRoom();
        int seed = Random.Range(0, 100);
        StoreManager.GetComponent<StoreManager>().BuildRandomStore(seed, 20, 20, 4.8f, 0.45f, 0.4f, 0.02f, 1, 4, 3, 0.5f);
    }

    public void FillShelves()
    {
        StoreManager.GetComponent<StoreManager>().DestroyAllProducts();
        int seed = Random.Range(0, 100);
        StoreManager.GetComponent<StoreManager>().FillShelves(seed, 270, Items);;
    }
}
