using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class chasisManager : MonoBehaviour
{
    public List<Light> carLights;
    public movimientoPruebas movScript;
    public npcBehaviour npcBehaviourScript;
    public List<GameObject> coches;
    public List<Material> glassMat;
    int randomCarIndedx;
    bool windowBreaked = false;
    int carIndex;

    public GameObject currentCar;
    void Start()
    {

        if ( transform .GetComponentInParent<movimientoPruebas>() .playerDriving )
            coches [0] .SetActive(true);//El jugador empieza con el coche dejavu siempre, gusto personal :)
        else
        {
            if ( npcBehaviourScript .isEnemy )
            {
                if ( npcBehaviourScript .enemyDasher )
                    coches [1] .SetActive(true);
                else
                if ( npcBehaviourScript .enemyShooter )
                    coches [2] .SetActive(true);
            }
            else
            {
                int rand = Random .Range(100 , 0);

                if ( rand > 0 && rand < 5 )
                    randomCarIndedx = 4;//deport
                if ( rand > 5 && rand < 20 )
                    randomCarIndedx = 0;//dejavu
                if ( rand > 20 && rand < 60 )
                    randomCarIndedx = 3;//avg
                if ( rand > 60 && rand < 70 )
                    randomCarIndedx = 6;//fuel
                if ( rand > 70 && rand < 75 )
                    randomCarIndedx = 8;//blindado
                if ( rand > 75 && rand < 90 )
                    randomCarIndedx = 5;//furgoneta
                if ( rand > 90 && rand < 100 )
                    randomCarIndedx = 7;//carga


                coches [randomCarIndedx] .SetActive(true);
            }

        }

        for ( int i = 0 ; i < 8 ; i++ )
        {
            if ( coches [i] .activeSelf )
                carIndex = i;
        }

        currentCar = coches [carIndex] .gameObject;
    }

    // Update is called once per frame


    /* void setGlassMaterial(int carIndex)
     {

         if (carIndex == 0)
         {
             if (windowBreaked)
                 coches[carIndex].GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(glassMat[1]);
             else
                 coches[carIndex].GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(glassMat[0]);

         }
         if (carIndex == 1)
         {
             if (windowBreaked)
                 coches[carIndex].GetComponent<MeshRenderer>().materials[2].CopyPropertiesFromMaterial(glassMat[1]);
             else
                 coches[carIndex].GetComponent<MeshRenderer>().materials[2].CopyPropertiesFromMaterial(glassMat[0]);

         }
         if (carIndex >= 2 && carIndex <= 5)
         {
             if (windowBreaked)
                 coches[carIndex].GetComponent<MeshRenderer>().materials[3].CopyPropertiesFromMaterial(glassMat[1]);
             else
                 coches[carIndex].GetComponent<MeshRenderer>().materials[3].CopyPropertiesFromMaterial(glassMat[0]);

         }
         if (carIndex == 6 || randomCarIndedx <= 7)
         {
             if (windowBreaked)
                 coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[1]);
             else
                 coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[0]);

         }

     }*/

    public void setBrakeLights( int carIndex , bool lightsOn )
    {

        /*
                 if (carIndex == 0 || carIndex == 1 || carIndex == 4 || carIndex == 5)
                  {

                      if (lightsOn)
                          coches[carIndex].GetComponent<MeshRenderer>().materials[2].CopyPropertiesFromMaterial(glassMat[1]);
                      else
                          coches[carIndex].GetComponent<MeshRenderer>().materials[2].CopyPropertiesFromMaterial(glassMat[0]);

                  }
                  if (carIndex == 2)
                  {
                      if (lightsOn)
                          coches[carIndex].GetComponent<MeshRenderer>().materials[5].CopyPropertiesFromMaterial(glassMat[1]);
                      else
                          coches[carIndex].GetComponent<MeshRenderer>().materials[5].CopyPropertiesFromMaterial(glassMat[0]);

                  }
                  if (carIndex == 3)
                  {

                      if (lightsOn)
                          coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[1]);
                      else
                          coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[0]);

                  }
                  if (randomCarIndedx == 6)
                  {
                      if (lightsOn)
                          coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[1]);
                      else
                          coches[carIndex].GetComponent<MeshRenderer>().materials[4].CopyPropertiesFromMaterial(glassMat[0]);

                  }*/



    }

    int carMeshIndex;

    public float playerLightIntensity, playerLightRange, enemyDiferential;

    void Update()
    {

        if ( GetComponentInParent<movimientoPruebas>() .casting )
        {
            setBrakeLights(carIndex , false);
        }
        else
        {
            setBrakeLights(carIndex , true);
        }

        if ( transform .parent .tag == "controlando" )
        {
            carLights [0] .intensity = playerLightIntensity;
            carLights [0] .range = playerLightRange;
        }
        else
        {
            carLights [0] .intensity = playerLightIntensity / enemyDiferential;
            carLights [0] .range = playerLightRange / enemyDiferential;
        }

    }


}
