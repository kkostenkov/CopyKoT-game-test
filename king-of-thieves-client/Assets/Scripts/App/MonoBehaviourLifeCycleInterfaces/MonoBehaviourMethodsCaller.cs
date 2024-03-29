using System.Collections.Generic;

public class MonoBehaviourMethodsCaller : IAwakable, IUpdatable, IDestroyable
{
    private List<IUpdatable> updatables = new();
    private List<IDestroyable> destroyables = new();
    private List<IAwakable> awakables = new();

    public void Register(object someClass)
    {
        Register(someClass as IUpdatable);
        Register(someClass as IDestroyable);
        Register(someClass as IAwakable);
    }

    public void Awake()
    {
        foreach (var awakable in awakables) {
            awakable.Awake();
        }
    }

    public void Update()
    {
        foreach (var updatable in updatables) {
            updatable.Update();
        }
    }

    public void OnDestroy()
    {
        awakables.Clear();
        updatables.Clear();
        foreach (var destroyable in destroyables) {
            destroyable.OnDestroy();
        }

        destroyables.Clear();
    }

    private void Register(IAwakable awakable)
    {
        if (awakable == null) {
            return;
        }

        awakables.Add(awakable);
    }

    private void Register(IUpdatable updatable)
    {
        if (updatable == null) {
            return;
        }

        updatables.Add(updatable);
    }

    private void Register(IDestroyable destroyable)
    {
        if (destroyable == null) {
            return;
        }

        destroyables.Add(destroyable);
    }
}