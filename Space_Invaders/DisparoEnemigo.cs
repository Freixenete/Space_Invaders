using System.Numerics;
using Heirloom;
using Vector = Heirloom.Vector;

namespace Space;

public class DisparoEnemigo
{
    public readonly Image imagen = new ("Imagen/DisparoEnemigo.png");
    private Vector posicion;
    private readonly int velocidad;
    

    public DisparoEnemigo(Vector posicionInicial, Image imagenDisparo)
    {
        imagen = imagenDisparo;
        posicion = posicionInicial;
        velocidad = 5;
    }
    
    public void MoverDisparoEnemigo()//Se mueve el disparo
    {
        posicion.Y += velocidad;
    }
    
    public void Pintar(GraphicsContext gfx)
    {
        gfx.DrawImage(imagen, posicion);
    }

    public Vector Posicion()
    {
        return posicion;
    }

    public bool FueraDePantalla(Rectangle ventana)
    {
        return posicion.Y > ventana.Height; // El disparo está fuera si supera el límite inferior
    }

    public bool ColisionaConNave(Nave nave)
    {
        var hitboxDisparo = new Rectangle(posicion, imagen.Size);
        var hitboxNave = new Rectangle(nave.Posicion(), nave.imagen.Size);
        return hitboxDisparo.Overlaps(hitboxNave);
    }
}