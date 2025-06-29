using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ChapterSelectionItem : MonoBehaviour
    {
        [SerializeField] private Image chapterImage = default;
        [SerializeField] private Button chapterButton = default;

        private bool isUnlock = false;
        private Action<int> loadChapter = default;
        private int chapter = default;

        public void Init(Sprite thumbnailSprite, int chapter, Action<int> loadChapter, bool isUnlock)
        {
            this.loadChapter = loadChapter;
            this.isUnlock = isUnlock;
            this.chapter = chapter;
            
            chapterImage.sprite = thumbnailSprite;
            UpdateState(isUnlock);
        }

        public void OnChapterSelected()
        {
            if (!isUnlock)
                return;

            loadChapter?.Invoke(chapter);
        }

        public void UpdateState(bool isUnlock)
        {
            chapterButton.interactable = isUnlock;
        }
    }
}
