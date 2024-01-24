using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Unity.Netcode;

public class VideoController : NetworkBehaviour
{
    VideoPlayer videoPlayer;
    [Header("VideoClips")]
    public VideoClip standby;
    public VideoClip repairing;
    public VideoClip repaired;
    public VideoClip broken;
    
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    /// <summary>
    /// Usado para mudar o video.
    /// <br> 0 = standby</br>
    /// <br>1 = broken</br>
    /// <br>2 = repairing</br>
    /// <br>3 = repaired</br>
    /// </summary>
    [ClientRpc]
    public void ChangeVideo_ClientRpc(int videoIndex)
    {
        switch (videoIndex)
        {
            case 0:
                videoPlayer.clip = standby;
                break;
            case 1:
                videoPlayer.clip = broken;
                break;
            case 2:
                videoPlayer.clip = repairing;
                break;
            case 3:
                videoPlayer.clip = repaired;
                break;
            default:
                break;
        }
    }
}
