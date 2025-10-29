using UnityEngine;
using System.Collections;


/// <summary>
/// Manages cave object functionalities for hiding pets and changing sprite
/// dependig on weather.
/// </summary>
public class Cave : MonoBehaviour
{
    const float InsideCaveCooldown = 15f;
    [SerializeField] Transform SpawnPoint;
    /// <summary>
    /// Current sprite that is beign displayed.
    /// </summary>
    [SerializeField] SpriteRenderer currentSprite;
    /// <summary>
    /// Cave sprite to display when snowing.
    /// </summary>
    [SerializeField] Sprite snowyCave;
    /// <summary>
    /// Standard cave sprite.
    /// </summary>
    [SerializeField] Sprite normalCave;
    /// <summary>
    /// Time that pets will be inside the cave.
    /// </summary>

    void Awake()
    {
        currentSprite.sprite = normalCave;
    }

    void Start()
    {
        Weather_Manager.Instance.OnWeatherChanged += UpdateSprite;
    }
    
    public void EnterCave(BaseAnimal pet)
    {
        pet.gameObject.SetActive(false);
        StartCoroutine(ExitCave(pet));
    }

    void UpdateSprite()
    {
        if (Weather_Manager.Instance.Snowing)
            currentSprite.sprite = snowyCave;
        else
            currentSprite.sprite = normalCave;
    }

    IEnumerator ExitCave(BaseAnimal pet)
    {
        yield return new WaitForSeconds(InsideCaveCooldown);
        pet.transform.position = SpawnPoint.position;
        pet.gameObject.SetActive(true);
    }
}
