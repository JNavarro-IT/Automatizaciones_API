# AutoAPI Documentaci�n de AutoAPI

## Descripci�n
Esta documentaci�n proporciona una descripci�n general de la estructura y los componentes de la aplicaci�n AutoAPI, que utiliza ASP.NET Core y Entity Framework Core para administrar una base de datos y exponer una API REST.

Es esencial personalizar y ampliar esta documentaci�n seg�n sea necesario para su proyecto espec�fico, incluyendo detalles sobre rutas, puntos finales y otra informaci�n relevante.

## Estructura del Proyecto
>- **AutoAPI**: Carpeta principal del proyecto.
>- **Properties**: Carpeta que para publicar y compilar el proyecto en Visual Studio
>- **Context**: Carpeta con los tipos de contexto que pueden tener las bases de datos relacionadas con la aplicaci�n. Definen el contexto y las relaciones entre las tablas. Utiliza Entity Framework Core para facilitar el acceso y la manipulaci�n de la Base de Datos.
>- **Controllers**: La aplicaci�n tiene un controlador gen�rico para gestionar las solicitudes HTTP y para administrar entidades de la base de datos. Estos controladores siguen una interfaz gen�rica IBaseController y proporcionan operaciones CRUD comunes.
>- **Models**: Define los modelos de datos utilizados en la aplicaci�n.
>   - **DTOs**: Define objetos de transferencia de datos utilizados para comunicaci�n entre la API y el frontend.
>- **Repository**:  Clase que representa un C.R.U.D (Create, Read, Update, Delete) gen�rico para realizar operaciones b�sicas en una base de datos utilizando Entity Framework Core junto con cualquier modelo y su DTO. Uso de Automapper para reasignaci�n autom�tica de atributos
>- **Services**: Contiene servicios para realizar tareas espec�ficas.
>- **Utilities**: Carpeta con diferentes recursos para la aplicaci�n
>   + **Resources**: Carpeta para archivos de lectura sobre informaci�n est�tica
>   + **SeedData**: Carpeta con archivos SQL para inicializar la estructura de la Base de Datos
>   + **_CommonMethods_**: Archivo con m�todos gen�ricos y de utilidad
>   + **_MappingProfile_**: Archivo que contiene la configuraci�n de la herramienta AutoMapper para asignar Modelos y DTOs. Los ensambla en ambas direcciones con el uso de dos clases ficticias que los arquetipan: *ModelBase* y *DtoBase*

## Clase DBContext
La clase DBContext se encarga de definir el contexto de la base de datos y las relaciones entre las tablas. Utiliza Entity Framework Core para facilitar el acceso y la manipulaci�n de la base de datos.

> - **Clientes**: Propiedad que representa una tabla de clientes en la base de datos.
> - **OnModelCreating(ModelBuilder modelBuilder)**: Al sobreescribir este m�todo se hace uso de la herramienta API Fluent de EFCore que permite definir las caracter�sticas de los atributos de cada entidad, en lugar de usar las anotaciones de clase. Normalmente, se usa para describir relaciones complejas entre entidades (Ej. N:M), u otros tipos de restricciones

## BaseController
>- **GetListAsync()**: Obtiene una lista de entidades.
>- **GetByIdentity(object identity)**: Obtiene una entidad por su identificador.
>- **CreateEntity(object identity, [FromBody] TDto dto)**: Crea una nueva entidad.
>- **UpdateEntity(object identity, [FromBody] TDto dto)**: Actualiza una entidad existente.
>- **DeleteEntity(object identity, [FromBody] TDto dto)**: Elimina una entidad.
>- **UpdatePartialEntity(object identity, JsonPatchDocument<TDto> patchDto)**: Actualiza parcialmente una entidad.

## CustomController
El controlador personalizado CustomController proporciona funcionalidades adicionales, (*Ej. Obtenci�n de datos meteorol�gicos y c�lculo de edades*). Se inyecta la dependencia del repositorio propio de la entidad para acceder e interactuar con la tabla correspondiente al modelo que se le pase por par�metro tipo. Tambi�n se comunica con los servicios personalizados.

Inyecta la interfaz ILogger para poder generar un registro de los eventos de la entidad, se puede configurar para que salgan por consola, se copien en un archivo o se registren en la Base de Datos
 
>- **GetTiempo()**: Obtiene datos meteorol�gicos y los devuelve como respuesta.
>- **GetEdad(ClienteDto? Cliente)**: Calcula la edad de un cliente y la devuelve como respuesta.

## Cliente
La clase Cliente representa a un cliente en un proyecto y se mapea a la tabla "Clientes" en la base de datos. Incluye varias anotaciones de datos para especificar restricciones de la base de datos.

>- **IdCliente**: El identificador �nico para un cliente.
>- **Nombre**: El nombre del cliente.
>- **Dni**: El n�mero de identificaci�n del cliente.
>- **Telefono**: El n�mero de tel�fono del cliente.
>- **Email**: La direcci�n de correo electr�nico del cliente (nullable).

>### ClienteDto
>> La clase ClienteDto es un ejemplo de Objeto de Transferencia de Datos (DTO) utilizado para transportar informaci�n sobre la entidad Cliente. Incluye propiedades que coinciden con las de la clase Cliente y pueden contener informaci�n adicional.

## IBaseRepository y BaseRepository
La interfaz IProjectService define m�todos para interactuar con la Base de Datos

La clase BaseRepository es un repositorio gen�rico de CRUD (Crear, Leer, Actualizar, Eliminar) para entidades registradas, a trav�s de Entity Framework Core. Interact�a con la Base de Datos y realiza la asignaci�n autom�tica entre objetos modelo y DTO con el uso de AutoMapper

>- **GetEntitiesList()**: Recupera una lista de entidades de la base de datos y las asigna a DTOs.
>- **GetEntityDto(object? identity)**: Recupera una sola entidad por uN identificador �nico y la asigna a un DTO.
>- **CreateEntity(TDto tDto)**: Inserta una nueva entidad en la base de datos y en la tabla que corresponde a su modelo.
>- **UpdateEntity(TDto entityDto)**: Actualiza una entidad por completo de la base de datos a trav�s de su identificador
>- **DeleteEntity(TDto entityDto)**: Elimina una entidad de la Base de Datos a trav�s de su identificador
>- **EntityExists(TDto entityDto)**: Comprueba si existe una entidad en la Base de Datos sin tener en cuenta el identificador

## IProjectService y ProjectService
La interfaz IProjectService define m�todos que ofrecen servicios para el proyecto. Ej. *Obtener la edad de un cliente o el acceso a datos de fuentes externas.*

La clase ProjectService implementa la interfaz *IProjectService* y utiliza la interfaz *ICommonMethods* para obtener utilidades y *IBaseRepository* para acceder a los datos de los clientes.

>- **GetCIF(string Empresa)**: Recupera el CIF (N�mero de Identificaci�n Fiscal) de una empresa desde un archivo JSON externo.
>- **GetAge(ClienteDto Cliente)**: Calcula edad de un cliente a trav�s de su DNI (simulado).
>- **CheckMunicipio(string municipio, string provincia)**: Comprueba si un municipio est� recogido entre las localidades especiales recgidas en un JSON local.

## ICommonMethods y CommonMethods
La interfaz ICommonMethods define m�todos utilizables por cualquier entidad o controlador (reutilizaci�n de c�digo).

La clase CoomonMethoods implementa la interfaz *ICommonMethods* y desarrolla m�todos que sirven como herramientas o utilidades para la aplicaci�n.
>- **WithoutTildes(string item)**: Normalizar una cadena de texto en otra, eliminando los simbolos de acentuaci�n y derivados.
>- **GetWeather()**: Proporciona informaci�n meteorol�gica a tiempo real (simulado)

## MappingProfile
Configuraci�n de AutoMapper, biblioteca externa utilizada para asignar dos modelos de datos diferentes, en este caso entre un Modelo y su Dto. 

Este enfoque elimina la necesidad de configurar manualmente cada asignaci�n.

>- **Clases Dummy**: Las clases ModelBase y DtoBase sirven como clases ficticias para agrupar y ensamblar modelos y DTOs juntos. Se utilizan para simplificar la configuraci�n.
>- **AutoMapperConfig**: Clase que configura e inserta un nuevo perfil en la clase AutoMapper para que implemente la interfaz *IMapper* a trav�s del m�todo *Initialize()*
>- **MappingProfile**: Clase que implementa *Profile*, que es otra clase de AutoMapper. Para ello, se utiliza la reflexi�n y se van asignando los atributos de clase modelo a su correpondiente clase de DTO y viceversa. Los identifica por la jerarqu�a de herencia de ModelBase y DtoBase.

## appsettings.json
Archivo de configuraci�n de la API, puede tener diferenntes versiones del archivo con diferentes nombres seg�n el entorno de carga de la aplcicacion. Ej. *Development, Staging, Production,...*. 

En funci�n del entorno (Ej. *Development, Staging, Production, etc.*), se carga el archivo de configuraci�n adecuado (Ej. *appsettings.Development.json*) y se establece la cadena de conexi�n, en este caso a SQL Server.

En el archivo se configura: 
>- La cadena de conexi�n indispensable para acceder a la Base de Datos
>- El nivel de detecci�n (detalle de la informaci�n) sobre los eventos. errores y excepciones para el logging
>- La direcci�n URL base en la que se encuentran los Endpoints y el protocolo de internet requerido (*http/https*) 

## Program.cs
- ### Creaci�n del Constructor de la Aplicaci�n
    **WebApplication.CreateBuilder(args)**: Crear un constructor de la aplicaci�n web. Este constructor es la base para configurar y ejecutar la aplicaci�n ASP.NET Core.

- ### Reflexi�n para el Ensamblado de Controladores y Repositorios
    El c�digo utiliza la estrategia de reflexi�n para ensamblar controladores y repositorios basados en los marcadores dummy *ModelBase* y *DtoBase*. Se obtienen todos los tipos que hereden y se registran para cada entidad. 
    > Esto permite la creaci�n din�mica  y autom�tica de controladores y repositorios (reutilizaci�n de c�digo, desacomplamiento y simplicidad)

- ### Configuraci�n de Servicios
    Se configuran los servicios de la aplicaci�n, incluido el servicio de *AutoMapper* para el mapeo entre Modelos y DTOs. Tambi�n se registran las implementaciones de *ICommonMethods*, *IProjectService* y el controlador personalizado *CustomController*.

- ### Configuraci�n de la Base de Datos
    Se registra el contexto de la base de datos (*DBContext*) y se configura la conexi�n de la base de datos seg�n el entorno de la aplicaci�n.

- ### Configuraci�n del Pipeline HTTP
    Se configura el pipeline de solicitudes HTTP de la aplicaci�n. Esto incluye:
    >- Configurar Swagger para la documentaci�n de la API en el entorno de desarrollo. 
    >- Habilitar la redirecci�n segura a HTTPS
    >- Agrega el middleware de autorizaci�n. 
    >- Mapear los controladores para gestionar las rutas de los endpoints.

- ### Ejecuci�n de la Aplicaci�n
    Finalmente, se construye y ejecuta la aplicaci�n utilizando builder.Build(). Dependiendo del entorno, se habilita Swagger para la documentaci�n en desarrollo o se configuran archivos y recursos est�ticos en producci�n.

## Instrucciones de Ejecuci�n
 1. Aseg�rate de tener .NET Core SDK instalado en tu sistema.
 1. Clona este repositorio en tu m�quina local.
 1. Abre la soluci�n en Visual Studio o tu editor de c�digo preferido.
 1. Aseg�rate de que tienes una base de datos, sino ejecuta el archivo SQL que se encuentra en *Utilities/SeedData/InitDB.sql* > 1. Configura la conexi�n de datos en el archivo *appsettings.json* y realiza las migraciones necesarias.
 1. Ejecuta la aplicaci�n.

## Instrucciones de uso
 1. Crea un Modelo y usa las anotaciones de EFCore para configurar caracter�sticas en los atributos (tipo de dato, requerido, nombre columna,...) que sean persistentes. Indica las posibles relaciones con otras entidades
 1. Crea un Dto correspondiente al modelo, puede ser id�ntico en cuest�n de atributos. Pero tambi�n puede tener menos propiedades o tener informaci�n adicional sin que afecte al mapeo entre ambos.
 
    > Deben heredar obligatoriamente de las clases arquetipo ficticias *Modelbase* y *DtoBase* respectivamente para: 
    >- Poder ser detectados por su tipo ficticio, ser ensamblados y formar un grupo. 
    >- Poder ser registrados como entidad junto con su propio controlador y su propio repositorio C.R.U.D en caso de necesitarlo 
    >- Poder ser mapeados por AutoMapper y tener asignaci�n autom�tica de sus atributos conforme se vayan a�adiendo en el Modelo y despu�s en el Dto 
   
 1. Registra el Modelo en el contexto de la Base de Datos(*DBContext*) para que genere su propia tabla. Haz uso de API Fluent para a�adir otras caracter�sticas o restricciones
 1. Mediante la inyecci�n de dependencias en tu controlador personalizado, puedes utilizar tu Modelo mediante su Dto. Con ello, ya podr�s:
    >- Crear un controlador y un repositorio de tipo base customizado para tu entidad. Ej. `BaseRepository<Cliente,ClienteDto> clienteRepo`
    >- Generar endpoints para tu web o app 
    >- Realizar operaciones C.R.U.D en la Base de Datos
    >- Disponer de los servicios y las utilidades propias
     
## Contribuciones
Si deseas contribuir a este proyecto, si�ntete libre de realizar una bifurcaci�n (fork) y enviar solicitudes de extracci�n (pull requests) con tus mejoras.

## Contacto
Para obtener m�s informaci�n o ayuda, puedes ponerte en contacto con el equipo de desarrollo de lidera en ***desarrolloIT@lideraenergia.com***
