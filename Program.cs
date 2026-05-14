using Raylib_cs;

class Program
{
    public static void Main(string[] args)
    {
        Raylib.InitWindow(1280, 720, "Survivors");
        Raylib.InitAudioDevice();

        using Game game = new Game();

        Font font = Raylib.GetFontDefault();
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            game.Update();

            Raylib.DrawFPS(20, 20);

            Raylib.EndDrawing();
        }

        game.Dispose();
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}

// TODO: нужна логичная система уничтожения объектов
// Если через таймер, то когда время 0.0, добавляем DeathComp с isDead = true
// в LocalTrs добавляем это ко всем дочерним объектам вместо их прямого уничтожения
// тогда каждый объект сам отвечает за свое уничтожение
// тогда систему timer sys может быть прямо перед death sys

/*
 *
 * SWING-анимаций
 * SHIELD-анимации
 *
 * Пауза. Разделить системы на две части, одна работает независимо от паузы, другая только если нет паузы
 *
 * Система достижений через шину событий
 *
 *
 * TODO: добавить в щиты параметр радиуса
 *
 *
 * TODO: разделить Weapon Module на подмодули для каждого типа оружия
 *
 * Типы оружия:
 *
 *
 * 2. Посохи, выпускают фаерболы.
 * 3. Карты. Касты кругов
 * 5. Простой щит
 * 6. Щит мести - наносит врагу такой же урон
 * 7. Щит отражения - отражает стрелы назад во врагов
 * 8. Щит уничтожающий стрелы.
 * 9. Щит замедления. накладывает на всех в радиусе эффект замедления
 * 10. Каст с отталкивающим взрывом
 * 11. Каст с притягивающим взрывом
 *
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

*/
