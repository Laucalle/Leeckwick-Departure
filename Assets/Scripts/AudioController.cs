using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    public AudioSource asc;
    public Slider slider;
    // Use this for initialization
    void Start()
    {
        //asc = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVolume()
    {

        asc.volume = slider.value;
        
    }

}
