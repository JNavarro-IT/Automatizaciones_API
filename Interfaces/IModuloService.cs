using backend_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Interfaces
{
    public interface IModuloService
    {
        Task<IActionResult> GetModulosList();
        Task<ActionResult<Modulo>> GetModulos(int id);
        Task<ActionResult<Modulo>> CreateModulo(Modulo modulo);
        Task<IActionResult> UpdateModulo(int id, Modulo modulo);
        Task<IActionResult> DeleteModulo(int id);
        public bool ModuloExists(int id);
    }
}
