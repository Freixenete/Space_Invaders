using System.Numerics;
using Heirloom;
using Vector = Heirloom.Vector;

namespace Space;
public class Nave
{
    public Image imagen = new ("Imagen/Nave.png");
    private Vector posicion;
    private readonly int velocidad;
    public readonly Image disparoNave = new ("Imagen/DisparoNave.png");
    private bool disparoActivo; // Campo bool para indicar si el disparo esta activo

    public Nave(int x, int y, Image imagenNave)
    {
        imagen = imagenNave;
        velocidad = 6;
        posicion = new Vector(x, y);
        disparoActivo = false; // Inicializa el disparo como inactivo
    }

    public DisparoNave? Disparar()
    {
        if (!disparoActivo)
        {
            var posicionDisparo = new Vector(posicion.X + imagen.Width / 3, posicion.Y - imagen.Height / 2);
            disparoActivo = true; // Marca el disparo como activo al disparar
            return new DisparoNave(posicionDisparo, imagenDisparo: disparoNave);
        }
        return null;
    }

    public void DesactivarDisparo()
    {
        disparoActivo = false; // Desactivar el disparo
    }

    public void Pintar(GraphicsContext gfx)
    {
        gfx.DrawImage(imagen, posicion);
    }

    public void Mover(Rectangle ventana)
    {
        var nuevaPosicion = new Rectangle(posicion, imagen.Size);
        
        if (Input.CheckKey(Key.A, ButtonState.Down))
        {
            nuevaPosicion.X -= velocidad;
        }
        
        if (Input.CheckKey(Key.D, ButtonState.Down))
        {
            nuevaPosicion.X += velocidad;
        }
        if (ventana.Contains(nuevaPosicion))
        { 
            posicion.X = nuevaPosicion.X; 
            posicion.Y = nuevaPosicion.Y;
        }
    }

    public DisparoNave? IntentarDisparar()
    {
        if (Input.CheckKey(Key.W, ButtonState.Pressed))//Poner "ButtonState.Pressed" para que se ejecute una accion
                                                       //por vez que apretas tecla
        {
            return Disparar();
        }

        return null;
    }

    public Vector Posicion()
    {
        return posicion;
    }
}
