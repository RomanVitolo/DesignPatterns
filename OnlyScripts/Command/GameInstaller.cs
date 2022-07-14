using System;
using System.Collections.Generic;
using UnityEngine;

    public partial class GameInstaller : MonoBehaviour
    {
        [SerializeField] private Inicio _Inicio;
        [SerializeField] private GameScene _GameScene;

        [SerializeField] private CanvasGroup _canvasGroup1;
        [SerializeField] private CanvasGroup _canvasGroup2;
        
        private void Awake()
        {
            var inicioCommands = new List<ICommand>
            {
                new CanvasFadeCommand(_canvasGroup1, 0, 0.5f),
                new CanvasFadeCommand(_canvasGroup2, 1, 0.5f)
            };
            _Inicio.Configure(inicioCommands);
            
            
            var gameSceneCommands = new List<ICommand>
            {
                new CanvasFadeCommand(_canvasGroup2, 0, 0.5f),
                new LoadSceneCommand("Game")
            };
            _GameScene.Configure(gameSceneCommands);
        }
    }
