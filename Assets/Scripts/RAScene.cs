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
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI dimension;
    [SerializeField] private TextMeshProUGUI price;
    
    private void Awake() 
    {
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

    public void ShowPanel(string _name, string _description, string _dimension, string _price)
    {
        panel.SetActive(true);
        this.gameObject.SetActive(false);
        planeFinder.SetActive(false);
        productName.text = _name;
        description.text = _description;
        dimension.text = _dimension;
        price.text = _price;
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
        if(visualRA.name != WINE_BOTTLE)
        {
            visualRA.transform.Rotate(visualRA.transform.rotation.x - rotationDegree, 0, 0);
        }
        else
        {
            visualRA.transform.Rotate(0, visualRA.transform.rotation.y - rotationDegree, 0);
        }
    }

    public void TurnLeft()
    {
        if(visualRA.name != WINE_BOTTLE)
        {
            visualRA.transform.Rotate(visualRA.transform.rotation.x + rotationDegree, 0, 0);
        }
        else
        {
            visualRA.transform.Rotate(0, visualRA.transform.rotation.y + rotationDegree, 0);
        }
    }

    private void OnDestroy() 
    {
        ObjectManager.OnShowPanelRequested -= ShowPanel;
    }
}
