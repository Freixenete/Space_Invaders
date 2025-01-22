using System.Numerics;
using Heirloom;
using Vector = Heirloom.Vector;

namespace Space;

public class Enemigo
{
    public Image Imagen = new("Imagen/enemigo.png");
    private Rectangle posicion;
    private float velocidadX;
    public bool disparoActivo;
    public readonly Image disparoEnemigo = new ("Imagen/DisparoEnemigo.png");
    Random movimientoRandom = new Random();
    Random posicionRandom = new Random();
    Random dispaorRandom = new Random();
    private int temporizadorDisparo;


    public Enemigo(Image imagenAlien)
    {
        Imagen = imagenAlien;
        posicion = new Rectangle(posicionRandom.Next(maxValue:1080 - Imagen.Width), 
                              posicionRandom.Next(maxValue:75 - Imagen.Height), 
                              Imagen.Width, Imagen.Height);

        velocidadX = movimientoRandom.Next(1, 1) * (movimientoRandom.Next(0, 2) == 0 ? -1 : 1);
        disparoActivo = false;
        temporizadorDisparo = dispaorRandom.Next(50, 150);
    }
    
    public bool ColisionaConDisparo(DisparoNave disparo)
    {
        var rectanguloDisparo = new Rectangle(disparo.Posicion(), disparo.imagen.Size);
        return posicion.Overlaps(rectanguloDisparo);
    }

    public void Mover(Rectangle limites)
    {
        // Mueve el enemigo
        posicion.X += velocidadX;
        var posicoNova = posicion;
        posicoNova.X += velocidadX;

        // Cambia la dirección si toca los bordes
        if (!limites.Contains(posicoNova))
        {
            velocidadX *= -1; // Cambia la dirección
        }
        else
        {
            posicion = posicoNova;
        }
    }
    
    public bool ColisionaCon(Enemigo otro)//Para que si enemigos chocan cambien de direccion
    {
        var rectanguloActual = posicion;
        var rectanguloOtro = (otro.Posicion());
        return rectanguloActual.Overlaps(rectanguloOtro);
    }
    
    public void CambiarDireccion()
    {
        velocidadX *= -1; // Invierte la dirección
    }

    public Rectangle Posicion()
    {
        return posicion;
    }

    public void Pintar(GraphicsContext gfx)
    {
        gfx.DrawImage(Imagen, posicion);
    }

    public bool EnemigoOverlaps(Enemigo otroEnemigo) //Para que enemigos no aparezcan uno encima de otro al principio
    {
        var rectanguloEnemigo = posicion;
        var rectanguloOtroEnemigo = (otroEnemigo.Posicion());
        return rectanguloEnemigo.Overlaps(rectanguloOtroEnemigo);
    }
    
    

    public void DesactivarDisparo()
    {
        disparoActivo = false; // Desactivar el disparo
    }

    public DisparoEnemigo? IntentarDisparar()
    {
        // Reduce el temporizador en cada frame
        if (temporizadorDisparo > 0)
        {
            temporizadorDisparo--;
        }

        // Comprueba si es el momento de disparar
        if (temporizadorDisparo <= 0 && !disparoActivo)
        {
            var posicionDisparo = new Vector(posicion.X + Imagen.Width / 3, posicion.Y + Imagen.Height);
            disparoActivo = true; // Marca el disparo como activo
            temporizadorDisparo = dispaorRandom.Next(50, 150); // Reinicia el temporizador
            return new DisparoEnemigo(posicionDisparo, disparoEnemigo);
        }

        return null;
    }

}