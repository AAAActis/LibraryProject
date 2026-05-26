using System.Linq;
using Libreria1.Application;

namespace Libreria1.Domain.Menus
{
    public abstract class MenuBase : IMenu
    {
        protected readonly IPresenter Presenter;

        protected MenuBase(IPresenter presenter)
        {
            Presenter = presenter;
        }

        public abstract void Ejecutar();

        protected void MostrarTitulo(string texto)
        {
            Presenter.MostrarTitulo(texto);
        }

        protected void MostrarMensaje(string texto)
        {
            Presenter.MostrarMensaje(texto);
        }

        protected void EsperarContinuar()
        {
            Console.WriteLine();
            Console.WriteLine("Presione Enter para continuar...");
            Console.ReadLine();
        }

        protected bool ValidarIsbn13(string? isbn, out string isbnVal)
        {
            isbnVal = (isbn ?? string.Empty).Trim();
            return isbnVal.Length == 13 && isbnVal.All(char.IsDigit);
        }
    }
}
