using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    [SerializeField] private string productName;
    public string ProductName => productName; // Ajoute une couche de protection, càd, qu'on peut juste get et non set
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private GameObject productModel;
    public GameObject ProductModel => productModel;
    [SerializeField] private string dimension;
    public string Dimension => dimension;
    [SerializeField] private string price;
    public string Price => price;

}
