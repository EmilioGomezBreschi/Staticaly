# Base de Datos

## Diagrama de Entidad-Relación

### Tabla: Usuarios

| Campo      | Tipo de dato | Descripción                                              |
| ---------- | ------------ | -------------------------------------------------------- |
| UsuarioID  | INT          | Identificador único para cada usuario                    |
| Nombre     | VARCHAR(50)  | Nombre del usuario                                       |
| Apellido   | VARCHAR(50)  | Apellido del usuario                                     |
| Email      | VARCHAR(100) | Dirección de correo electrónico del usuario              |
| Contraseña | VARCHAR(100) | Contraseña del usuario                                   |
| RolID      | INT          | Referencia al ID del rol del usuario en la tabla "Roles" |
| Imagen     | VARCHAR(100) | Ruta de la imagen del usuario                            |

### Tabla: Roles

| Campo | Tipo de dato | Descripción                       |
| ----- | ------------ | --------------------------------- |
| RolID | INT          | Identificador único para cada rol |
| Rol   | VARCHAR(50)  | Nombre del rol                    |

### Tabla: RangoUsuarios

| Campo      | Tipo de dato | Descripción                           |
| ---------- | ------------ | ------------------------------------- |
| RangoID    | INT          | Identificador único para cada rango   |
| Rango      | VARCHAR(50)  | Nombre del rango                      |
| PuntajeMin | INT          | Puntaje mínimo para alcanzar el rango |
| PuntajeMax | INT          | Puntaje máximo para alcanzar el rango |

### Tabla: Equipos

| Campo        | Tipo de dato | Descripción                                                    |
| ------------ | ------------ | -------------------------------------------------------------- |
| EquipoID     | INT          | Identificador único para cada equipo                           |
| Nombre       | VARCHAR(50)  | Nombre del equipo                                              |
| Descripcion  | VARCHAR(100) | Descripción del equipo                                         |
| TipoEquipoID | INT          | Referencia al ID del tipo de equipo en la tabla "TiposEquipos" |

### Tabla: TiposEquipos

| Campo        | Tipo de dato | Descripción                                  |
| ------------ | ------------ | -------------------------------------------- |
| TipoEquipoID | INT          | Identificador único para cada tipo de equipo |
| Tipo         | VARCHAR(50)  | Nombre del tipo de equipo                    |

### Tabla: UsuariosEquipos

| Campo           | Tipo de dato | Descripción                                           |
| --------------- | ------------ | ----------------------------------------------------- |
| UsuarioEquipoID | INT          | Identificador único para cada relación usuario-equipo |
| UsuarioID       | INT          | Referencia al ID del usuario en la tabla "Usuarios"   |
| PermisoID       | INT          | Referencia al ID del permiso en la tabla "Permisos"   |
| EquipoID        | INT          | Referencia al ID del equipo en la tabla "Equipos"     |

### Tabla: Foros

| Campo         | Tipo de dato | Descripción                                        |
| ------------- | ------------ | -------------------------------------------------- |
| ForoID        | INT          | Identificador único para cada foro                 |
| Titulo        | VARCHAR(100) | Título del foro                                    |
| Descripcion   | VARCHAR(200) | Descripción del foro                               |
| EquipoID      | INT          | Referencia al ID del Equipo en el que esta el foro |
| FechaCreacion | DATE         | Fecha de creación del foro                         |

### Tabla: Publicaciones

| Campo         | Tipo de dato | Descripción                                                                    |
| ------------- | ------------ | ------------------------------------------------------------------------------ |
| PublicacionID | INT          | Identificador único para cada publicación                                      |
| ForoID        | INT          | Referencia al ID del foro al que pertenece la publicación en la tabla "Foros"  |
| UsuarioID     | INT          | Referencia al ID del usuario que realizó la publicación en la tabla "Usuarios" |
| Contenido     | TEXT         | Contenido de la publicación                                                    |
| Imagen        | VARCHAR(100) | Ruta de la imagen relacionada con la publicación                               |

### Tabla: Comentarios

| Campo         | Tipo de dato | Descripción                                                                                     |
| ------------- | ------------ | ----------------------------------------------------------------------------------------------- |
| ComentarioID  | INT          | Identificador único para cada comentario                                                        |
| PublicacionID | INT          | Referencia al ID de la publicación a la que pertenece el comentario en la tabla "Publicaciones" |
| UsuarioID     | INT          | Referencia al ID del usuario que realizó el comentario en la tabla "Usuarios"                   |
| Contenido     | TEXT         | Contenido del comentario                                                                        |
| Imagen        | VARCHAR(100) | Ruta de la imagen relacionada con el comentario                                                 |

### Tabla: RevisionComentarios

| Campo                | Tipo de dato | Descripción                                                                 |
| -------------------- | ------------ | --------------------------------------------------------------------------- |
| RevisionComentarioID | INT          | Identificador único para cada revisión de comentario                        |
| ComentarioID         | INT          | Referencia al ID del comentario en la tabla "Comentarios"                   |
| UsuarioID            | INT          | Referencia al ID del usuario que realizó la revisión en la tabla "Usuarios" |
| Aprobado             | TINYINT(1)   | Indica si el comentario ha sido aprobado (1) o no (0)                       |

### Tabla: Proyectos

| Campo       | Tipo de dato | Descripción                            |
| ----------- | ------------ | -------------------------------------- |
| ProyectoID  | INT          | Identificador único para cada proyecto |
| Nombre      | VARCHAR(100) | Nombre del proyecto                    |
| Descripcion | VARCHAR(200) | Descripción del proyecto               |
| FechaInicio | DATE         | Fecha de inicio del proyecto           |
| FechaFin    | DATE         | Fecha de finalización del proyecto     |

### Tabla: Permisos

| Campo     | Tipo de dato | Descripción                           |
| --------- | ------------ | ------------------------------------- |
| PermisoID | INT          | Identificador único para cada permiso |
| Nombre    | VARCHAR(50)  | Nombre del permiso                    |

### Tabla: UsuariosProyectos

| Campo             | Tipo de dato | Descripción                                             |
| ----------------- | ------------ | ------------------------------------------------------- |
| UsuarioProyectoID | INT          | Identificador único para cada relación usuario-proyecto |
| UsuarioID         | INT          | Referencia al ID del usuario en la tabla "Usuarios"     |
| ProyectoID        | INT          | Referencia al ID del proyecto en la tabla "Proyectos"   |

### Tabla: Encuestas

| Campo       | Tipo de dato | Descripción                            |
| ----------- | ------------ | -------------------------------------- |
| EncuestaID  | INT          | Identificador único para cada encuesta |
| Nombre      | VARCHAR(100) | Nombre de la encuesta                  |
| Descripcion | VARCHAR(200) | Descripción de la encuesta             |

### Tabla: Preguntas

| Campo      | Tipo de dato | Descripción                                                                            |
| ---------- | ------------ | -------------------------------------------------------------------------------------- |
| PreguntaID | INT          | Identificador único para cada pregunta                                                 |
| EncuestaID | INT          | Referencia al ID de la encuesta a la que pertenece la pregunta en la tabla "Encuestas" |
| Contenido  | TEXT         | Contenido de la pregunta                                                               |

### Tabla: Respuestas

| Campo       | Tipo de dato | Descripción                                                                             |
| ----------- | ------------ | --------------------------------------------------------------------------------------- |
| RespuestaID | INT          | Identificador único para cada respuesta                                                 |
| PreguntaID  | INT          | Referencia al ID de la pregunta a la que pertenece la respuesta en la tabla "Preguntas" |
| Contenido   | TEXT         | Contenido de la respuesta                                                               |

### Tabla: DatosEncuestas

| Campo           | Tipo de dato | Descripción                                                                   |
| --------------- | ------------ | ----------------------------------------------------------------------------- |
| DatosEncuestaID | INT          | Identificador único para cada conjunto de datos de encuesta                   |
| EncuestaID      | INT          | Referencia al ID de la encuesta en la tabla "Encuestas"                       |
| UsuarioID       | INT          | Referencia al ID del usuario que respondió la encuesta en la tabla "Usuarios" |
| FechaRespuesta  | DATE         | Fecha en la que se respondió la encuesta                                      |

### Tabla: RespuestasUsuarios

| Campo              | Tipo de dato | Descripción                                                            |
| ------------------ | ------------ | ---------------------------------------------------------------------- |
| RespuestaUsuarioID | INT          | Identificador único para cada respuesta de usuario                     |
| DatosEncuestaID    | INT          | Referencia al ID de los datos de encuesta en la tabla "DatosEncuestas" |
| PreguntaID         | INT          | Referencia al ID de la pregunta en la tabla "Preguntas"                |
| RespuestaID        | INT          | Referencia al ID de la respuesta en la tabla "Respuestas"              |

### Tabla: Ejercicios

| Campo                  | Tipo de dato | Descripción                                                                  |
| ---------------------- | ------------ | ---------------------------------------------------------------------------- |
| EjercicioID            | INT          | Identificador único para cada ejercicio                                      |
| ForoID                 | INT          | Referencia al ID del foro en la tabla "Foros"                                |
| UsuarioID              | INT          | Referencia al ID del usuario que publicó el ejercicio en la tabla "Usuarios" |
| Titulo                 | VARCHAR(100) | Título del ejercicio                                                         |
| Descripcion            | VARCHAR(200) | Descripción del ejercicio                                                    |
| Imagen                 | VARCHAR(100) | Ruta de la imagen relacionada con el ejercicio                               |
| RespuestaEstablecidaID | INT          | Referencia al ID de la respuesta establecida en la tabla "Respuestas"        |
| RespuestaAlumno        | TEXT         | Respuesta proporcionada por el alumno                                        |

### Tabla: Puntajes

| Campo       | Tipo de dato | Descripción                                             |
| ----------- | ------------ | ------------------------------------------------------- |
| PuntajeID   | INT          | Identificador único para cada puntaje                   |
| UsuarioID   | INT          | Referencia al ID del usuario en la tabla "Usuarios"     |
| EjercicioID | INT          | Referencia al ID del ejercicio en la tabla "Ejercicios" |
| Puntaje     | INT          | Puntaje obtenido por el usuario en el ejercicio         |
| Tiempo      | TIME         | Tiempo que tomó al usuario completar el ejercicio       |
