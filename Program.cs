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
 *
 * Распаралеллить системы какие получится
 * постараться с системой физики
 *
 *
 * TODO: сделать в mod registry параметры которые отвечают за фактор негативных и позитивных эффектов, negativeScale, positiveScale
 * применяются к новым эффектам, обязательно перед их комбинированием со старыми
 * плюс соответствующие статус эффекты, LongPositive, ShortPositive, LongNegative, ShortNegative
 * эти эффекты не относятся ни к позитивным ни к негативным, поэтому фактор на них не действует
 *
 *
 * SWING-анимаций
 * SHIELD-анимации
 *
 * Пауза. Разделить системы на две части, одна работает независимо от паузы, другая только если нет паузы
 *
 * Система достижений через шину событий
 *
 * TODO: разделить Weapon Module на подмодули для каждого типа оружия
 *
 * Типы оружия:
 *
 *
 * посохи, карты и книги работают через bullet weapon
 * к пуле дополнительно прибавляем таймер
 * в конце таймера рассчитываем по кому пришелся урон
 *
 * 2. Посохи, выпускают фаерболы.
 * 3. Карты. Касты кругов
 * 4. Книги. Кастуют взрывы
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

negativeEffectFactor
positiveEffectFactor

*/
