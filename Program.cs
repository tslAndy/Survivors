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

// TODO: weapons and shield to interface

/*
 *      делать то на что хватает ума, лучше хоть что-то сделать. все что хочется сделать не получится, пока не хватает знаний
 *
 *
 *      SWING-анимаций
 *      SHIELD-анимации
 *      Поиск игрока по его entity
 *      Враги обходят стены
 *
 *      Пауза
 *      Система достижений через шину событий
 *
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
