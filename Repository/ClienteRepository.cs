using AutoMapper;
using backend_API.Dto;
using backend_API.Models;
using backend_API.Models.Data;
using System.Data.Entity;

namespace backend_API.Services
{
    //INTERFAZ PARA DESACOPLAR EL ACCESO A DATOS DEL CONTROLADOR
    public interface IClienteRepository
    {
        public Task<IEnumerable<ClienteDto>> GetClientesListAsync();
        public ClienteDto GetCliente(int id);
        public Task<bool> CreateClienteAsync(ClienteDto clienteDto);
        public Task<bool> UpdateClienteAsync(ClienteDto clienteDto);
        public Task<bool> DeleteClienteAsync(ClienteDto clienteDto);
    }

    //CLASE REPOSITORIO CRUD DE LA ENTIDAD CLIENTE
    public class ClienteRepository : IClienteRepository
    { 
        protected readonly DBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteRepository> _logger;
        public ClienteRepository(DBContext dbContext, IMapper mapper, ILogger<ClienteRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        //OBTENER LA LISTA DE CLIENTES
        public async Task<IEnumerable<ClienteDto>> GetClientesListAsync() 
        {
            _logger.LogInformation("Obtener lista de clientes");
            var clientes = await _dbContext.Clientes.ToListAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

            return clientesDto;
        }

        //OBTENER UN CLIENTE POR SU ID
        public ClienteDto GetCliente(int id) 
        {
            var cliente = _dbContext.Clientes.FirstOrDefault(v => v.IdCliente == id);
            var clientesDto = _mapper.Map<ClienteDto>(cliente);
            return clientesDto;
        }

        //CREAR UN CLIENTE 
        public async Task<bool> CreateClienteAsync(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                var resultDB = await _dbContext.Clientes.AddAsync(cliente);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //ACTUALIZAR UN CLIENTE
        public async Task<bool> UpdateClienteAsync(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                _dbContext.Clientes.Update(cliente);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //BORRAR UN CLIENTE
        public async Task<bool> DeleteClienteAsync(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }

}
