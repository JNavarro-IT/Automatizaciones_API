using AutoMapper;
using backend_API.Dto;
using backend_API.Models;
using backend_API.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace backend_API.Services
{
    //INTERFAZ PARA DESACOPLAR EL ACCESO A DATOS DEL CONTROLADOR
    public interface IModuloRepository
    {
        public Task<IEnumerable<ModuloDto>> GetModulosListAsync();
        public ModuloDto GetModulo(int id);
        public Task<bool> DeleteModuloAsync(ModuloDto moduloDto);
        public Task<bool> UpdateModuloAsync(ModuloDto moduloDto);
        public Task<bool> CreateModuloAsync(ModuloDto moduloDto);
    }

    //REPOSITORIO CRUD DE LA ENTIDAD MODULO
    public class ModuloRepository : IModuloRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _mapper;

        //CONSTRUCTOR QUE OBTIENE LA BASE DE DATOS
        public ModuloRepository(DBContext db, IMapper mapper)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        //OBTENER LA LISTA DE MÓDULOS
        public async Task<IEnumerable<ModuloDto>> GetModulosListAsync()
        {
            try
            {
                var modulos = await _dbContext.Modulos.ToListAsync();
                var modulosDto = _mapper.Map<IEnumerable<ModuloDto>>(modulos);
                return modulosDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Enumerable.Empty<ModuloDto>();
            }
       
        }

        //OBTENER UN MODULO POR SU ID
        public ModuloDto? GetModulo(int id)
        {
            try
            {
                var modulo = _dbContext.Modulos.FirstOrDefault(m
               => m.Id == id);
                var moduloDto = _mapper.Map<ModuloDto>(modulo);
                return moduloDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
           
        }

        //CREAR UN MODULO 
        public async Task<bool> CreateModuloAsync(ModuloDto moduloDto)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(moduloDto);
                var resultDB = await _dbContext.Modulos.AddAsync(modulo);
                await _dbContext.SaveChangesAsync();
                return true;
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }        
        }

        //ACTUALIZAR UN MODULO
        public async Task<bool> UpdateModuloAsync(ModuloDto moduloDto)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(moduloDto);
                _dbContext.Modulos.Update(modulo);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }

        //BORRAR UN MODULO
        public async Task<bool> DeleteModuloAsync(ModuloDto moduloDto)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(moduloDto);
                _dbContext.Modulos.Remove(modulo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
