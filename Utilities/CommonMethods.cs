   using System.Globalization;
   using System.Text;

namespace backend_API.Utilities
{
   // INTERFAZ PARA MÉTODOS GENÉRICOS QUE USAN OTRAS CLASES, MEDIANTE INYECCION DE DEPENDENCIAS
   public interface ICommonMethods
   {
      public StringBuilder WithoutTildes(string item);

   }

   // CLASE DA SERVICIO PARA PODER OBTENER MÉTODOS REUTILIZABLES POR CUALQUIER ENTIDAD
   public class CommonMethods : ICommonMethods
   {

      // CONSTRUCTOR POR DEFECTO
      public CommonMethods() { }

      // OBTENER UNA CADENA DE TEXTO SIN TILDES
      public StringBuilder WithoutTildes(string item)
      {
         StringBuilder sb = new();
         var format = item.Normalize(NormalizationForm.FormD);
         foreach (char f in format)
            if (CharUnicodeInfo.GetUnicodeCategory(f) != UnicodeCategory.NonSpacingMark)
               sb.Append(f);

         return sb;
      }
   
   }
}

