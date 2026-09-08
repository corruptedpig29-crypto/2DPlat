using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tpToOtherScrollPlatTP : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject otherScrollRegionTP;
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        // 2. Create a list to hold the root objects
        List<GameObject> rootObjects = new List<GameObject>();
        activeScene.GetRootGameObjects(rootObjects);

        // 3. Loop through root objects and their hierarchies

        float xShift = 0f;
        if(this.gameObject.name == "TPLeft")
        {
            xShift = -1f;
        }
        else
        {
            xShift = 1f;
        }
        Vector2 pos;

        if (collision.CompareTag("Player"))    
        {
            float valRShift = otherScrollRegionTP.transform.position.x + xShift * 50f - playerMovement.transform.position.x;

            Debug.Log($"rootObjects: {rootObjects.Count}, valRShift: {valRShift}");
            foreach (GameObject rootObj in rootObjects)
            {
                Debug.Log(rootObj.name);
                if (rootObj.CompareTag("Monster") || rootObj.CompareTag("Projectile"))
                {
                    Debug.Log($"Shifting {rootObj.name} by {valRShift}");
                    rootObj.transform.position = new Vector2(rootObj.transform.position.x + valRShift, rootObj.transform.position.y);
                }
            }


            collision.gameObject.transform.position = pos = new Vector2(otherScrollRegionTP.transform.position.x + xShift * 50f,collision.transform.position.y );
        }

    }
    
}
