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
 * PLAN
 *
 * вероятно нужен отдельный компонент который отвечает за destroy и возвращение ресурсов в пул
 *
 * 8. AI
 * 9. Exp and Money collect. Different enemies give different amount
 * 11. Добавить систему достижений через Event Bus. Например, убей 100 слизней и так далее
 *
 * 12. Звук для врагов
 * 13. Звук атаки игрока
 * 14. Звук сброра денег
 *
 * Сделать несколько уровней.
 *
 * */
