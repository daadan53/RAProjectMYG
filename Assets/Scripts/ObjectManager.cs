using System;
using TMPro;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static event Action<string, string, string, string> OnShowPanelRequested;
    public ObjectData objectData;

    [SerializeField] private GameObject model;

    private void Start()
    {
        LoadPrefab();
    }

    private void LoadPrefab()
    {
        model = Instantiate(objectData.ProductModel);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = new Vector3(1,1,1);
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
        OnShowPanelRequested?.Invoke(objectData.ProductName, objectData.Description, objectData.Dimension, objectData.Price);
    }
}
