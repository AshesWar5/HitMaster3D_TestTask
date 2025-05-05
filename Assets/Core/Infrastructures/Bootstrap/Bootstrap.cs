using System.Globalization;
using System.Threading;
using Core.Infrastructures.StateMachines.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core.Infrastructures.Bootstrap
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        
        private GameStateMachine _gameMachine;

        [Inject]
        public void Construct(GameStateMachine stateMachine) =>
            _gameMachine = stateMachine;

        private void Start()
        {
            CultureInfo customCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;
            
            DontDestroyOnLoad(_eventSystem);
            _gameMachine.Enter<BootstrapState>();
        }
    }
}
