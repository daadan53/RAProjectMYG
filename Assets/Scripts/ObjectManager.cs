using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public static event Action OnShowPanelRequested;
    public ObjectData objectData;

    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject model;
    [SerializeField] private TextMeshProUGUI dimension;
    [SerializeField] private TextMeshProUGUI price;

    private void Awake() 
    {
        
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
        AdjustColliderToMatchChild();
        //model.transform.rotation = Quaternion.identity;
    }

    private void AdjustColliderToMatchChild()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogError("Pas de BoxCollider trouvé.");
            return;
        }

        if (transform.childCount == 0)
        {
            Debug.LogError("Aucun enfant trouvé.");
            return;
        }

        Transform child = transform.GetChild(0);

        Renderer childRenderer = child.GetComponent<Renderer>();
        if (childRenderer == null)
        {
            childRenderer = child.GetComponentInChildren<Renderer>();
        }

        // Ajuste la taille et le centre du BoxCollider
        collider.size = childRenderer.bounds.size;
        collider.center = transform.InverseTransformPoint(childRenderer.bounds.center);
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
