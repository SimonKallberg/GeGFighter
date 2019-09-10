using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projscript : MonoBehaviour
{
    GameObject Fireproj;
    private Transform projspawner;
    GameObject PlatformerCharacter2D;
    //GameObject projspawner;
    float projspeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Fireproj = GameObject.FindGameObjectWithTag("projectile");
        projspawner = transform.Find("projspawner");
        //PlatformerCharacter2D = GetComponent<PlatformerCharacter2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Input.GetAxis("Horizontal") < 0)
            {
                Debug.Log("vi skjuter åt vänster");
                GameObject projectile = Instantiate(Fireproj, projspawner.transform.position, transform.rotation) as GameObject;
                projectile.GetComponent<Rigidbody2D>().velocity = -transform.right * projspeed; 

          
            }
            else if(Input.GetAxis("Horizontal") > 0)
            {
                Debug.Log("vi skjuter åt höger");
                GameObject projectile = Instantiate(Fireproj, projspawner.transform.position, transform.localRotation) as GameObject;
                projectile.GetComponent<Rigidbody2D>().velocity = transform.right * projspeed;
                projectile.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                Debug.Log("vi skjuter åt hållet spelaren står mot");
                //GameObject projectile = Instantiate(Fireproj, projspawner.transform.position, transform.rotation) as GameObject;
                
                //projectile.GetComponent<Rigidbody2D>().velocity = transform. * projspeed;
            }

        }
    }
}
