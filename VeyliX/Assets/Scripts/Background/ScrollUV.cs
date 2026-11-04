using Unity.Mathematics;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    public Transform player; //used to follow the player, while the background scrolls 
    public float scrollSpeed;
    public float sortingOrderZ; //sort it behind the player and other sprites 
    private Material mat;
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        if (player == null) return;
        Vector3 pos = player.position;
        transform.position = new Vector3(pos.x, sortingOrderZ, pos.z); //scrolls the UV of the sprite
        Vector2 offset = new Vector3(pos.x, pos.z) * scrollSpeed;
        mat.mainTextureOffset = offset;
    }
}



/* 
ship stun when hit 
boss alien 
more enemies 
stats ui 
hide minimap 
sound 
check if enemy is outside the zone 

*/