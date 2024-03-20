#nullable enable

using System;
using UnityEngine;

/// <summary>
/// Switch between different sprite renderers based on camera facing angle.
/// </summary>
public class SpriteSwitcher : MonoBehaviour
{

    [SerializeField] private SpriteInfo[] spriteInfos;
    [SerializeField] public bool ignoreYInDotProduct;
    [SerializeField] private float lerpSpeed = 5.0f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float minimumSpriteChangeDelay = 0.25f;
    [SerializeField] private float _cameraForwardOffset = 0.1f;
    private float lastSpriteChangeTime = 0.0f;
    private float _initialUpOffset = 0.0f;
    #region Unity Lifecycle

    private void Awake()
    {
        _initialUpOffset = _spriteRenderer.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraForward = ignoreYInDotProduct ? 
            new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) :
            Camera.main.transform.forward;
        
        Vector3 parentForward = ignoreYInDotProduct ? 
            new Vector3(transform.forward.x, 0, transform.forward.z) :
            transform.forward;
        
        float dot = Vector3.Dot(-cameraForward, parentForward);
        var spriteInfo = GetTargetSpriteInfo(dot);
        
        // Non-null and different sprite
        if (spriteInfo is {} info) 
        {
            if (info.sprite != _spriteRenderer.sprite && Time.time - lastSpriteChangeTime > minimumSpriteChangeDelay)
            {
                _spriteRenderer.sprite = info.sprite;
                lastSpriteChangeTime = Time.time;
            }

            if (info.flipHorizontallyByFacingDirection)
            {
                _spriteRenderer.flipX = 
                    info.flipHorizontallyByFacingDirection && 
                    Vector3.Dot(Camera.main.transform.right, transform.forward) < 0;
            }
        }

        // Face the camera
        _spriteRenderer.transform.rotation = Quaternion.Slerp(
            _spriteRenderer.transform.rotation, 
            Quaternion.LookRotation(Camera.main.transform.forward, _spriteRenderer.transform.parent.up),
            lerpSpeed * Time.deltaTime
        );
    }
    

    #endregion

    private SpriteInfo? GetTargetSpriteInfo(float dot)
    {
        foreach (var spriteInfo in spriteInfos)
        {
            if (dot > spriteInfo.minDotProduct && dot <= spriteInfo.maxDotProduct)
            {
                return spriteInfo;
            }
        }
        return null;    
    }
    
    [System.Serializable]
    public struct SpriteInfo
    {
        public Sprite sprite;
        public bool flipHorizontallyByFacingDirection;
        public float minDotProduct;
        public float maxDotProduct;
    }
}
