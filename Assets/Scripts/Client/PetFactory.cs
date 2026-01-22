using UnityEngine;
using System.Collections.Generic;

public class PetFactory : MonoBehaviour
{
    public static PetFactory Instance;

    [System.Serializable]
    public struct PetPrefabEntry
    {
        public TypeOfPet type;
        public GameObject prefab;
    }

    public PetPrefabEntry[] prefabs;

    Dictionary<TypeOfPet, GameObject> prefabMap;

    void Awake()
    {
        Instance = this;

        prefabMap = new Dictionary<TypeOfPet, GameObject>();
        foreach (var p in prefabs)
            prefabMap[p.type] = p.prefab;
    }

    public RemotePet SpawnPet(PetSnapshot p)
    {
        var go = Instantiate(
            prefabMap[p.petType],
            new Vector3(p.petX, p.petY, p.petZ),
            Quaternion.Euler(p.petRotationX, p.petRotationY, p.petRotationZ)
        );

        var pet = go.GetComponent<RemotePet>();
        pet.petId = p.petId;
        pet.typeOfPet = p.petType;
        pet.SetName(p.petName);
        return pet;
    }
}
