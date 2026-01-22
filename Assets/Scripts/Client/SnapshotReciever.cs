using UnityEngine;
using System.Collections.Generic;

public static class SnapshotReceiver
{
    static Dictionary<string, RemotePet> pets = new();

    public static void Apply(WorldSnapshot snapshot)
    {
        foreach (var p in snapshot.pets)
        {
            if (!pets.TryGetValue(p.petId, out var animal))
            {
                Debug.Log("SPAWN PET " + p.petId);

                animal = PetFactory.Instance.SpawnPet(p); // your existing spawn logic
                pets[p.petId] = animal;
            }

            animal.ApplySnapshot(
            new Vector3(p.petX, p.petY, p.petZ),
            Quaternion.Euler(p.petRotationX, p.petRotationY, p.petRotationZ)
            );
        }
    }
}
