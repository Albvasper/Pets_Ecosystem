using UnityEngine;
using TMPro;

public class RemotePet : MonoBehaviour
{
    public string petId;
    public string petName;
    public TypeOfPet typeOfPet;
    [SerializeField] TextMeshProUGUI nameTag;

    Vector3 targetPos;
    Quaternion targetRot;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
    }

    public void ApplySnapshot(Vector3 pos, Quaternion rot)
    {
        targetPos = pos;
        targetRot = rot;
    }

    public void SetName(string name)
    {
        petName = name;
        nameTag.text = name;
    }
}
