[System.Serializable]
public struct PetSnapshot
{
    public string petId;
    public TypeOfPet petType;
    public string petName;
    public float petX, petY, petZ;
    public bool isDead;
    public bool isStunned;
    //public bool isAttacking;
    //public float petRotationX, petRotationY, petRotationZ;
}