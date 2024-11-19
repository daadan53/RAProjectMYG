using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public string productName;
    public string description;
    //public GameObject productModel;
    public string dimension;
    public string price;
}
