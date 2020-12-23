using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.AI.DecisionMaking
{
    [Serializable]
    internal class BehaviorTree : IBehaviorTree
    {
        private IBehaviorTreeTask _root;
        private bool _disposed;

        public void SetRootTask(IBehaviorTreeTask task)
        {
            CleanupCurrent();

            _root = task;
        }

        public async UniTask RunLoop()
        {
            // Debug.Log("Start");
            while (!_disposed)
            {
                // Debug.Log("Next");
                if (_root == null)
                    await UniTask.WaitWhile(RootIsNotSet);

                // Debug.Log($"Run {_root}");
                
                var result = await _root!.RunAsync();
                
                // todo: think how to deal with the result

                CleanupCurrent();                
            }
        }

        private bool RootIsNotSet() => _root == null;

        private void CleanupCurrent()
        {
            if (_root != null)
            {
                _root.Cancel();
                _root.Dispose();
            }
            _root = null;
        }

        public void Dispose()
        {
            CleanupCurrent();
            _disposed = true;
        }
    }
}