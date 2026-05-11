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
 * Сделать
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
 * Общий компонент с модификаторами.
 *
 * Внести модификаторы в оружие
 *
 * Типы оружия:
 *
 * Ближнее.
 *
 * Стрелы.
 * Стрелы сквозные.
 * Стрелы сквозные с отражением от стен.
 *
 * Посохи. Фаерболы.
 *
 * Карты. Касты кругов
 *
 * Лазер до первого врага
 * Лазер сквозной
 * Лазер сквозной с отражением
 *
 * Бумеранги
 *
 * Вращающееся вокруг игрока.
 *
 * */
