using UnityEngine;

public class RAScene : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    private GameObject visualRA;
    private const string wineBottle = "WineBottle";
    [SerializeField] private int rotationDegree = 10;

    private void Awake() 
    {
        GameManager.instance.groundStage = this.gameObject;
    }

    private void Start() 
    {
        visualRA = prefabs[GameManager.instance.visualPrefabIndex];
        visualRA.transform.SetParent(this.gameObject.transform);
        visualRA.transform.localPosition = Vector3.zero;
        visualRA.SetActive(false);
    }

    public void GoBack() => GameManager.instance.OnGoBack("CatalogueScene");
    
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
