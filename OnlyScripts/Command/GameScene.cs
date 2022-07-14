using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class GameScene : MonoBehaviour
    {
        [SerializeField] private Button _loadNextSceneButton;

        [SerializeField] private CanvasGroup _canvasGroup2;
        private List<ICommand> _loadNextSceneCommands;

        private void Awake()
        {
            _loadNextSceneButton.onClick.AddListener(LoadNextScene);
        }

        private void LoadNextScene()
        {
            foreach (var command in _loadNextSceneCommands)
            {
                CommandQueue.Instance.AddCommand(command);
            }
        }

        public void Configure(List<ICommand> loadNextSceneCommands)
        {
            _loadNextSceneCommands = loadNextSceneCommands;
        }
    }

