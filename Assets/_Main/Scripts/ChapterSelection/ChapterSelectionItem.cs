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
        private ChapterSelectionData chapterSelectionData = default;
        private Action<int> loadChapter = default;

        public void Init(ChapterSelectionData chapterSelectionData, Action<int> loadChapter, bool isUnlock)
        {
            this.loadChapter = loadChapter;
            this.isUnlock = isUnlock;
            this.chapterSelectionData = chapterSelectionData;

            chapterImage.sprite = chapterSelectionData.chapterSprite;
            UpdateState(isUnlock);
        }

        public void OnChapterSelected()
        {
            if (!isUnlock)
                return;

            loadChapter?.Invoke(chapterSelectionData.chapter);
        }

        public void UpdateState(bool isUnlock)
        {
            chapterButton.interactable = isUnlock;
        }
    }
}
