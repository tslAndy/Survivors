using Arch.Core;
using Utils;

namespace Systems.Physics;

// можно сделать этот компонент глобальным
// все кто хочет запомнить коллизию должны заносить ее каждый кадр
// если не была занесена в течении одного кадра, то удаляется в начале след кадра
// таким образом пуля может проверя
//
// переделать в систему
public class CollisionRegistry
{
    private readonly Dictionary<(Entity, Entity), CollState> _collStates;
    private readonly CachedList<(Entity, Entity)> _removeList;

    public CollisionRegistry()
    {
        _collStates = new Dictionary<(Entity, Entity), CollState>();
        _removeList = CachedList<(Entity, Entity)>.Create();
    }

    // should be called before AddColl
    public void Update()
    {
        foreach (KeyValuePair<(Entity, Entity), CollState> kvp in _collStates)
        {
            if ((kvp.Value & CollState.Exit) != 0)
                _removeList.Add(kvp.Key);
            else
                _collStates[kvp.Key] = kvp.Value | CollState.Exit;
        }

        for (int i = 0; i < _removeList.Count; i++)
            _collStates.Remove(_removeList[i]);
        _removeList.Reset();
    }

    // should be called before GetCollState
    public CollState AddColl(Entity first, Entity second)
    {
        if (_collStates.TryGetValue((first, second), out CollState state))
        {
            _collStates[(first, second)] = CollState.Stay;
            return CollState.Stay;
        }
        else
        {
            _collStates[(first, second)] = CollState.Enter;
            return CollState.Enter;
        }
    }

    public CollState GetColl(Entity first, Entity second)
    {
        if (_collStates.TryGetValue((first, second), out CollState state))
            return state;
        return default;
    }

    public enum CollState
    {
        None = 0,
        Enter = 1,
        Stay = 1 << 1,
        Exit = 1 << 2,
    }
}
