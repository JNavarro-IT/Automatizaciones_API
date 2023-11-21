using AutoMapper;
using Automatizaciones_API.Context;
using Automatizaciones_API.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Automatizaciones_API.Repository
{
   //INTERFAZ PRINCIPAL QUE IMPLEMENTA UN CRUD BASICO A LOS CONTROLADORES
   public interface IBaseRepository<T, TDto>
       where T : ModelBase
       where TDto : DtoBase
   {
      public Task<IEnumerable<TDto>> GetEntitiesList();
      public IQueryable<TDto> GetEntitiesInclude(
         Expression<Func<T, bool>>? filter = null,
         Func<IQueryable<T>,
         IIncludableQueryable<T, object>>? includes = null);
      public Task<TDto?> GetEntityDto(object? identity);
      public Task<TDto> CreateEntity(TDto tDto);
      public Task<int> UpdateEntity(TDto entityDto);
      public Task<int> DeleteEntity(TDto entityDto);
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
      public async Task<IEnumerable<TDto>> GetEntitiesList()
      {
         List<T> entities = await _dbContext.Set<T>().ToListAsync();
         return _mapper.Map<IEnumerable<TDto>>(entities);
      }

      public IQueryable<TDto>GetEntitiesInclude(Expression<Func<T, bool>>? filter = null,
      Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
      {
         IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

         if (filter != null)
            query = query.Where(filter);

         if (includes != null)
            query = includes(query);

         return _mapper.ProjectTo<TDto>(query);
      }


      //OBTENER UNA ENTIDAD POR SU ID
      public async Task<TDto?> GetEntityDto(object? identity)
      {
         if (identity == null)
            return null;

         T? entity = await _dbContext.Set<T>().FindAsync(identity);
         return entity == null ? null : _mapper.Map<TDto>(entity);
      }

      public async Task<T?> GetEntity(object identity)
      {
         if (identity == null)
            return null;

         T? entity = await _dbContext.Set<T>().FindAsync(identity);
         return entity ?? null;
      }
      //CREAR UNA ENTIDAD 
      public async Task<TDto> CreateEntity(TDto tDto)
      {
         try
         {
            var entity = _mapper.Map<T>(tDto);
            EntityEntry<T> resultEntry = await _dbContext.Set<T>().AddAsync(entity);
            _ = await _dbContext.SaveChangesAsync();
            var newEntity = resultEntry.Entity;
            return _mapper.Map<TDto>(newEntity);
         }
         catch (Exception ex)
         {
            Console.Error.WriteLine("ERROR => al crear la entidad: " + ex);
            throw new InvalidOperationException();
         }
      }

      //ACTUALIZAR UNA ENTIDAD
      public async Task<int> UpdateEntity(TDto entityDto)
      {
         try
         {
            T entity = _mapper.Map<T>(entityDto);
            _ = _dbContext.Set<T>().Update(entity);
            int files = await _dbContext.SaveChangesAsync();
            return await _dbContext.SaveChangesAsync();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex);
            return -1;
         }
      }

      // BORRAR UNA ENTIDAD
      public async Task<int> DeleteEntity(TDto entityDto)
      {
         try
         {
            T entity = _mapper.Map<T>(entityDto);
            _ = _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex);
            return 0;
         }
      }

      // CHECKEAR SI EXISTE UNA ENTIDAD AL COMPLETO EN LA BASE DE DATOS INDEPENDIENTEMENTE DE SU ID
      public async Task<bool> EntityExists(TDto? entityDto)
      {
         if (entityDto is null) return false;

         var dtoProperties = typeof(TDto).GetProperties();
         if (dtoProperties.Length == 0) return false;

         foreach (var dtoProperty in dtoProperties)
         {
            if (dtoProperty.Name.StartsWith("Id"))
            {
               var id = dtoProperty.GetValue(entityDto);
               if (id is null) return false;
               var findDto = await GetEntityDto(id);
               if (findDto is null) return false;
            }
         }
         return true;
      }
   }
}