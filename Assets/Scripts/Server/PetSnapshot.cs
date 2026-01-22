[System.Serializable]
public struct PetSnapshot
{
    public string petId;
    public TypeOfPet petType;
    public string petName;

    public float petX, petY, petZ;
    public float petRotationX, petRotationY, petRotationZ;

    //public int state; // animation / behavior enum
}