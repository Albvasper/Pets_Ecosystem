using System.Collections.Generic;

[System.Serializable]
public class WorldSnapshot
{
    public long serverTime;
    public List<PetSnapshot> pets;
    public float birthRate;
    public float populationHappiness;
    public float populationSentience;
    public bool isRaining;
    public bool isSnowing;
}