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


/*
 * делать то на что хватает ума, лучше хоть что-то сделать. все что хочется сделать не получится, пока не хватает знаний
 *
 *
 *
 * Сделать в WeaponConfig  параметр attackSound, если не нулл то проигрывать
 *
 * SWING-анимаций
 * SHIELD-анимации
 * Враги обходят стены
 *
 * Пауза. Разделить системы на две части, одна работает независимо от паузы, другая только если нет паузы
 *
 * Система достижений через шину событий
 *
 *
 *
 * Типы оружия:
 *
 *
 * 1. Пуля с рейкастом. проще всего сделать наоборот. отправляем луч в обратном направлении от движения пули и пока его длина не ноль ищем отражения
 * 2. Посохи, выпускают фаерболы.
 * 3. Карты. Касты кругов
 * 4. Бумеранги
 * 5. Простой щит
 * 6. Щит мести - наносит врагу такой же урон
 * 7. Щит отражения - отражает стрелы назад во врагов
 * 8. Щит уничтожающий стрелы.
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
