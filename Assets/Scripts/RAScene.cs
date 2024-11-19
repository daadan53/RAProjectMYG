using UnityEngine;

public class RAScene : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    private GameObject visualRA;
    private const string wineBottle = "WineBottle";
    [SerializeField] private int rotationDegree = 10;

    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject planeFinder;
    private int index;
    private const string panelName = "PanelDescription";
    
    private void Awake() 
    {
        GameManager.instance.groundStage = this.gameObject;
    }

    private void Start() 
    {
        arCamera = Camera.main;

        visualRA = prefabs[GameManager.instance.visualPrefabIndex];
        visualRA.transform.SetParent(this.gameObject.transform);
        visualRA.transform.localPosition = Vector3.zero;
        visualRA.SetActive(false);

        panel.SetActive(false);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        this.gameObject.SetActive(false);
        planeFinder.SetActive(false);
        Debug.Log("Action déclenchée pour cet objet !");
    }

    public void GoBack() => GameManager.instance.OnGoNextScene("CatalogueScene");

    public void Return() => GameManager.instance.Return(panel, this.gameObject, planeFinder);
    
    public void TurnRight()
    {
        if(visualRA.name != wineBottle)
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
        if(visualRA.name != wineBottle)
        {
            visualRA.transform.Rotate(0, visualRA.transform.rotation.y + rotationDegree, 0);
        }
        else
        {
            visualRA.transform.Rotate(0, 0, visualRA.transform.rotation.z + rotationDegree);
        }
    }
}
