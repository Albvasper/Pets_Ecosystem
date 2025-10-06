using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Scriptable Objects/AnimalData")]
public class AnimalData : ScriptableObject
{
    public int id;
    public string petName;
    public Sprite sprite;
    public float speed;
    public int rarity;
}
