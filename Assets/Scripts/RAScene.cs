using System.Collections.Generic;
using UnityEngine;

public class RAScene : MonoBehaviour
{
    GameManager gameManager;
    public ObjectManager objectManager;

    private GameObject visualRA;
    private const string WINE_BOTTLE = "Product1";
    [SerializeField] private int rotationDegree = 10;


    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject planeFinder;
    private const string PANEL_NAME = "PanelDescription";
    
    private void Awake() 
    {
        ObjectManager.OnShowPanelRequested += ShowPanel;

        gameManager = GameManager.instance;
        gameManager.groundStage = this.gameObject;
        objectManager = gameManager.gameObject.GetComponent<ObjectManager>();
        panel.SetActive(false);
    }
    private void Start() 
    {
        visualRA = gameManager.prefabModelList[gameManager.visualPrefabIndex];

        gameManager.MovePrefabTo(visualRA, this.gameObject.transform, false);

        
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        this.gameObject.SetActive(false);
        planeFinder.SetActive(false);
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
            visualRA.transform.Rotate(0, visualRA.transform.rotation.y - rotationDegree, 0);
        }
        else
        {
            visualRA.transform.Rotate(0, 0, visualRA.transform.rotation.z - rotationDegree);
        }
    }

    public void TurnLeft()
    {
        if(visualRA.name != WINE_BOTTLE)
        {
            visualRA.transform.Rotate(0, visualRA.transform.rotation.y + rotationDegree, 0);
        }
        else
        {
            visualRA.transform.Rotate(0, 0, visualRA.transform.rotation.z + rotationDegree);
        }
    }

    private void OnDestroy() 
    {
        ObjectManager.OnShowPanelRequested -= ShowPanel;
    }
}
