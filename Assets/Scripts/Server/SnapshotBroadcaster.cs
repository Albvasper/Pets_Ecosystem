using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class SnapshotBroadcaster : MonoBehaviour
{
    [SerializeField] float snapshotRate = 0.1f; // ~6–7 Hz

    float timer;

    void Update()
    {
#if UNITY_SERVER || !UNITY_WEBGL
        timer += Time.deltaTime;
        if (timer >= snapshotRate)
        {
            timer = 0f;
            BroadcastSnapshot();
        }
#endif
    }

    void BroadcastSnapshot()
    {
        BaseAnimal[] animals = FindObjectsByType<BaseAnimal>(FindObjectsSortMode.None);

        WorldSnapshot snapshot = new WorldSnapshot
        {
            serverTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            pets = new List<PetSnapshot>(animals.Length)
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
                //isAttacking = a.Animator.isAtta
            });
        }

        string json = JsonUtility.ToJson(snapshot);
        EcosystemWebSocketClient.Instance.Send(json);
        Debug.Log("SERVER SENT SNAPSHOT");
    }
}
