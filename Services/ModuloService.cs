using backend_API.Dto;
using backend_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Services
{
    public class ModuloService
    {
        private readonly IModuloService _moduloService;
        
        public ModuloService(IModuloService moduloService)
        {
            _moduloService = moduloService;
        }

 

        // GET: Modulo/modulosList
        [HttpGet("modulosList")]
        public IEnumerable<ModuloDto> GetModulosList()
        {
            //var usersList = await _context.Users.ToListAsync();
            return new List<ModuloDto>
            {
                new ModuloDto(nombre: "JAM66S30 500MR", potencia:500, icc:13.04),
                new ModuloDto(nombre: "PS460M5GFH-24/TSH", potencia:460, icc:10.94)
            };
        }

    }
}
