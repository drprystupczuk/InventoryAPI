
# README - Inventory API y Notification Service con Docker y RabbitMQ

Este proyecto incluye dos servicios: **Inventory API** y **Notification Service**, ambos ejecutándose en contenedores Docker. Además, se utiliza **RabbitMQ** como middleware para la mensajería entre los servicios.

## Requisitos previos

- Docker y Docker Compose instalados en tu máquina.
- .NET SDK (en caso de que necesites trabajar con el código directamente, no solo ejecutar los contenedores).

## Estructura del Proyecto

```
/Kinetic
    /InventoryAPI
        Dockerfile
        InventoryAPI.csproj
        ...
    /NotificationService
        Dockerfile
        NotificationService.csproj
        ...
    docker-compose.yml
```

## Instrucciones para ejecutar la solución

### 1. Construir y levantar los contenedores

Asegúrate de estar en la carpeta **/Kinetic** que contiene el archivo **docker-compose.yml**, y ejecuta el siguiente comando para construir y levantar los contenedores:

```bash
docker-compose up --build
```

Este comando hace lo siguiente:
- Construye las imágenes de Docker para **InventoryAPI** y **NotificationService**.
- Levanta los contenedores de los servicios, junto con **RabbitMQ**.
- Mapea los puertos para acceder a la API de **InventoryAPI** en `http://localhost:5000` y **NotificationService** en `http://localhost:5001`.
- Los contenedores **RabbitMQ** están accesibles en **`http://localhost:15672`** (usuario: `guest`, contraseña: `guest`).

### 2. Aplicar las migraciones (si es necesario)

Si necesitas ejecutar las migraciones de la base de datos **SQLite** (que se usa en **InventoryAPI**), puedes hacerlo de las siguientes maneras:

#### Opción 1: Ejecutar migraciones en un contenedor separado (recomendado)

El servicio **`ef-migrations`** está configurado en el archivo **`docker-compose.yml`** para instalar **`dotnet-ef`** y ejecutar las migraciones al iniciar el contenedor. Si no se ejecutaron automáticamente, puedes forzar la ejecución de las migraciones ejecutando:

```bash
docker-compose run ef-migrations
```

Este comando instalará **`dotnet-ef`** (si no está instalado) y luego ejecutará las migraciones.

#### Opción 2: Ejecutar migraciones manualmente desde el contenedor de `inventory-api`

Si el contenedor de **`ef-migrations`** no está funcionando correctamente, puedes aplicar las migraciones manualmente accediendo al contenedor de **`inventory-api`**:

1. Accede al contenedor **`inventory-api`**:

   ```bash
   docker exec -it kinetic-inventory-api-1 bash
   ```

2. Dentro del contenedor, ejecuta las migraciones:

   ```bash
   dotnet ef database update
   ```

Esto aplicará las migraciones y creará las tablas necesarias en la base de datos **SQLite**.

### 3. Acceder a Swagger

Una vez que los contenedores estén corriendo, podrás acceder a la documentación de la API de **InventoryAPI** en:

```
http://localhost:5000/swagger
```

Aquí podrás probar los diferentes endpoints de la API de **InventoryAPI**.

### 4. Verificar RabbitMQ

Puedes acceder a **RabbitMQ** en:

```
http://localhost:15672
```

Inicia sesión con las credenciales predeterminadas:
- Usuario: `guest`
- Contraseña: `guest`

Aquí podrás ver las colas y los mensajes que se están enviando entre los servicios.

### 5. Detener los contenedores

Cuando hayas terminado de trabajar, puedes detener los contenedores con el siguiente comando:

```bash
docker-compose down
```

Esto detendrá los contenedores y eliminará cualquier red de Docker que se haya creado.

## Notas adicionales

- Si deseas aplicar cambios en la base de datos, asegúrate de que **`dotnet ef`** esté correctamente configurado y las migraciones se hayan aplicado.
- Los datos de la base de datos **SQLite** están almacenados en un volumen de Docker (`inventory_db_data`), lo que garantiza que los datos persistan entre reinicios de los contenedores.

---

### Resumen de comandos importantes:

- **Levantar contenedores**:

  ```bash
  docker-compose up --build
  ```

- **Ejecutar migraciones**:

  ```bash
  docker-compose run ef-migrations
  ```

- **Acceder a Swagger** en **InventoryAPI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

- **Acceder a RabbitMQ**: [http://localhost:15672](http://localhost:15672)  
  (Usuario: `guest`, Contraseña: `guest`)

- **Detener contenedores**:

  ```bash
  docker-compose down
  ```

