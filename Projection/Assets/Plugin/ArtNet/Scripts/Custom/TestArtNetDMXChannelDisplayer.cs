using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestArtNetDMXChannelDisplayer : MonoBehaviour
{
    [SerializeField] int channelNumber = 0;
    private Text label;
    private ArtDotNet.ArtNetClient client;
    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<ArtDotNet.ArtNetClient>();
        label = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (client != null)
        {
            label.text = client.DMXdata[channelNumber].ToString();
        }
    }
}
