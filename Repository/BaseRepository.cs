using AutoMapper;
using backend_API.Models.Data;
using backend_API.Utilities;
using Microsoft.EntityFrameworkCore;
namespace backend_API.Repository
{
    //INTERFAZ PRINCIPAL QUE IMPLEMENTA UN CRUD BASICO A LOS CONTROLADORES
    public interface IBaseRepository<T, TDto>
        where T : ModelBase
        where TDto : DtoBase
    {
        public Task<IEnumerable<TDto>> GetEntitiesListAsync();
        public Task<TDto> GetEntity(object identity);
        public Task<bool> CreateEntityAsync(TDto tDto);
        public Task<int> UpdateEntityAsync(TDto entityDto);
        public Task<int> DeleteEntityAsync(TDto entityDto);
        public Task<bool> EntityExists(TDto entityDto);

    }

    //CLASE ENCARGADA DE HACER EL CRUD A LA BASE DE DATOS DE FORMA GENÉRICA
    public class BaseRepository<T, TDto> : IBaseRepository<T, TDto>
        where T : ModelBase
        where TDto : DtoBase
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _mapper;

        //CONSTRUCTOR PARA LA INYECCION DE DEPENDENCIAS DE DBCONTEXT Y AUTOMAPPER
        public BaseRepository(DBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //OBTENER LA LISTA DE LA ENTIDAD
        public async Task<IEnumerable<TDto>> GetEntitiesListAsync()
        {
            var entities = await _dbContext.Set<T>().ToListAsync();
            var entitiesDto = _mapper.Map<IEnumerable<TDto>>(entities);

            return entitiesDto;
        }

        //OBTENER UNA ENTIDAD POR SU ID
        public async Task<TDto> GetEntity(object identity)
        {
            var entity = await _dbContext.Set<T>().FindAsync(identity);
            var entityDto = _mapper.Map<TDto>(entity);
            return entityDto;
        }

        //CREAR UNA ENTIDAD 
        public async Task<bool> CreateEntityAsync(TDto entityDto)
        {
            try
            {
                var entity = _mapper.Map<T>(entityDto);
                var resultDB = await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //ACTUALIZAR UNA ENTIDAD
        public async Task<int> UpdateEntityAsync(TDto entityDto)
        {
            try
            {
                var entity = _mapper.Map<T>(entityDto);
                _dbContext.Set<T>().Update(entity);
                var updated = await _dbContext.SaveChangesAsync();
                return updated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        //BORRAR UNA ENTIDAD
        public async Task<int> DeleteEntityAsync(TDto entityDto)
        {
            try
            {
                var entity = _mapper.Map<T>(entityDto);
                _dbContext.Set<T>().Remove(entity);
                var deleted = await _dbContext.SaveChangesAsync();
                return deleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public async Task<bool> EntityExists(TDto entityDto)
        {
            var entity = _mapper.Map<T>(entityDto);
            var entitiesList = await _dbContext.Set<T>().ToListAsync();
            var exist = entitiesList.Contains(entity);
            return exist;
        }
    }
}