using Utils;
using Weapons;

namespace Components.Fighting;

struct ShieldComp
{
    public CachedList<Shield> shields;
    public Shield single;
}

/*
 * Если игрок, то список содержит много компонентов
 * Если враг, то список пустой и только одно оружие
 * */
