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
            var gameInfo = new GameModeParameters(Model.MainMenu.Seed);
            _parametersPipeline.Set(gameInfo);
        }
    }
    //
    // interface IManagedState
    // {
    //     // todo: use it to validate FSM on start
    //     bool IsInputValid(Type parametersType);
    //     Type GetOutputType();
    //
    //     UniTask OnEnterAsync(IParameters parameters);
    //
    //     UniTask<IParameters> OnExitAsync(IParametersPool pool);
    // }
    //
    // interface IParameters
    // {
    // }
    //
    // class Parameters<T> : IParameters
    // {
    //     public T Value { get; set; }
    // }
    //
    // class EmptyParameters : IParameters
    // {
    //     public static readonly EmptyParameters Instance = new EmptyParameters();
    //
    //     private EmptyParameters()
    //     {
    //     }
    // }
    //
    // interface IParametersPool
    // {
    //     public IParameters GetInstance<T>(T value);
    // }
    //
    // abstract class ManagedState<TInput, TOutput> : IManagedState
    // {
    //     public bool IsInputValid(Type parametersType)
    //     {
    //         if (parametersType == typeof(EmptyParameters))
    //             return true;
    //
    //         return parametersType == typeof(TInput);
    //     }
    //
    //     public Type GetOutputType() => typeof(TOutput);
    //
    //     public UniTask OnEnterAsync(IParameters parameters) => parameters switch
    //     {
    //         Parameters<TInput> p => OnEnterAsync(p),
    //         EmptyParameters _ => OnEnterAsyncNoInput(),
    //         _ => throw new Exception()
    //     };
    //
    //     protected abstract UniTask OnEnterAsync(Parameters<TInput> parameters);
    //
    //     protected abstract UniTask OnEnterAsyncNoInput();
    //
    //     public async UniTask<IParameters> OnExitAsync(IParametersPool pool)
    //     {
    //         var (value, success) = await OnExitAsync();
    //
    //         if (success)
    //             return pool.GetInstance(value);
    //
    //         return EmptyParameters.Instance;
    //     }
    //
    //     protected abstract UniTask<(TOutput result, bool success)> OnExitAsync();
    // }
    //
    // class ConcreteState : ManagedState<Seed, TransferringRef<PoliticalActionInstance>>
    // {
    //     private ILifetime _lifetime;
    //
    //     public GameStore Store { get; private set; }
    //
    //     protected override UniTask OnEnterAsync(Parameters<Seed> parameters)
    //     {
    //         return OnEnterAsyncImpl(parameters.Value);
    //     }
    //
    //     protected override UniTask OnEnterAsyncNoInput()
    //     {
    //         return OnEnterAsyncImpl(new Seed(42));
    //     }
    //
    //     private UniTask OnEnterAsyncImpl(Seed seed)
    //     {
    //         _lifetime = null;
    //         Store = new GameStore(_lifetime, seed);
    //         return UniTask.CompletedTask;
    //     }
    //
    //     protected async override UniTask<(TransferringRef<PoliticalActionInstance>, bool)> OnExitAsync()
    //     {
    //         if (Store.PoliticalActions.TryDeref(out var politicalActionsStore))
    //         {
    //             var transfer = politicalActionsStore.ExtractSelectedAction();
    //             return (transfer, true);
    //         }
    //
    //         return (default, false);
    //     }
    // }
    //
    // interface IInitializerTransition<T>
    // {
    //     public T Value { get; }
    // }
    //
    //
    // class OnSignalTransition<T> : IReusableTransition
    // {
    //     public bool ToTransition { get; private set; }
    //
    //     public void Reset()
    //     {
    //         ToTransition = false;
    //     }
    //
    //     public void OnSignal(T signal)
    //     {
    //         ToTransition = true;
    //     }
    // }
    //
    // readonly struct Seed
    // {
    //     public Seed(int value) => Value = value;
    //
    //     public int Value { get; }
    // }
    //
    //
    // interface ILifetime
    // {
    //     public bool IsTerminated { get; }
    //     public void Add(IDisposable item);
    //     public void TransferTo(IDisposable item, ILifetime newOwner);
    //     bool Contains(IDisposable obj);
    //     public ILifetime CreateChild();
    //
    //     Ref<T> GetRef<T>(T obj) where T : class, IDisposable;
    //     // { if(Contains(obj)) return new Ref(obj, this);
    // }
    //
    // interface ILifetimeDefinition
    // {
    //     public void Terminate();
    //     public ILifetime Lifetime { get; }
    // }
    //
    // readonly struct Ref<T> : IEquatable<Ref<T>>
    //     where T : class, IDisposable
    // {
    //     private readonly T _obj;
    //     private readonly ILifetime _lifetime;
    //     private readonly bool _initialized;
    //
    //     public Ref(T obj, ILifetime lifetime)
    //     {
    //         _obj = obj;
    //         _lifetime = lifetime;
    //         _initialized = true;
    //     }
    //
    //     public bool TryDeref([NotNullWhen(true)] out T? obj)
    //     {
    //         if (!_initialized || _lifetime.IsTerminated || !_lifetime.Contains(_obj))
    //         {
    //             obj = null;
    //             return false;
    //         }
    //
    //         obj = _obj;
    //         return true;
    //     }
    //
    //     public void Deref(Action<T> action)
    //     {
    //         if (!_initialized)
    //             return;
    //
    //         if (TryDeref(out var obj))
    //             action(obj);
    //     }
    //
    //     public Ref<TResult> Deref<TResult>(Func<T, Ref<TResult>> func)
    //         where TResult : class, IDisposable
    //     {
    //         if (TryDeref(out var obj))
    //         {
    //             return func(obj);
    //         }
    //
    //         return default;
    //     }
    //
    //     public TResult Unwrap<TResult>(Func<T, TResult> func, TResult @default)
    //     {
    //         if (TryDeref(out var obj))
    //         {
    //             return func(obj);
    //         }
    //
    //         return @default;
    //     }
    //
    //     #region Equality
    //
    //     public bool Equals(Ref<T> other) => EqualityComparer<T>.Default.Equals(_obj, other._obj);
    //
    //     public override bool Equals(object? obj) => obj is Ref<T> other && Equals(other);
    //
    //     public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(_obj);
    //
    //     public static bool operator ==(Ref<T> left, Ref<T> right) => left.Equals(right);
    //
    //     public static bool operator !=(Ref<T> left, Ref<T> right) => !left.Equals(right);
    //
    //     #endregion
    // }
    //
    // struct TransferringRef<T>
    //     where T : class, IDisposable
    // {
    //     private T? _obj;
    //     private ILifetime? _lifetime;
    //
    //     public TransferringRef(T obj, ILifetime lifetime)
    //     {
    //         _obj = obj;
    //         _lifetime = lifetime;
    //     }
    //
    //     public bool TransferOwnership(ILifetime newOwner, out Ref<T> reference)
    //     {
    //         reference = default;
    //
    //         if (_lifetime is null || _obj is null)
    //             return false;
    //
    //         if (!_lifetime.Contains(_obj))
    //             return false;
    //
    //         _lifetime.TransferTo(_obj, newOwner);
    //
    //         reference = _lifetime.GetRef(_obj);
    //
    //         _obj = null;
    //         _lifetime = null;
    //
    //         return true;
    //     }
    // }
    //
    // class RefCollection<T> : IReadOnlyList<Ref<T>>
    //     where T : class, IDisposable
    // {
    //     private readonly ILifetime _lifetime;
    //     private readonly List<Ref<T>> _list = new List<Ref<T>>();
    //
    //     public RefCollection(ILifetime lifetime)
    //     {
    //         _lifetime = lifetime;
    //     }
    //
    //     public void Add(T item) => _list.Add(new Ref<T>(item, _lifetime));
    //
    //     public TransferringRef<T> Extract(T item)
    //     {
    //         int index = 0;
    //         foreach (var @ref in _list)
    //         {
    //             if (@ref.TryDeref(out var obj))
    //             {
    //                 if (obj.Equals(item))
    //                     break;
    //             }
    //             else
    //                 // todo: remove this ref from list
    //                 Debug.LogError("Memory leak");
    //
    //             index++;
    //         }
    //
    //         if (index < _list.Count)
    //         {
    //             _list.RemoveAt(index);
    //             return new TransferringRef<T>(item, _lifetime);
    //         }
    //
    //         throw new Exception();
    //     }
    //
    //     public TransferringRef<T> Extract(int index)
    //     {
    //         if (_list[index].TryDeref(out var item))
    //         {
    //             _list.RemoveAt(index);
    //             return new TransferringRef<T>(item, _lifetime);
    //         }
    //
    //         throw new Exception();
    //     }
    //
    //     public IEnumerator<Ref<T>> GetEnumerator() => _list.GetEnumerator();
    //
    //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //
    //     public int Count => _list.Count;
    //
    //     public Ref<T> this[int index] => _list[index];
    // }
    //
    // class GameStore
    // {
    //     private readonly ILifetime _lifetime;
    //
    //     public GameStore(ILifetime lifetime, Seed seed)
    //     {
    //         _lifetime = lifetime;
    //         Seed = seed;
    //         Parliament = new Ref<ParliamentEntity>(new ParliamentEntity(), lifetime);
    //         PoliticalActions =
    //             new Ref<PoliticalActionsStore>(new PoliticalActionsStore(_lifetime.CreateChild()), _lifetime);
    //     }
    //
    //     public Seed Seed { get; }
    //
    //     public Ref<ParliamentEntity> Parliament { get; }
    //
    //     public Ref<PoliticalActionsStore> PoliticalActions { get; }
    // }
    //
    // class ParliamentEntity : IDisposable
    // {
    //     public IReadOnlyList<Politician> Politicians { get; } = new[]
    //     {
    //         new Politician {Name = "Sasha"}
    //     };
    //
    //     public void Dispose()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
    //
    // class Politician
    // {
    //     public string Name { get; set; }
    //
    //     private void OnAssignPoliticalAction(object politicalAction)
    //     {
    //     }
    // }
    //
    // class PoliticalActionsStore : BaseStore, IDisposable
    // {
    //     public PoliticalActionsStore(ILifetime lifetime)
    //     {
    //         _lifetime = lifetime;
    //     }
    //
    //     public Ref<PoliticalActionInstance> ActiveAction { get; }
    //
    //
    //     public Ref<PoliticalActionInstance>? SelectedAction { get; private set; }
    //
    //     public RefCollection<PoliticalActionInstance> PoliticalActions { get; }
    //
    //     public void OnActionSelected(TransferringRef<PoliticalActionInstance> action)
    //     {
    //         if (action.TransferOwnership(_lifetime, out var selectedAction))
    //             SelectedAction = selectedAction;
    //     }
    //
    //     // todo: make it available only via class + use IPoliticalActionStore for other things
    //     // todo: or use dispatching
    //     internal TransferringRef<PoliticalActionInstance> ExtractSelectedAction()
    //     {
    //         if (SelectedAction != null)
    //         {
    //             if (TryExtract(this, t => t.SelectedAction!.Value, t => t.SelectedAction = null,
    //                 out var transfer))
    //             {
    //                 return transfer;
    //             }
    //         }
    //
    //         return default;
    //     }
    //
    //     public void Dispose()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
    //
    // class BaseStore
    // {
    //     protected ILifetime _lifetime;
    //
    //     protected bool TryExtract<TThis, TResult>(TThis that, Func<TThis, Ref<TResult>> selector, Action<TThis> cleanup,
    //         out TransferringRef<TResult> transfer)
    //         where TResult : class, IDisposable
    //         where TThis : BaseStore
    //     {
    //         var valueRef = selector(that);
    //         if (valueRef.TryDeref(out var value))
    //         {
    //             cleanup(that);
    //             transfer = new TransferringRef<TResult>(value, _lifetime);
    //             return true;
    //         }
    //
    //         transfer = default;
    //         return false;
    //     }
    // }
    //
    // class PoliticalActionInstance : IDisposable
    // {
    //     public int Intel { get; }
    //
    //     public void Dispose()
    //     {
    //     }
    // }
    //
    // class PlayerTabletViewModel
    // {
    //     private readonly GameStore _gameStore;
    //
    //     public PlayerTabletViewModel(GameStore gameStore)
    //     {
    //         _gameStore = gameStore;
    //     }
    //
    //     public int GetActiveIntel()
    //     {
    //         if (_gameStore.PoliticalActions.Deref(r => r.ActiveAction).TryDeref(out var politicalAction))
    //         {
    //         }
    //
    //         return _gameStore.PoliticalActions.Deref(r => r.ActiveAction).Unwrap(r => r.Intel, 0);
    //     }
    //
    //     public string GetFirstPoliticianName()
    //     {
    //         return _gameStore.Parliament.Unwrap(p => p.Politicians.First().Name, "");
    //     }
    //
    //     public TransferringRef<PoliticalActionInstance> TakeAction(int index)
    //     {
    //         if (_gameStore.PoliticalActions.TryDeref(out var politicalActionsStore))
    //         {
    //             return politicalActionsStore.PoliticalActions.Extract(index);
    //         }
    //
    //         throw new Exception();
    //     }
    // }
}