# AutoAPI Documentación de AutoAPI

## Descripción
Esta documentación proporciona una descripción general de la estructura y los componentes de la aplicación AutoAPI, que utiliza ASP.NET Core y Entity Framework Core para administrar una base de datos y exponer una API REST.

Es esencial personalizar y ampliar esta documentación según sea necesario para su proyecto específico, incluyendo detalles sobre rutas, puntos finales y otra información relevante.

## Estructura del Proyecto
>- **AutoAPI**: Carpeta principal del proyecto.
>- **Properties**: Carpeta que para publicar y compilar el proyecto en Visual Studio
>- **Context**: Carpeta con los tipos de contexto que pueden tener las bases de datos relacionadas con la aplicación. Definen el contexto y las relaciones entre las tablas. Utiliza Entity Framework Core para facilitar el acceso y la manipulación de la Base de Datos.
>- **Controllers**: La aplicación tiene un controlador genérico para gestionar las solicitudes HTTP y para administrar entidades de la base de datos. Estos controladores siguen una interfaz genérica IBaseController y proporcionan operaciones CRUD comunes.
>- **Models**: Define los modelos de datos utilizados en la aplicación.
>   - **DTOs**: Define objetos de transferencia de datos utilizados para comunicación entre la API y el frontend.
>- **Repository**:  Clase que representa un C.R.U.D (Create, Read, Update, Delete) genérico para realizar operaciones básicas en una base de datos utilizando Entity Framework Core junto con cualquier modelo y su DTO. Uso de Automapper para reasignación automática de atributos
>- **Services**: Contiene servicios para realizar tareas específicas.
>- **Utilities**: Carpeta con diferentes recursos para la aplicación
>   + **Resources**: Carpeta para archivos de lectura sobre información estática
>   + **SeedData**: Carpeta con archivos SQL para inicializar la estructura de la Base de Datos
>   + **_CommonMethods_**: Archivo con métodos genéricos y de utilidad
>   + **_MappingProfile_**: Archivo que contiene la configuración de la herramienta AutoMapper para asignar Modelos y DTOs. Los ensambla en ambas direcciones con el uso de dos clases ficticias que los arquetipan: *ModelBase* y *DtoBase*

## Clase DBContext
La clase DBContext se encarga de definir el contexto de la base de datos y las relaciones entre las tablas. Utiliza Entity Framework Core para facilitar el acceso y la manipulación de la base de datos.

> - **Clientes**: Propiedad que representa una tabla de clientes en la base de datos.
> - **OnModelCreating(ModelBuilder modelBuilder)**: Al sobreescribir este método se hace uso de la herramienta API Fluent de EFCore que permite definir las características de los atributos de cada entidad, en lugar de usar las anotaciones de clase. Normalmente, se usa para describir relaciones complejas entre entidades (Ej. N:M), u otros tipos de restricciones

## BaseController
>- **GetListAsync()**: Obtiene una lista de entidades.
>- **GetByIdentity(object identity)**: Obtiene una entidad por su identificador.
>- **CreateEntity(object identity, [FromBody] TDto dto)**: Crea una nueva entidad.
>- **UpdateEntity(object identity, [FromBody] TDto dto)**: Actualiza una entidad existente.
>- **DeleteEntity(object identity, [FromBody] TDto dto)**: Elimina una entidad.
>- **UpdatePartialEntity(object identity, JsonPatchDocument<TDto> patchDto)**: Actualiza parcialmente una entidad.

## CustomController
El controlador personalizado CustomController proporciona funcionalidades adicionales, (*Ej. Obtención de datos meteorológicos y cálculo de edades*). Se inyecta la dependencia del repositorio propio de la entidad para acceder e interactuar con la tabla correspondiente al modelo que se le pase por parámetro tipo. También se comunica con los servicios personalizados.

Inyecta la interfaz ILogger para poder generar un registro de los eventos de la entidad, se puede configurar para que salgan por consola, se copien en un archivo o se registren en la Base de Datos
 
>- **GetTiempo()**: Obtiene datos meteorológicos y los devuelve como respuesta.
>- **GetEdad(ClienteDto? Cliente)**: Calcula la edad de un cliente y la devuelve como respuesta.

## Cliente
La clase Cliente representa a un cliente en un proyecto y se mapea a la tabla "Clientes" en la base de datos. Incluye varias anotaciones de datos para especificar restricciones de la base de datos.

>- **IdCliente**: El identificador único para un cliente.
>- **Nombre**: El nombre del cliente.
>- **Dni**: El número de identificación del cliente.
>- **Telefono**: El número de teléfono del cliente.
>- **Email**: La dirección de correo electrónico del cliente (nullable).

>### ClienteDto
>> La clase ClienteDto es un ejemplo de Objeto de Transferencia de Datos (DTO) utilizado para transportar información sobre la entidad Cliente. Incluye propiedades que coinciden con las de la clase Cliente y pueden contener información adicional.

## IBaseRepository y BaseRepository
La interfaz IProjectService define métodos para interactuar con la Base de Datos

La clase BaseRepository es un repositorio genérico de CRUD (Crear, Leer, Actualizar, Eliminar) para entidades registradas, a través de Entity Framework Core. Interactúa con la Base de Datos y realiza la asignación automática entre objetos modelo y DTO con el uso de AutoMapper

>- **GetEntitiesList()**: Recupera una lista de entidades de la base de datos y las asigna a DTOs.
>- **GetEntityDto(object? identity)**: Recupera una sola entidad por uN identificador único y la asigna a un DTO.
>- **CreateEntity(TDto tDto)**: Inserta una nueva entidad en la base de datos y en la tabla que corresponde a su modelo.
>- **UpdateEntity(TDto entityDto)**: Actualiza una entidad por completo de la base de datos a través de su identificador
>- **DeleteEntity(TDto entityDto)**: Elimina una entidad de la Base de Datos a través de su identificador
>- **EntityExists(TDto entityDto)**: Comprueba si existe una entidad en la Base de Datos sin tener en cuenta el identificador

## IProjectService y ProjectService
La interfaz IProjectService define métodos que ofrecen servicios para el proyecto. Ej. *Obtener la edad de un cliente o el acceso a datos de fuentes externas.*

La clase ProjectService implementa la interfaz *IProjectService* y utiliza la interfaz *ICommonMethods* para obtener utilidades y *IBaseRepository* para acceder a los datos de los clientes.

>- **GetCIF(string Empresa)**: Recupera el CIF (Número de Identificación Fiscal) de una empresa desde un archivo JSON externo.
>- **GetAge(ClienteDto Cliente)**: Calcula edad de un cliente a través de su DNI (simulado).
>- **CheckMunicipio(string municipio, string provincia)**: Comprueba si un municipio está recogido entre las localidades especiales recgidas en un JSON local.

## ICommonMethods y CommonMethods
La interfaz ICommonMethods define métodos utilizables por cualquier entidad o controlador (reutilización de código).

La clase CoomonMethoods implementa la interfaz *ICommonMethods* y desarrolla métodos que sirven como herramientas o utilidades para la aplicación.
>- **WithoutTildes(string item)**: Normalizar una cadena de texto en otra, eliminando los simbolos de acentuación y derivados.
>- **GetWeather()**: Proporciona información meteorológica a tiempo real (simulado)

## MappingProfile
Configuración de AutoMapper, biblioteca externa utilizada para asignar dos modelos de datos diferentes, en este caso entre un Modelo y su Dto. 

Este enfoque elimina la necesidad de configurar manualmente cada asignación.

>- **Clases Dummy**: Las clases ModelBase y DtoBase sirven como clases ficticias para agrupar y ensamblar modelos y DTOs juntos. Se utilizan para simplificar la configuración.
>- **AutoMapperConfig**: Clase que configura e inserta un nuevo perfil en la clase AutoMapper para que implemente la interfaz *IMapper* a través del método *Initialize()*
>- **MappingProfile**: Clase que implementa *Profile*, que es otra clase de AutoMapper. Para ello, se utiliza la reflexión y se van asignando los atributos de clase modelo a su correpondiente clase de DTO y viceversa. Los identifica por la jerarquía de herencia de ModelBase y DtoBase.

## appsettings.json
Archivo de configuración de la API, puede tener diferenntes versiones del archivo con diferentes nombres según el entorno de carga de la aplcicacion. Ej. *Development, Staging, Production,...*. 

En función del entorno (Ej. *Development, Staging, Production, etc.*), se carga el archivo de configuración adecuado (Ej. *appsettings.Development.json*) y se establece la cadena de conexión, en este caso a SQL Server.

En el archivo se configura: 
>- La cadena de conexión indispensable para acceder a la Base de Datos
>- El nivel de detección (detalle de la información) sobre los eventos. errores y excepciones para el logging
>- La dirección URL base en la que se encuentran los Endpoints y el protocolo de internet requerido (*http/https*) 

## Program.cs
- ### Creación del Constructor de la Aplicación
    **WebApplication.CreateBuilder(args)**: Crear un constructor de la aplicación web. Este constructor es la base para configurar y ejecutar la aplicación ASP.NET Core.

- ### Reflexión para el Ensamblado de Controladores y Repositorios
    El código utiliza la estrategia de reflexión para ensamblar controladores y repositorios basados en los marcadores dummy *ModelBase* y *DtoBase*. Se obtienen todos los tipos que hereden y se registran para cada entidad. 
    > Esto permite la creación dinámica  y automática de controladores y repositorios (reutilización de código, desacomplamiento y simplicidad)

- ### Configuración de Servicios
    Se configuran los servicios de la aplicación, incluido el servicio de *AutoMapper* para el mapeo entre Modelos y DTOs. También se registran las implementaciones de *ICommonMethods*, *IProjectService* y el controlador personalizado *CustomController*.

- ### Configuración de la Base de Datos
    Se registra el contexto de la base de datos (*DBContext*) y se configura la conexión de la base de datos según el entorno de la aplicación.

- ### Configuración del Pipeline HTTP
    Se configura el pipeline de solicitudes HTTP de la aplicación. Esto incluye:
    >- Configurar Swagger para la documentación de la API en el entorno de desarrollo. 
    >- Habilitar la redirección segura a HTTPS
    >- Agrega el middleware de autorización. 
    >- Mapear los controladores para gestionar las rutas de los endpoints.

- ### Ejecución de la Aplicación
    Finalmente, se construye y ejecuta la aplicación utilizando builder.Build(). Dependiendo del entorno, se habilita Swagger para la documentación en desarrollo o se configuran archivos y recursos estáticos en producción.

## Instrucciones de Ejecución
 1. Asegúrate de tener .NET Core SDK instalado en tu sistema.
 1. Clona este repositorio en tu máquina local.
 1. Abre la solución en Visual Studio o tu editor de código preferido.
 1. Asegúrate de que tienes una base de datos, sino ejecuta el archivo SQL que se encuentra en *Utilities/SeedData/InitDB.sql* > 1. Configura la conexión de datos en el archivo *appsettings.json* y realiza las migraciones necesarias.
 1. Ejecuta la aplicación.

## Instrucciones de uso
 1. Crea un Modelo y usa las anotaciones de EFCore para configurar características en los atributos (tipo de dato, requerido, nombre columna,...) que sean persistentes. Indica las posibles relaciones con otras entidades
 1. Crea un Dto correspondiente al modelo, puede ser idéntico en cuestón de atributos. Pero también puede tener menos propiedades o tener información adicional sin que afecte al mapeo entre ambos.
 
    > Deben heredar obligatoriamente de las clases arquetipo ficticias *Modelbase* y *DtoBase* respectivamente para: 
    >- Poder ser detectados por su tipo ficticio, ser ensamblados y formar un grupo. 
    >- Poder ser registrados como entidad junto con su propio controlador y su propio repositorio C.R.U.D en caso de necesitarlo 
    >- Poder ser mapeados por AutoMapper y tener asignación automática de sus atributos conforme se vayan añadiendo en el Modelo y después en el Dto 
   
 1. Registra el Modelo en el contexto de la Base de Datos(*DBContext*) para que genere su propia tabla. Haz uso de API Fluent para añadir otras características o restricciones
 1. Mediante la inyección de dependencias en tu controlador personalizado, puedes utilizar tu Modelo mediante su Dto. Con ello, ya podrás:
    >- Crear un controlador y un repositorio de tipo base customizado para tu entidad. Ej. `BaseRepository<Cliente,ClienteDto> clienteRepo`
    >- Generar endpoints para tu web o app 
    >- Realizar operaciones C.R.U.D en la Base de Datos
    >- Disponer de los servicios y las utilidades propias
     
## Contribuciones
Si deseas contribuir a este proyecto, siéntete libre de realizar una bifurcación (fork) y enviar solicitudes de extracción (pull requests) con tus mejoras.

## Contacto
Para obtener más información o ayuda, puedes ponerte en contacto con el equipo de desarrollo de lidera en ***desarrolloIT@lideraenergia.com***
