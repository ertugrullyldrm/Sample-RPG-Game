using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour
{

    public GameManager gameManager;
    public Transform windows;
    public Transform player;
    public CharacterStatus characterStatus;

    public static Transform characteristics;

    // Start is called before the first frame update
    public void Start()
    {
        windows = GameObject.FindWithTag("UI").transform.Find("Windows").transform;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        player = gameManager.player;
        characterStatus = player.GetComponent<CharacterStatus>();

        characteristics = transform.Find("Characteristics");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ToggleWindow( string windowName )
    {
        if( windowName == "Characteristics")
        {
            bool isActive = characteristics.gameObject.activeSelf;
            characteristics.gameObject.SetActive(!isActive);
        }
    }

}
