using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RAScene : MonoBehaviour
{
    GameManager gameManager;

    private GameObject visualRA;
    private const string WINE_BOTTLE = "Product1";
    [SerializeField] private int rotationDegree = 10;


    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject planeFinder;
    private const string PANEL_NAME = "PanelDescription";
    [SerializeField] private TextMeshProUGUI productNameChamp;
    [SerializeField] private TextMeshProUGUI descriptionChamp;
    [SerializeField] private TextMeshProUGUI dimensionChamp;
    [SerializeField] private TextMeshProUGUI priceChamp;

    private string productName;
    private string productDescription;
    private string productDimension;
    private int productPrice;
    
    private void Awake() 
    {
        RetrieveTable.OnProductsRetrieved += SetProductDetails;
        ObjectManager.OnShowPanelRequested += ShowPanel;

        gameManager = GameManager.instance;
        gameManager.groundStage = this.gameObject;
        panel.SetActive(false);
    }
    private void Start() 
    {
        visualRA = gameManager.prefabModelList[gameManager.visualPrefabIndex];

        gameManager.MovePrefabTo(visualRA, this.gameObject.transform, false);
    }

    //Recup les détails du produit actuellement séléctionné
    public void SetProductDetails(Product[] _products)
    {
        foreach(var product in _products)
        {
            if(product.id - 1 == gameManager.visualPrefabIndex)
            {
                Debug.Log($"Product : {product.name}");
                productName = product.name;
                productDescription = product.description;
                productDimension = product.dimension;
                productPrice = product.price;
            }
        }
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        this.gameObject.SetActive(false);
        planeFinder.SetActive(false);
        productNameChamp.text = productName;
        descriptionChamp.text = productDescription;
        dimensionChamp.text = productDimension;
        priceChamp.text = productPrice.ToString() + "€";
    }

    public void OnGoNextScene() => gameManager.OnGoNextScene("CatalogueScene", visualRA, gameManager.prefabModelList);


    public void GoBack()
    {
        panel.SetActive(false);
        this.gameObject.SetActive(true);
        planeFinder.SetActive(true);
    }
    
    public void TurnRight()
    {
        visualRA.transform.Rotate(0, visualRA.transform.rotation.y - rotationDegree, 0);
    }

    public void TurnLeft()
    {
        visualRA.transform.Rotate(0, visualRA.transform.rotation.y + rotationDegree, 0);
    }

    private void OnDestroy() 
    {
        RetrieveTable.OnProductsRetrieved -= SetProductDetails;
        ObjectManager.OnShowPanelRequested -= ShowPanel;
    }
}
