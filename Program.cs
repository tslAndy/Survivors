using Raylib_cs;
using rlImGui_cs;

class Program
{
    public static void Main(string[] args)
    {
        Raylib.InitWindow(1280, 720, "Survivors");
        Raylib.InitAudioDevice();

        rlImGui.Setup(true);

        using Game game = new Game();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            game.Update();
            Raylib.DrawFPS(20, 20);

            Raylib.EndDrawing();
        }

        rlImGui.Shutdown();

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}

/*
 *
 *
 *
 *
 * TODO: для камикадзе сделать так что анимации взрыва это анимация смерти
 * разница в том что есть узел который проверяет расстояние до игрока
 * если подходящее то пытается достать до игрока, и наносит самому себе большой урон
 *
 * Пауза. Разделить системы на две части, одна работает независимо от паузы, другая только если нет паузы.
 * Сделать событие паузы для шины. Например, система звука ставит звуки на паузу
 *
 * Звуки для оружия игрока
 *
 * сделать только обработку смерти игрока
 * смена уровня слищком сложная
 *
 *
 * Конкретное оружие
 *
 *
 * доп враги
 *
 * система уровней
 *
 * сделать разблокировку оружия
 * сделаьт запоминание разблокированного оружия
 * сделать запоминание разблокированных достижений
 * сделать разные уровни
 *
 * Система смена уровня и система запуска игры по новой в случае смерти игрока
 * прощего всего почистит все сущности в мире и заново занести
 * сброс системы опыта
 * сброс времени в системе уровней
 * сброс оружий игрока
 *
 *
 * TODO: разделить Weapon Module на подмодули для каждого типа оружия
 *
 * Типы оружия:
 * 5. Простой щит
 * 6. Щит мести - наносит врагу такой же урон
 * 9. Щиты накладывающие разные
 *
 *
 * TODO: посмотреть какие достижения есть в vampire survivors
 *
 * */

/*

MODIFIERS:

moveFactor
animTimeFactor

attackSpeedFactor
detectRadiusFactor

bulletSpeedFactor

damageGiveFactor
damageTakeFactor

lootRadiusFactor
lootIncomeFactor
lootDropFactor

negativeEffectFactor
positiveEffectFactor

*/
