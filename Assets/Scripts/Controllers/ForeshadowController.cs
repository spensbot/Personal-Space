using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeshadowController : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemyEntryEffect;

    public float secondsToEnemy = 3;

    // Start is called before the first frame update
    void Start()
    {
        iTween.FadeFrom(this.gameObject, iTween.Hash("from", 0f, "time", secondsToEnemy));
        
    }

    void enterEnemy()
    {
        GameObject enemyInstance = Instantiate(enemy, this.gameObject.transform);
    }

    void OnDestroy()
    {
        
    }
}
