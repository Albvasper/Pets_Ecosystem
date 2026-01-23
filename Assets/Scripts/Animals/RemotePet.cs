using UnityEngine;
using TMPro;

public class RemotePet : MonoBehaviour
{
    public string petId;
    public string petName;
    public TypeOfPet typeOfPet;
    [SerializeField] TextMeshProUGUI nameTag;

    Vector3 targetPos;
    Vector3 lastPos;
    bool isDead;
    bool isStunned;
    bool isAttacking;

    bool initialized;
    RemotePetAnimator petAnimator;

    void Awake()
    {
        petAnimator = GetComponent<RemotePetAnimator>();
    }

    void Start()
    {
        ClientEcosystemUiManager.AddToPetCount();
        switch (typeOfPet)
        {
            case TypeOfPet.Dog: ClientEcosystemUiManager.AddToDogCount(); break;
            case TypeOfPet.Cat: ClientEcosystemUiManager.AddToCatCount(); break;
            case TypeOfPet.Deer: ClientEcosystemUiManager.AddToDeerCount(); break;
            case TypeOfPet.Wolf: ClientEcosystemUiManager.AddToWolfCount(); break;
            case TypeOfPet.Tiger: ClientEcosystemUiManager.AddToTigerCount(); break;
            case TypeOfPet.Bear: ClientEcosystemUiManager.AddToBearCount(); break;
            //default: Pet_Manager.Instance.AddToPokemonPopulation(); break;
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 10f * Time.deltaTime);
        Vector3 delta = transform.position - lastPos;
        Vector2 velocity = new Vector2(delta.x, delta.y) / Time.deltaTime;

        petAnimator.UpdateMovementAnimation(velocity);
        petAnimator.UpdateStateAnimation(isDead, isStunned, isAttacking);
        lastPos = transform.position;
    }

    void OnDestroy()
    {
        ClientEcosystemUiManager.RemoveFromPetCount();
    }

    public void ApplySnapshot(Vector3 pos)
    {
        targetPos = pos;
        if (!initialized)
        {
            transform.position = pos;
            lastPos = pos;
            initialized = true;
        }
    }

    public void ApplyState(bool isDead, bool isStunned)
    {
        this.isDead = isDead;
        this.isStunned = isStunned;
        //this.isAttacking = isAttacking;
    }

    public void SetName(string name)
    {
        petName = name;
        nameTag.text = name;
    }
}
