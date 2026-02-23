# 🎸 Chile Hero 3 — WinForms (.NET) + MySQL

Aplicación **WinForms** para listar canciones, descargar archivos desde **Google Drive** con progreso y administrar usuarios (solo **Administrador** o **Dueño**) con edición del **rol** desde un `DataGridView`.

---

## ✨ Funcionalidades

### 🎵 Form2 — Lista de Canciones
- Carga canciones desde MySQL (join entre `cancioneschilehero3` y `artistas`).
- Buscador global por:
  - Artista, Canción, Álbum, Año y Descarga.
- Columna **Descarga** como link:
  - Muestra **Descargar** si existe enlace.
  - Muestra **Próximamente** si está vacío.
- Descarga desde Google Drive:
  - Extrae `fileId` automáticamente desde URL o ID.
  - Maneja confirmación (archivos grandes / warning).
  - Sugiere nombre del archivo desde headers.
  - Barra de progreso y mensaje “✅ Descarga completada”.

### 👥 Form3 — Gestión de Usuarios (Admin/Dueño)
- Menú **Gestión** visible solo para roles:
  - ✅ `Administrador`
  - ✅ `Dueño`
- Lista usuarios desde tabla `usuarios` (**NO muestra `contrasena`**).
- Buscador por: `nombre`, `correo`, `rol`, `IP`.
- Edición del campo **rol**:
  - Columna `rol` como **ComboBox** con los valores del ENUM.
  - Guarda automáticamente en MySQL al cambiar el rol:
    - `UPDATE usuarios SET rol=@rol WHERE id=@id`
- Modo “Baneados”:
  - Opción del menú abre Form3 filtrando `rol = 'Baneado'`.

---

## 🔒 Seguridad (Repositorio público)
Este repositorio es **público** y por seguridad:
- ✅ No incluye credenciales reales de base de datos
- ✅ No incluye contraseñas (ni hashes) en los listados
- ✅ La configuración de BD la realiza cada usuario localmente

> Si detectas un problema de seguridad, por favor abre un **Issue**.

---

## 🧱 Requisitos
- Windows 10/11
- Visual Studio 2022
- .NET 6/7/8 (según tu proyecto)
- MySQL Server (local o remoto)

### 📦 Paquete NuGet
- `MySqlConnector`

---

## 🗃️ Base de Datos

### Tabla `usuarios` (mínimo requerido)
Campos usados por la app:
- `id` (PK)
- `nombre`
- `correo`
- `genero` (opcional)
- `fecha_registro` (timestamp)
- `fecha_nacimiento` (date)
- `rol` (enum)
- `IP` (opcional)

**ENUM de `rol` esperado:**
- `Baneado`
- `Usuario`
- `VIP Donador`
- `VIP Premium`
- `VIP Supremo`
- `VIP Legendario`
- `Staff`
- `Administrador`
- `Dueño`

> ⚠️ Importante: La app **NO** trae ni muestra el campo `contrasena`.

### Tablas de Canciones (mínimo requerido)
- `cancioneschilehero3`: `id_artista`, `cancion`, `album`, `year`, `descarga`
- `artistas`: `id`, `nombre`

---

## ⚠️ Problema común: fechas `0000-00-00`
Si MySQL tiene valores como `0000-00-00` en `fecha_nacimiento`, `MySqlConnector` puede lanzar error al convertir a `DateTime`.

✅ Solución aplicada en Form3: usar `NULLIF()` en el SELECT para convertir fechas cero a `NULL`.

Ejemplo:
```sql
SELECT
  NULLIF(fecha_nacimiento, '0000-00-00') AS fecha_nacimiento
FROM usuarios;
