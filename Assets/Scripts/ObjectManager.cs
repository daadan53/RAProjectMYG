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
    [SerializeField] private GameObject model;
    [SerializeField] private TextMeshProUGUI dimension;
    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private RAScene raScene;

    private void Start()
    {
        LoadPrefab();
    }

    private void LoadPrefab()
    {
        model = Instantiate(objectData.ProductModel);
        model.transform.SetParent(transform);
        model.transform.localPosition = Vector3.zero;
        //model.transform.rotation = Quaternion.identity;
    }

    /*private void OnMouseDown()
    {
        raScene.ShowPanel();
        productName.text = objectData.ProductName;
        description.text = objectData.Description;
        dimension.text = objectData.Dimension;
        price.text = objectData.Price;
    }*/
}
