using System;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
            player = GameObject.FindWithTag("Player").transform;
    }   

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        // cameraPosition.x = player.position.x;
        //option for the right move only 
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        if (player.position.x > 177) //climbing the castle
        {
            cameraPosition.y = player.position.y+4f;
        }
        transform.position = cameraPosition;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
