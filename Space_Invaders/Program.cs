using Heirloom;
using Heirloom.Desktop;

namespace Space;

class Program
{
    const int EnemigosOnScreen = 5;
    private static Window _ventana = null!;
    private static Game _game = null!;
    
    static void Main(string[] args)
    {
        Application.Run(() =>
        {
            _ventana = new Window("La ventana");
            _ventana.BeginFullscreen();
            
            _game = new Game(_ventana, EnemigosOnScreen);
            _game.Inicializa();

            var loop = GameLoop.Create(_ventana.Graphics, OnUpdate, 140);
            loop.Start();
        });
    }
    
    private static void OnUpdate(GraphicsContext gfx, float dt)
    {
        _game.Mover();
        _game.Pintar();
        if (_game.vidas == 0)
        {
            _game.GameOver();
        }
        else if (_game.EnemigosPorOleada >= 7)
        {
            _game.GameWon();
        }
    }
}