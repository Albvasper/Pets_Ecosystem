using System.Collections.Generic;

[System.Serializable]
public class WorldSnapshot
{
    public long serverTime;
    public List<PetSnapshot> pets;
}