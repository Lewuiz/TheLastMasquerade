using UnityEngine;

namespace Main
{
    public class StorySceneData 
    {
        public int SelectedChapter { get; private set; } = -1;

        public void SetSelectedChapter(int chapter)
        {
            SelectedChapter = chapter;
        }
    }
}
