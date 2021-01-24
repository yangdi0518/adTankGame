using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerPrefab;
    public GameObject[] enemyPrefabList;
    public bool createPlayer;
    void Start()
    {
        Invoke("TankBorn", 1f);
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TankBorn() {

        if(createPlayer) {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        } else {
        int num = Random.Range(0,2);
        Instantiate(enemyPrefabList[num], transform.position, Quaternion.identity);
        }

    }

}
