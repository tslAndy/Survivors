using Achieves;
using Arch.Buffer;
using Arch.Bus;
using Arch.Core;
using Arch.System;
using Autofac;
using Behaviours.Specific;
using Engine;
using Events;
using Levels;
using Other;
using Raylib_cs;
using Systems;
using Systems.Basic;
using UI;
using Weapons.Specific;

namespace Scripts;

public partial class Game : IDisposable
{
    private readonly ILifetimeScope _scope;

    private readonly World _world;
    private readonly CommandBuffer _commandBuffer;
    private readonly Group<float> _gameplaySystems,
        _renderingSystems;

    private bool _isPaused;
    public bool isPaused
    {
        get => _isPaused;
        set
        {
            PausedEvent @event = new PausedEvent { isPaused = value };
            EventBus.Send(ref @event);
        }
    }

    [Event]
    public void OnPaused(ref PausedEvent @event) => _isPaused = @event.isPaused;

    public Game()
    {
        ContainerBuilder builder = new ContainerBuilder();

        builder.RegisterModule(new EngineModule());
        builder.RegisterModule(new SystemsModule());
        builder.RegisterModule(new WeaponsModule());
        builder.RegisterModule(new BehavioursModule());
        builder.RegisterModule(new EntitiesModule());
        builder.RegisterModule(new AchievesModule());
        builder.RegisterModule(new UIModule());

        IContainer container = builder.Build();
        _scope = container.BeginLifetimeScope();

        // main stuff
        _world = _scope.Resolve<World>();
        _commandBuffer = _scope.Resolve<CommandBuffer>();
        _gameplaySystems = _scope.ResolveNamed<Group<float>>("gameplaySystems");
        _renderingSystems = _scope.ResolveNamed<Group<float>>("renderingSystems");

        // other stuff
        _scope.Resolve<AchieveSys>();
        _scope.Resolve<MainUI>();

        // LEVEL

        _scope.Resolve<LevelSys>().level = Level_One.Get(_scope);

        Hook();
    }

    //
    public void Update()
    {
        if (!isPaused)
        {
            _commandBuffer.Playback(_world, true);
            _gameplaySystems.Update(Raylib.GetFrameTime());
        }

        _renderingSystems.Update(Raylib.GetFrameTime());
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
