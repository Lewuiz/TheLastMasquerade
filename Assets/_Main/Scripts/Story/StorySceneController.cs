using UnityEngine;

namespace Main
{
    public class StorySceneController : RootSceneController
    {
        [SerializeField] private StoryRunner storyRunner = default;
        protected override void OnStartCompleted()
        {
            storyRunner.Init();
        }
    }
}
