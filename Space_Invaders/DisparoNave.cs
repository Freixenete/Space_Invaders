using System.Numerics;
using Heirloom;
using Vector = Heirloom.Vector;

namespace Space;

public class DisparoNave
{
    public readonly Image imagen = new ("Imagen/DisparoNave.png");
    private Vector posicion;
    private readonly int velocidad;
    

    public DisparoNave(Vector posicionInicial, Image imagenDisparo)
    {
        imagen = imagenDisparo;
        posicion = posicionInicial;
        velocidad = 8;
    }

    public void MoverDisparo()//Se mueve el disparo
    {
        posicion.Y -= velocidad;
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
        return posicion.Y + imagen.Height < 0;
    }
}