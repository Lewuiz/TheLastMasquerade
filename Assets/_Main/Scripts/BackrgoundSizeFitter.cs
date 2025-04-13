using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class BackrgoundSizeFitter : MonoBehaviour
    {
        private void Awake()
        {
            FitToCamera();
        }

#if UNITY_EDITOR
        private void Update()
        {
            FitToCamera();
        }
#endif

        public void FitToCamera()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite == null)
                return;

            float screenHeight = Camera.main.orthographicSize * 2f;
            float screenWidth = screenHeight * Screen.width / Screen.height;

            Vector2 spriteSize = sr.sprite.bounds.size;
            float scaleX = screenWidth / spriteSize.x;
            float scaleY = screenHeight / spriteSize.y;

            transform.localScale = Mathf.Min(scaleX, scaleY) * Vector3.one;
        }
    }
}
