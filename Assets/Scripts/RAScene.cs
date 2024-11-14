using UnityEngine;

public class RAScene : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    private GameObject visualRA;

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
    
    /*public void OnTargetFound()
    { 
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }*/
}
