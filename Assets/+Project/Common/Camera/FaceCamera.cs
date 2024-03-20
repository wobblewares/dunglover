using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    [SerializeField] public float minDotProduct = -1.0f;
    [SerializeField] public float maxDotProduct = 1.0f;
    [SerializeField] public bool ignoreYInDotProduct;
    [SerializeField] private float lerpSpeed = 5.0f;
    [SerializeField] private bool hideWhenNotWithinDot = false;
    [SerializeField] private bool flipHorizontally = false;
    private Quaternion originalLocalRotation;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        originalLocalRotation = transform.localRotation;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraForward = ignoreYInDotProduct ? 
            new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) :
            Camera.main.transform.forward;
        
        Vector3 parentForward = ignoreYInDotProduct ? 
            new Vector3(transform.parent.forward.x, 0, transform.parent.forward.z) :
            transform.parent.forward;
        
        float dot = Vector3.Dot(-cameraForward, parentForward);
        if (dot <= minDotProduct || dot > maxDotProduct)
        {
            if (hideWhenNotWithinDot && _spriteRenderer)
            {
                _spriteRenderer.enabled = false;
            }
            transform.localRotation = originalLocalRotation;
            return;
        }
    
        if (hideWhenNotWithinDot&& _spriteRenderer) {
            _spriteRenderer.enabled = true;
        }
        
        if(flipHorizontally && _spriteRenderer)
        {
            _spriteRenderer.flipX =  Vector3.Dot(Camera.main.transform.right, transform.parent.forward) < 0;
        }
 
        transform.rotation = Quaternion.Slerp(
        transform.rotation, 
        Quaternion.LookRotation(Camera.main.transform.forward, transform.parent.up),
        lerpSpeed * Time.deltaTime
        );
    }
}
