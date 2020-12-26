using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;

namespace Kugushev.Scripts.Game.AI.BehaviorTree
{
    [Serializable]
    internal class BehaviorTreeManager : IBehaviorTreeManager
    {
        private IBehaviorTreeTask _root;
        private readonly List<IBehaviorTreeTask> _pushedOut = new List<IBehaviorTreeTask>(32);
        private bool _inProgress;

        private bool _disposed;

        // todo: fix this GC pressure
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public void SetRootTask(IBehaviorTreeTask task)
        {
            if (_inProgress)
                _cancellationTokenSource.Cancel(false);
            
            PushOutCurrent();

            _root = task;
        }

        public async UniTask RunLoop()
        {
            while (!_disposed)
            {
                if (_pushedOut.Count > 0)
                {
                    DisposePushedOutTasks();
                }

                if (_root == null)
                    await UniTask.WaitWhile(RootIsNotSet);

                try
                {
                    _inProgress = true;
                    // todo: think how to deal with the result
                    await _root!.RunAsync(_cancellationTokenSource.Token);
                    PushOutCurrent();
                }
                catch (OperationCanceledException) // todo: use manual cancellation to avoid allocations
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = new CancellationTokenSource();
                }
                finally
                {
                    _inProgress = false;
                }
            }
        }

        private void DisposePushedOutTasks()
        {
            foreach (var toDispose in _pushedOut)
            {
                toDispose.Dispose();
            }

            _pushedOut.Clear();
        }

        private bool RootIsNotSet() => _root == null;

        private void PushOutCurrent()
        {
            if (_root != null)
            {
                _pushedOut.Add(_root);
                _root = null;
            }
        }

        public void Dispose()
        {
            PushOutCurrent();
            DisposePushedOutTasks();
            _cancellationTokenSource.Dispose();
            _disposed = true;
        }
    }
}