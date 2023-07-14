using backend_API.Dto;
using backend_API.Models;
using backend_API.Models.Data;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace backend_API.Services
{
    public interface IUbicacionService
    {

        Task<Ubicacion> GetUbicacionById(int idUbicacion);
        Task<Ubicacion> InsertUbicacionAsync(Ubicacion ubicacion);
    }

    public class UbicacionRepository : IUbicacionService
    {
        private readonly DBContext _dbContext;
        private readonly ICoordinateTransformation _latLonToUtmTransform;
        private readonly ICoordinateTransformation _utmToXYTransform;

        public UbicacionRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            var latLonSystem = GeographicCoordinateSystem.WGS84;
            var utmSystem = ProjectedCoordinateSystem.WGS84_UTM(30, true);
            var xySystem = ProjectedCoordinateSystem.WebMercator;
            var factory = new CoordinateTransformationFactory();
            _latLonToUtmTransform = factory.CreateFromCoordinateSystems(latLonSystem, utmSystem);
            _utmToXYTransform = factory.CreateFromCoordinateSystems(utmSystem, xySystem);
        }

        public async Task<Ubicacion> GetUbicacionById(int idUbicacion)
        {
            var ubicacion = await _dbContext.Ubicaciones.FindAsync(idUbicacion);
            if (ubicacion == null)
            {
            }
            return ubicacion;
        }

        public async Task<Ubicacion> InsertUbicacionAsync(UbicacionDto ubicacionDto)
        {
            double longitud = ubicacionDto.Longitud;
            double latitud = ubicacionDto.Latitud;
            double[] utm30 = _latLonToUtmTransform.MathTransform.Transform(new[] { longitud, latitud });
            double[] xy = _utmToXYTransform.MathTransform.Transform(new[] { longitud, latitud });

            Ubicacion ubicacion = new()
            {
                Ref_catastral = ubicacionDto.Ref_catastral,
                Superficie = ubicacionDto.Superficie,
                CoordX_UTM = utm30[0],
                CoordY_UTM = utm30[1],
                Latitud = latitud,
                Longitud = longitud,
                Inclinacion = ubicacionDto.Inclinacion,
                Azimut = ubicacionDto.Azimut
            };

            await _dbContext.Ubicaciones.AddAsync(ubicacion);
            await _dbContext.SaveChangesAsync();

            return ubicacion;
        }

        public Task<Ubicacion> InsertUbicacionAsync(Ubicacion ubicacion)
        {
            throw new NotImplementedException();
        }
    }


    
}

