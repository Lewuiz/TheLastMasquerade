using System;
using UnityEngine;

namespace Main
{
    public class ChapterSelection : MonoBehaviour
    {
        [SerializeField] private ChapterSelectionItem chapterSelectionItemTemplate = default;

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
            for (int i = 0; i < storyManager.ChapterDatabase.chapters.Count; i++)
            {
                ChapterData chapterData = storyManager.ChapterDatabase.chapters[i];

                ChapterSelectionItem chapterSelectionItem = Instantiate(chapterSelectionItemTemplate, chapterSelectionItemTemplate.transform.parent);
                chapterSelectionItem.gameObject.SetActive(true);

                bool isUnlock = storyManager.CurrentChapter >= i;
                int chapter = i;

                chapterSelectionItem.Init(chapterData.thumbnail, chapter, GoToStoryScene, isUnlock);
            }
        }
    }
}
