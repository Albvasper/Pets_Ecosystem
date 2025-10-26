using UnityEngine;
using System.Collections;

public class Cave : MonoBehaviour
{
    [SerializeField] Transform SpawnPoint;
    [SerializeField] SpriteRenderer currentSprite;
    [SerializeField] Sprite snowyCave;
    [SerializeField] Sprite normalCave;
    float insideCaveCooldown = 15f;

    void Awake()
    {
        currentSprite.sprite = normalCave;
    }

    void Start()
    {
        Weather_Manager.Instance.OnWeatherChanged += UpdateSprite;
    }

    void UpdateSprite()
    {
        if (Weather_Manager.Instance.Snowing)
            currentSprite.sprite = snowyCave;
        else
            currentSprite.sprite = normalCave;
    }
    
    public void EnterCave(BaseAnimal pet)
    {
        pet.gameObject.SetActive(false);
        StartCoroutine(ExitCave(pet));
    }

    IEnumerator ExitCave(BaseAnimal pet)
    {
        yield return new WaitForSeconds(insideCaveCooldown);
        pet.transform.position = SpawnPoint.position;
        pet.gameObject.SetActive(true);
    }
}
