using UnityEngine;
using System.Collections.Generic;

public static class SnapshotReceiver
{
    static Dictionary<string, RemotePet> pets = new();

    public static void Apply(WorldSnapshot snapshot)
    {
        HashSet<string> aliveIds = new();

        foreach (var p in snapshot.pets)
        {
            aliveIds.Add(p.petId);

            // Spawn pets
            if (!pets.TryGetValue(p.petId, out var animal))
            {
                Debug.Log("SPAWN PET " + p.petId);
                animal = PetFactory.Instance.SpawnPet(p);
                pets[p.petId] = animal;
            }

            animal.ApplySnapshot(new Vector3(p.petX, p.petY, p.petZ));
        }
        // Despawn pets
        var petsToDespawn = new List<string>();
        foreach (var id in pets.Keys)
        {
            if (!aliveIds.Contains(id))
                petsToDespawn.Add(id);
        }
        foreach (var id in petsToDespawn)
        {
            GameObject.Destroy(pets[id].gameObject);
            pets.Remove(id);
        }
    }
}
