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

// TODO: tile collision
/*
 *      делать то на что хватает ума, лучше хоть что-то сделать. все что хочется сделать не получится, пока не хватает знаний
 *
 *
 *      сделать реф-структуру для итерации по лучу, итерируем по стандартным клеткам. если нет чанка, то пытается его перепрыгнуть
 *      затем определяет координаты луча в чанке и итерирует по ним
 *      используется в физике объектов
 *      затем в физике тайдлвлй карты
 *      сначала итерируем по чанкам
 *      затем если чанк есть, запускаем отдельный итератор внутри чанка
 *      так проще будет
 *
 *
 *      SWING-анимаций
 *      SHIELD-анимации
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
