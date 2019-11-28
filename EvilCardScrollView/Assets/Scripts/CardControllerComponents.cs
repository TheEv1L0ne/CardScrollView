using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardControllerComponents : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    public RectTransform RectTransform => rectTransform;

    public RectTransform[] Parts => parts;

    [SerializeField]
    private RectTransform[] parts = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
