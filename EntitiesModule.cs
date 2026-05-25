using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Autofac;
using Behaviours.Specific;
using Components.Basic;
using Components.Behaviour;
using Components.Fighting;
using Components.Health;
using Components.Loot;
using Components.Physics;
using Engine.Animations;
using Engine.Common;
using Utils;

class EntitiesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // -----------------------------Player-----------------------------
        builder
            .Register<Entity>(x =>
            {
                AnimAtlas playerAnimAtlas = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Entities/Player.animAtlas");

                CachedList<WeaponElem> weapons = CachedList<WeaponElem>.Create();
                CachedList<ShieldElem> shields = CachedList<ShieldElem>.Create();
                CachedList<Line> lines = CachedList<Line>.Create();

                Entity player = x.Resolve<World>()
                    .Create<
                        PlayerTag,
                        BehaviourComp,
                        MoveComp,
                        SpriteComp,
                        AnimComp,
                        TrsComp,
                        RigidComp,
                        CollComp,
                        ShieldComp,
                        WeaponComp,
                        DamageComp,
                        HealthComp,
                        StatusEffectComp,
                        LootCollComp,
                        ModComp
                    >(
                        new PlayerTag(),
                        new BehaviourComp { behaviour = x.Resolve<PlayerBehaviour>() },
                        new MoveComp { maxSpeed = 3.0f },
                        new SpriteComp { drawOrder = 1 },
                        new AnimComp
                        {
                            atlas = playerAnimAtlas,
                            anim = playerAnimAtlas["Idle_Up"],
                            animDir = AnimDir.Up,
                            groupHash = AnimAtlas.CountHash("Idle"),
                        },
                        new TrsComp
                        {
                            position = new Vector2(15.0f, 10.0f),
                            scale = 1.0f,
                            descs = CachedList<Entity>.Create(),
                        },
                        new RigidComp { layer = x.Resolve<LayerMap>()["PlayerEnts"] },
                        new CollComp { radius = 0.5f },
                        new ShieldComp { shields = shields },
                        new WeaponComp { weapons = weapons },
                        new DamageComp { hits = CachedList<Hit>.Create() },
                        new HealthComp { currentHP = 100, maxHP = 100 },
                        new StatusEffectComp
                        {
                            newEffects = CachedList<StatusEffect>.Create(),
                            runningEffects = CachedList<StatusEffect>.Create(),
                        },
                        new LootCollComp { radius = 15.0f, speed = 10.0f },
                        new ModComp()
                    );

                WeaponElem weaponElem = x.ResolveNamed<WeaponElem>("simpleLaser");
                weapons.Add(weaponElem);
                if (weaponElem.entity != null)
                    player.Get<TrsComp>().descs?.Add(weaponElem.entity.Value);

                // ShieldElem shieldElem = x.ResolveNamed<ShieldElem>("reflectShield");
                // shields.Add(shieldElem);
                // if (shieldElem.entity != null)
                //     player.Get<TrsComp>().descs?.Add(shieldElem.entity.Value);

                return player;
            })
            .Named<Entity>("player")
            .InstancePerDependency();

        // -----------------------------Goblin Weapons-----------------------------
        builder
            .Register<CachedList<WeaponElem>>(x =>
            {
                CachedList<WeaponElem> result = new CachedList<WeaponElem>();
                // result.Add(x.ResolveNamed<WeaponElem>("goblinBow"));
                return result;
            })
            .Named<CachedList<WeaponElem>>("goblinWeapons")
            .InstancePerLifetimeScope();

        // -----------------------------Goblin-----------------------------
        builder
            .Register<Entity>(x =>
            {
                AnimAtlas goblinAtlas = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Entities/Goblin.animAtlas");

                Entity enemy = x.Resolve<World>()
                    .Create<
                        EnemyTag,
                        BehaviourComp,
                        MoveComp,
                        SpriteComp,
                        AnimComp,
                        TrsComp,
                        RigidComp,
                        CollComp,
                        HealthComp,
                        DamageComp,
                        StatusEffectComp,
                        DropComp,
                        ModComp,
                        WeaponComp
                    >(
                        new EnemyTag(),
                        new BehaviourComp { behaviour = x.Resolve<GoblinBehaviour>() },
                        new MoveComp { maxSpeed = 1.0f },
                        new SpriteComp { drawOrder = 1 },
                        new AnimComp
                        {
                            atlas = goblinAtlas,
                            anim = goblinAtlas["Idle_Down"],
                            animDir = AnimDir.Down,
                            groupHash = AnimAtlas.CountHash("Idle"),
                        },
                        new TrsComp
                        {
                            position = new Vector2(
                                2.0f + Random.Shared.NextSingle() * 36.0f,
                                2.0f + Random.Shared.NextSingle() * 26.0f
                            ),
                            scale = 1.0f,
                        },
                        new RigidComp { layer = x.Resolve<LayerMap>()["EnemyEnts"] },
                        new CollComp { radius = 0.5f },
                        new HealthComp { currentHP = 100, maxHP = 100 },
                        new DamageComp { hits = CachedList<Hit>.Create() },
                        new StatusEffectComp
                        {
                            newEffects = CachedList<StatusEffect>.Create(),
                            runningEffects = CachedList<StatusEffect>.Create(),
                        },
                        new DropComp { amount = 1000 },
                        new ModComp(),
                        new WeaponComp
                        {
                            weapons = x.ResolveNamed<CachedList<WeaponElem>>("goblinWeapons"),
                        }
                    );

                return enemy;
            })
            .Named<Entity>("goblin")
            .InstancePerDependency();
    }
}
