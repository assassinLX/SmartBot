using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public GameObject[] gameObjectMaps;

    public GameObject Main_Character;
    public Vector3 currCharacterentPos;

    public GameObject CharacterOnBlock;
    public GameObject NextOnBlock;

	private void Update()
	{
        if(Main_Character != null){
            
            currCharacterentPos = Main_Character.transform.position;

            for (int i = 0; i < gameObjectMaps.Length; i++)
            {
                if (Vector3.Distance(currCharacterentPos, gameObjectMaps[i].transform.position) <= 0.5f)
                {
                    CharacterOnBlock = gameObjectMaps[i];
                    if (i + 1 < gameObjectMaps.Length)
                    {
                        NextOnBlock = gameObjectMaps[i + 1];
                    }
                    else
                    {
                        NextOnBlock = CharacterOnBlock;
                    }
                    break;
                }
            }
        }
       
	}
}
