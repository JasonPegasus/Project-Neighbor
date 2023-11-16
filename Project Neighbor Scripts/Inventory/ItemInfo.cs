using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string ItemName;
    // public string Type;
    public Sprite Icon;
    [HideInInspector]
    public GameObject ItemObject;
    public float SoundRadius;
    public AudioClip HitSound;
    [HideInInspector]
    public GameObject SoundObject;
    [HideInInspector]
    public new AudioSource audio;
    
    void Start()
    {
        if (transform.tag != "Untagged")
        {
            ItemObject = transform.gameObject;
            transform.gameObject.AddComponent<ItemPhysics>();
        }

        gameObject.layer = LayerMask.NameToLayer("ItemLayer");

        SoundObject = new GameObject("HitSound");
        SoundObject.transform.SetParent(gameObject.transform);
        SoundObject.transform.localScale = new Vector3(1,1,1);
        SoundObject.transform.localPosition = new Vector3(0,0,0); 

        audio = SoundObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = HitSound;
        audio.volume = 0.2f;
        audio.spatialBlend = 1;
    }
}
