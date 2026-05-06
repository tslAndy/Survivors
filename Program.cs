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

// можно сделать entity context
// он содержит модификации скорости, активен ли entity, скорость анимаций и скорость оружия
// тогда вместо нескольких можификаторов времени нужен только один
// применять и для врагов и для игрока
// модификаторы удачи туда же и урона

/*
 *      SWING-анимаций
 *      SHIELD-анимации
 *      Дополнительные статус эффекты.
 *      Поиск игрока по его entity
 *      Враги обходят стены
 *
 *      Пауза
 *      Система достижений через шину событий
 *
 *
 *      Типы оружия, кроме стандартных
 *
 *      Стрела прошивает всех врагов насквозь
 *      Стрела отскакивает от стен
 *      Лазер через рейкаст
 *      Мечи вращаются вокруг игрока
 *      Щит отражающий стрелы во врагов
 *
 * */
