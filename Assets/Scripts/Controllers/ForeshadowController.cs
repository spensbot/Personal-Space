using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeshadowController : Trackable
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemyEntryEffect;
    public float secondsToEnemy;
    public List<GameObject> enemies;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Vector3 destination = this.gameObject.transform.position;
        adjustTransform();
        iTween.MoveTo(this.gameObject, iTween.Hash("position", destination, "time", secondsToEnemy, "easetype", iTween.EaseType.easeOutQuad));
        Invoke("SpawnEnemy", secondsToEnemy);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyEntryEffect, this.gameObject.transform.position, Quaternion.identity);
        Instantiate(enemy, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void adjustTransform()
    {
        Vector2 screenbounds = GameManager.Instance.screenBounds;
        Vector2 position = this.gameObject.transform.position;
        if (Mathf.Approximately(position.x, screenbounds.x))
        {
            //Right of screen.
            this.gameObject.transform.Rotate(new Vector3(0, 0, 90));
            this.gameObject.transform.position += Vector3.right;
        }
        else if (Mathf.Approximately(position.y, screenbounds.y))
        {
            //Top of screen.
            this.gameObject.transform.Rotate(new Vector3(0, 0, 180));
            this.gameObject.transform.position += Vector3.up;
        }
        else if (Mathf.Approximately(-position.x, screenbounds.x))
        {
            //Left of screen.
            this.gameObject.transform.Rotate(new Vector3(0, 0, 270));
            this.gameObject.transform.position += Vector3.left;
        } else
        {
            //Bottom of screen.
            this.gameObject.transform.position += Vector3.down;
        }
    }
}
