using Achieves;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Autofac;
using Behaviours.Specific;
using Engine;
using Levels;
using Other;
using Raylib_cs;
using Systems;
using Systems.Basic;
using UI;
using Weapons.Specific;

class Game : IDisposable
{
    private readonly ILifetimeScope _scope;

    private readonly World _world;
    private readonly CommandBuffer _commandBuffer;
    private readonly Group<float> _gameplaySystems,
        _renderingSystems;

    public bool isPaused;

    public Game()
    {
        ContainerBuilder builder = new ContainerBuilder();

        builder.RegisterModule(new EngineModule());
        builder.RegisterModule(new SystemsModule());
        builder.RegisterModule(new WeaponsModule());
        builder.RegisterModule(new BehavioursModule());
        builder.RegisterModule(new EntitiesModule());
        builder.RegisterModule(new AchievesModule());
        builder.RegisterModule(new OtherModule());
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
        _scope.Resolve<ExpSys>();
        _scope.Resolve<MainUI>();

        // LEVEL

        _scope.Resolve<LevelSys>().level = Level_One.Get(_scope);
    }

    //
    public void Update()
    {
        if (!isPaused)
            _gameplaySystems.Update(Raylib.GetFrameTime());

        _renderingSystems.Update(Raylib.GetFrameTime());

        _commandBuffer.Playback(_world, true);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
