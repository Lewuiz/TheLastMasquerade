using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ChapterSelection : MonoBehaviour
    {
        [SerializeField] private ChapterSelectionItem chapterSelectionItemTemplate = default;

        private List<ChapterSelectionItem> chapterSelectionItemList = new List<ChapterSelectionItem>();
        private Action goToStoryScene = default;

        public void Init(Action goToStoryScene)
        {
            this.goToStoryScene = goToStoryScene;
            CreateChapterSelection();
        }

        private void GoToStoryScene(int chapter)
        {
            Overlay.Instance.LoadingOverlay.Show(.5f, () => 
            {
                GameCore.Instance.StorySceneData.SetSelectedChapter(chapter);
                goToStoryScene.Invoke();
            });
        }

        private void CreateChapterSelection()
        {
            StoryManager storyManager = GameCore.Instance.StoryManager;

            for (int i = 0; i < storyManager.StoryDataList.Count; i++)
            {
                ChapterSelectionItem chapterSelectionItem = Instantiate(chapterSelectionItemTemplate, chapterSelectionItemTemplate.transform.parent);
                chapterSelectionItem.gameObject.SetActive(true);
                ChapterSelectionData chapterSelectionData = storyManager.GetChapterSelectionData(i);
                bool isUnlock = storyManager.CurrentChapter < i;
                chapterSelectionItem.Init(chapterSelectionData, GoToStoryScene, isUnlock);
            }
        }
    }
}
