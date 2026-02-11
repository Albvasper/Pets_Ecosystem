using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class SnapshotBroadcaster : MonoBehaviour
{
    [SerializeField] float snapshotRate;

    float timer;

    void Update()
    {
        if (!ServerRole.IsServer) return;

        timer += Time.deltaTime;
        if (timer >= snapshotRate)
        {
            timer = 0f;
            BroadcastSnapshot();
        }
    }

    void BroadcastSnapshot()
    {
        Debug.Log("Sending Snapshot!");
        BaseAnimal[] animals = FindObjectsByType<BaseAnimal>(FindObjectsSortMode.None);

        WorldSnapshot snapshot = new WorldSnapshot
        {
            serverTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            pets = new List<PetSnapshot>(animals.Length),
            birthRate = Pet_Manager.Instance.BirthRate,
            populationHappiness = Pet_Manager.Instance.GetAverageHappiness(),
            populationSentience = Pet_Manager.Instance.GetAverageSentience(),
            isRaining = Weather_Manager.Instance.Raining,
            isSnowing = Weather_Manager.Instance.Snowing
        };

        foreach (var a in animals)
        {
            var t = a.transform;

            snapshot.pets.Add(new PetSnapshot
            {
                petId = a.petID,
                petType = a.TypeOfPet,
                petName = a.petName,

                petX = t.position.x,
                petY = t.position.y,
                petZ = t.position.z,

                isDead = a.isDead,
                isStunned = a.Animator.IsBeingBumped
            });
        }
        string json = JsonUtility.ToJson(snapshot);
        EcosystemWebSocketClient.Instance.Send(json);
    }
}
