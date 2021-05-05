using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    [SerializeField]
    GameObject[] videos = null;

    [SerializeField]
    string[] names = null;

    int videoCount = 9;
    // Start is called before the first frame update
    void Start()
    {


        for(int i = 0; i < videos.Length; i++)
        {
            VideoPlayer player = videos[i].GetComponent<VideoPlayer>();
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, names[i % videoCount]);
            //player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "basics.mp4");
            player.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
