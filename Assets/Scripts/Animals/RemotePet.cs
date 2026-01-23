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
    Quaternion targetRot;
    bool initialized;
    RemotePetAnimator petAnimator;

    void Awake()
    {
        petAnimator = GetComponent<RemotePetAnimator>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        Vector3 delta = transform.position - lastPos;
        Vector2 velocity = new Vector2(delta.x, delta.y) / Time.deltaTime;

        petAnimator.UpdateMovementAnimation(velocity);
        lastPos = transform.position;
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

    public void SetName(string name)
    {
        petName = name;
        nameTag.text = name;
    }
}
