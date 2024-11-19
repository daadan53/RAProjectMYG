using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public ObjectData objectData;

    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI dimension;
    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private RAScene raScene;

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        // Appelé lorsqu'on clique sur l'objet
        Debug.Log($"Prefab {gameObject.name} cliqué !");
        raScene.ShowPanel();
        productName.text = objectData.productName;
        description.text = objectData.description;
        dimension.text = objectData.dimension;
        price.text = objectData.price;
    }
}
