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

            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                game.isPaused = !game.isPaused;

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
 * Звуки для оружия игрока
 *
 * сделать только обработку смерти игрока
 * смена уровня слищком сложная
 *
 *
 * Конкретное оружие
 *
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
