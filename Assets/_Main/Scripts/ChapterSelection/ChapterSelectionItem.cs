using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ChapterSelectionItem : MonoBehaviour
    {
        [SerializeField] private Image chapterImage = default;
        [SerializeField] private Button chapterButton = default;

        private bool isLocked = false;
        private ChapterSelectionData chapterSelectionData = default;
        private Action<int> loadChapter = default;

        public void Init(ChapterSelectionData chapterSelectionData, Action<int> loadChapter, bool isLocked)
        {
            this.loadChapter = loadChapter;
            this.isLocked = isLocked;
            this.chapterSelectionData = chapterSelectionData;

            chapterImage.sprite = chapterSelectionData.chapterSprite;
            UpdateState(isLocked);
        }

        public void OnChapterSelected()
        {
            if (isLocked)
                return;

            loadChapter?.Invoke(chapterSelectionData.chapter);
        }

        public void UpdateState(bool isLocked)
        {
            chapterButton.interactable = !isLocked;
        }
    }
}
