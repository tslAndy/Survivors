using Utils;
using Weapons;

namespace Components.Fighting;

struct WeaponComp
{
    public CachedList<Weapon> weapons;
    public Weapon single;
}
/*
 * Если игрок, то список содержит много компонентов
 * Если враг, то список пустой и только одно оружие
 * */
