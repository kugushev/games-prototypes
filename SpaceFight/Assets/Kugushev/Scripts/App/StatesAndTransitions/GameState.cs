using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    internal class GameState : BaseSceneLoadingState<AppModel>
    {
        private object Store { get; }

        private readonly GameSceneParametersPipeline _parametersPipeline;

        public GameState(AppModel model, GameSceneParametersPipeline parametersPipeline)
            : base(model, UnityConstants.GameManagementScene, false)
        {
            _parametersPipeline = parametersPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var gameInfo = new GameInfo(Model.MainMenu.Seed);
            _parametersPipeline.Set(gameInfo);
        }
    }

    interface IManagedState
    {
        // todo: use it to validate FSM on start
        bool IsTransitionValid(ITransition transition);

        void Setup(ITransition transition);
        UniTask OnEnterAsync();

        UniTask OnExitAsync();
        void TearDown();
    }

    abstract class ManagedState<TInput> : IManagedState
    {
        public bool IsTransitionValid(ITransition transition) => transition is IInitializingTransition<TInput>;


        public void Setup(ITransition transition)
        {
            var initializer = UnwrapInitializer(transition);
            Setup(initializer);
        }

        protected virtual void Setup(IInitializingTransition<TInput> initializer)
        {
        }

        public virtual UniTask OnEnterAsync() => UniTask.CompletedTask;

        public virtual UniTask OnExitAsync() => UniTask.CompletedTask;

        public virtual void TearDown()
        {
        }

        private IInitializingTransition<TInput> UnwrapInitializer(ITransition transition)
        {
            if (transition is IInitializingTransition<TInput> initializer)
                return initializer;
            throw new Exception();
        }
    }

    class ConcreteState : ManagedState<Seed>
    {
        private ILifetime _lifetime;

        public GameStore Store { get; private set; }

        protected override void Setup(IInitializingTransition<Seed> initializer)
        {
            _lifetime = null;
            Store = new GameStore(_lifetime);
        }
    }

    interface IInitializingTransition<T> : ITransition
    {
        public T Value { get; }
    }

    class CustomTransition<T> : IInitializingTransition<T>
    {
        public T Value { get; set; }
        public bool ToTransition { get; set; }
    }

    class MyTransition : IInitializingTransition<PoliticalActionInstance>
    {
        private readonly GameStore _gameStore;

        public MyTransition(GameStore gameStore)
        {
            _gameStore = gameStore;
        }
        
        
        как делать это??? как transtiion сможешь инициализировать store???? как ему открыть доступ, не открывая дургим??
        ОТВЕТ: интерфейс или тупо метод Initialize

        public T Value { get; set; }
        public bool ToTransition { get; set; }
    }

    readonly struct Seed
    {
        public Seed(int value) => Value = value;

        public int Value { get; }
    }


    interface ILifetime
    {
        public bool IsTerminated { get; }
        public void Add(IDisposable item);
        public void TransferTo(IDisposable item, ILifetime newOwner);
        public ILifetime CreateChild();
    }

    interface ILifetimeDefinition
    {
        public void Terminate();
        public ILifetime Lifetime { get; }
    }

    readonly struct Ref<T> : IEquatable<Ref<T>>
        where T : class
    {
        private readonly T _obj;
        private readonly ILifetime _lifetime;
        private readonly bool _initialized;

        public Ref(T obj, ILifetime lifetime)
        {
            _obj = obj;
            _lifetime = lifetime;
            _initialized = true;
        }

        public bool TryDeref([NotNullWhen(true)] out T? obj)
        {
            if (!_initialized || _lifetime.IsTerminated)
            {
                obj = null;
                return false;
            }

            obj = _obj;
            return true;
        }

        public void Deref(Action<T> action)
        {
            if (!_initialized)
                return;

            if (TryDeref(out var obj))
                action(obj);
        }

        public Ref<TResult> Deref<TResult>(Func<T, Ref<TResult>> func)
            where TResult : class
        {
            if (TryDeref(out var obj))
            {
                return func(obj);
            }

            return default;
        }

        public TResult Unwrap<TResult>(Func<T, TResult> func, TResult @default)
        {
            if (TryDeref(out var obj))
            {
                return func(obj);
            }

            return @default;
        }

        #region Equality

        public bool Equals(Ref<T> other) => EqualityComparer<T>.Default.Equals(_obj, other._obj);

        public override bool Equals(object? obj) => obj is Ref<T> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(_obj);

        public static bool operator ==(Ref<T> left, Ref<T> right) => left.Equals(right);

        public static bool operator !=(Ref<T> left, Ref<T> right) => !left.Equals(right);

        #endregion
    }

    struct TransferringRef<T>
        where T : class, IDisposable
    {
        private T? _obj;
        private ILifetime? _lifetime;

        public TransferringRef(T obj, ILifetime lifetime)
        {
            _obj = obj;
            _lifetime = lifetime;
        }

        public void TransferOwnership(ILifetime newOwner)
        {
            if (_lifetime is null || _obj is null)
                return;

            _lifetime.TransferTo(_obj, newOwner);
            _obj = null;
            _lifetime = null;
        }
    }

    class RefCollection<T> : IReadOnlyList<Ref<T>>
        where T : class, IDisposable
    {
        private readonly ILifetime _lifetime;
        private readonly List<Ref<T>> _list = new List<Ref<T>>();

        public RefCollection(ILifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public void Add(T item) => _list.Add(new Ref<T>(item, _lifetime));

        public TransferringRef<T> Extract(T item)
        {
            int index = 0;
            foreach (var @ref in _list)
            {
                if (@ref.TryDeref(out var obj))
                {
                    if (obj.Equals(item))
                        break;
                }
                else
                    // todo: remove this ref from list
                    Debug.LogError("Memory leak");

                index++;
            }

            if (index < _list.Count)
            {
                _list.RemoveAt(index);
                return new TransferringRef<T>(item, _lifetime);
            }

            throw new Exception();
        }

        public TransferringRef<T> Extract(int index)
        {
            if (_list[index].TryDeref(out var item))
            {
                _list.RemoveAt(index);
                return new TransferringRef<T>(item, _lifetime);
            }

            throw new Exception();
        }

        public IEnumerator<Ref<T>> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _list.Count;

        public Ref<T> this[int index] => _list[index];
    }

    class GameStore
    {
        private readonly ILifetime _lifetime;

        public GameStore(ILifetime lifetime)
        {
            _lifetime = lifetime;
            Parliament = new Ref<ParliamentEntity>(new ParliamentEntity(), lifetime);
            PoliticalActions =
                new Ref<PoliticalActionsStore>(new PoliticalActionsStore(_lifetime.CreateChild()), _lifetime);
        }

        public Ref<ParliamentEntity> Parliament { get; }

        public Ref<PoliticalActionsStore> PoliticalActions { get; }
    }

    class ParliamentEntity
    {
        public IReadOnlyList<Politician> Politicians { get; } = new[]
        {
            new Politician {Name = "Sasha"}
        };
    }

    class Politician
    {
        public string Name { get; set; }

        private void OnAssignPoliticalAction(object politicalAction)
        {
        }
    }

    class PoliticalActionsStore
    {
        private readonly ILifetime _lifetime;

        public PoliticalActionsStore(ILifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public Ref<PoliticalActionInstance> ActiveAction { get; }

        public RefCollection<PoliticalActionInstance> PoliticalActions { get; }
    }

    class PoliticalActionInstance : IDisposable
    {
        public int Intel { get; }

        public void Dispose()
        {
        }
    }

    class PlayerTabletViewModel
    {
        private readonly GameStore _gameStore;

        public PlayerTabletViewModel(GameStore gameStore)
        {
            _gameStore = gameStore;
        }

        public int GetActiveIntel()
        {
            if (_gameStore.PoliticalActions.Deref(r => r.ActiveAction).TryDeref(out var politicalAction))
            {
            }

            return _gameStore.PoliticalActions.Deref(r => r.ActiveAction).Unwrap(r => r.Intel, 0);
        }

        public string GetFirstPoliticianName()
        {
            return _gameStore.Parliament.Unwrap(p => p.Politicians.First().Name, "");
        }

        public TransferringRef<PoliticalActionInstance> TakeAction(int index)
        {
            if (_gameStore.PoliticalActions.TryDeref(out var politicalActionsStore))
            {
                return politicalActionsStore.PoliticalActions.Extract(index);
            }

            throw new Exception();
        }
    }
}