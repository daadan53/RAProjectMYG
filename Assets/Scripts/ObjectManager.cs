using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public UnityEvent OnShowPanelRequested;
    public ObjectData objectData;

    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject model;
    [SerializeField] private TextMeshProUGUI dimension;
    [SerializeField] private TextMeshProUGUI price;

    private void Awake() 
    {
        OnShowPanelRequested = new UnityEvent();
    }

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

    private void OnMouseDown()
    {
        OnShowPanelRequested?.Invoke();
        productName.text = objectData.ProductName;
        description.text = objectData.Description;
        dimension.text = objectData.Dimension;
        price.text = objectData.Price;
    }
}
