using System.Runtime.CompilerServices;
using Heirloom;
using Heirloom.Desktop;
using Space;

public class Game
{
    //private string _fase = "playing";
    public int EnemigosPorOleada = 5;
    private static Window _ventana = null!;
    private static Window _ventanaWin = null!;
    private static Window _ventanaLose = null!;
    private static Nave _nave = null!;
    private static List<DisparoNave> _disparosNave = new();
    private static List<Enemigo> _enemigos = new();
    public int vidas = 4;
    private static Image fondo;
    private static Image fondoWin;
    private static Image fondoLose;
   // private static Image enemigo;
    private static List<DisparoEnemigo> _disparosEnemigos = new();
    private static int temporizadorGlobal = 2; // Contador para 2 segundos
    private static Random random = new Random();
    private static Image enemigo = new ("Imagen/enemigo.png");
    public Game(Window ventana, int enemigosOnScreen)
    {
        _ventana = ventana;
        fondo = new Image("Imagen/fondoRojo.jpg");
        fondoWin = new Image("Imagen/win.png");
        fondoLose = new Image("Imagen/lose.png");
        //enemigo = new Image("Imagen/enemigo.png");
        
        //Graphics.DrawText
    }

    public void Inicializa()
    {
        _nave = new Nave(900, 760, new Image("Imagen/Nave.png"));

        _enemigos.Clear();
        AgregarEnemigo(EnemigosPorOleada);
        //for (int i = 0; i < EnemigosPorOleada; i++)
        //{
        //    AgregarEnemigo();
        //}
    }

    private void AgregarEnemigo(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            Enemigo nuevoEnemigo;
            bool posicionValida;

            do
            {
                nuevoEnemigo = new Enemigo(enemigo);
                posicionValida = true;

                foreach (var enemigoExistente in _enemigos)
                {
                    if (nuevoEnemigo.EnemigoOverlaps(enemigoExistente))
                    {
                        posicionValida = false;
                        break;
                    }
                }
            } while (!posicionValida);

            _enemigos.Add(nuevoEnemigo);

        }
    }


    public void Mover()
    {


        var rectanguloVentana = new Rectangle(0, 0, _ventana.Width, _ventana.Height);

        _nave.Mover(rectanguloVentana);

        foreach (var enemigo in _enemigos)
        {
            enemigo.Mover(rectanguloVentana);

            foreach (var otroEnemigo in _enemigos)
            {
                if (enemigo != otroEnemigo && enemigo.ColisionaCon(otroEnemigo))
                {
                    enemigo.CambiarDireccion();
                    otroEnemigo.CambiarDireccion();
                }
            }

            enemigo.Pintar(_ventana.Graphics);
        }

        // Temporizador global para disparo cada 2 segundos
        temporizadorGlobal--;
        if (temporizadorGlobal <= 0)
        {
            // Selecciona tres enemigos de manera aleatoria
            for (int k = 0; k < 3; k++)
            {
                if (_enemigos.Count > 0)
                {
                    var enemigo = _enemigos[random.Next(_enemigos.Count)];
                    var nuevoDisparo = enemigo.IntentarDisparar();
                    if (nuevoDisparo != null)
                    {
                        _disparosEnemigos.Add(nuevoDisparo);
                    }
                }
            }

            temporizadorGlobal = 2; // Reinicia el temporizador (2 segundos)
        }

        // Mueve los disparos enemigos y los elimina si están fuera de pantalla
        for (int i = _disparosEnemigos.Count - 1; i >= 0; i--)
        {
            var disparo = _disparosEnemigos[i];
            disparo.MoverDisparoEnemigo();

            if (disparo.ColisionaConNave(_nave))
            {
                vidas--;
                _disparosEnemigos.RemoveAt(i);
            }
            else if (disparo.FueraDePantalla(rectanguloVentana))
            {
                _disparosEnemigos.RemoveAt(i);
                foreach (var enemigo in _enemigos)
                {
                    if (enemigo.disparoActivo)
                    {
                        enemigo.DesactivarDisparo();
                    }
                }
            }
            else
            {
                disparo.Pintar(_ventana.Graphics);
            }
        }

        if (_enemigos.Count == 0)
        {
            if (EnemigosPorOleada < 7)
            {
                EnemigosPorOleada++;
                _enemigos.Clear();
                //Inicializa();
                AgregarEnemigo(EnemigosPorOleada);
            }

        }
    }

    public void Pintar()
    {
        var rectanguloVentana = new Rectangle(0, 0, _ventana.Width, _ventana.Height);

        _ventana.Graphics.Clear(Color.Black);
        _ventana.Graphics.DrawImage(fondo, rectanguloVentana);

        _nave.Pintar(_ventana.Graphics);

        var nuevoDisparo = _nave.IntentarDisparar();
        if (nuevoDisparo != null)
        {
            _disparosNave.Add(nuevoDisparo);
        }

        for (int i = _disparosNave.Count - 1; i >= 0; i--)
        {
            var disparo = _disparosNave[i];
            disparo.MoverDisparo();

            if (disparo.FueraDePantalla(rectanguloVentana))
            {
                _disparosNave.RemoveAt(i);
                _nave.DesactivarDisparo(); // Desactiva el disparo cuando sale de la pantalla
            }
            else
            {
                disparo.Pintar(_ventana.Graphics);
            }

            for (int j = _enemigos.Count - 1; j >= 0; j--)
            {
                var enemigo = _enemigos[j];
                if (enemigo.ColisionaConDisparo(disparo))
                {
                    _enemigos.RemoveAt(j); // Elimina el enemigo alcanzado
                    _disparosNave.RemoveAt(i); // Elimina el disparo que lo alcanzó
                    _nave.DesactivarDisparo(); // Desactiva el disparo
                    break;
                }
            }
        }

        foreach (var enemigo in _enemigos)
        {
            enemigo.Pintar(_ventana.Graphics);
        }

        foreach (var disparo in _disparosEnemigos)
        {
            disparo.MoverDisparoEnemigo(); // Este método actualiza la posición internamente
            disparo.Pintar(_ventana.Graphics); // Dibuja el disparo en la nueva posición
        }

        DibujarVidas(_ventana.Graphics);
    }

    public bool GameOver()
    {
        var rectanguloVentana = new Rectangle(0, 0, _ventana.Width, _ventana.Height);

        if (vidas == 0)
        {
            _ventana.Graphics.Clear(Color.Black);
            _ventana.Graphics.DrawImage(fondoLose, rectanguloVentana);
        }

        return false;
    }

    public void GameWon()
    {

        var rectanguloVentana = new Rectangle(0, 0, _ventana.Width, _ventana.Height);
        if (EnemigosPorOleada >= 6)
        {
            _enemigos.Clear();
            _ventana.Graphics.Clear(Color.Black);
            _ventana.Graphics.DrawImage(fondoWin, rectanguloVentana);
        }

        //return false;
    }
    
private void DibujarVidas(GraphicsContext gfx)
{
    Vector text = default;
    var pos = new Vector(_ventana.Width - 150, _ventana.Height - 50);
    gfx.DrawText($"Vidas: {vidas}", pos, Font.Default, 48);
    
    //Graphics.DrawText(cavas, text, x, y, options);
        //var textoVidas = $"Vidas: {vidas}";
        //var posicionVidas = new Vector(_ventana.Width - 150, _ventana.Height - 50);
        //var estiloFuente = Font.Default;
        //gfx.DrawText(textoVidas, pos, estiloFuente);
    }
}