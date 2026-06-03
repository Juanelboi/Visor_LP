# Visor LPGC

Proyecto Unity para la visualización de Las Palmas de Gran Canaria.

## Requisitos

- Unity 6000.x o superior
- Git

## Cómo clonar el repositorio

```bash
git clone https://github.com/Juanelboi/Visor_LP.git
```

Abre la carpeta clonada desde Unity Hub como proyecto existente. Unity generará automáticamente la carpeta `Library` al abrir el proyecto por primera vez (puede tardar unos minutos).

## Estructura del proyecto

```
Assets/          # Escenas, scripts, modelos y recursos del proyecto
Packages/        # Paquetes de Unity (gestionados por el Package Manager)
ProjectSettings/ # Configuración del proyecto
```

## Flujo de trabajo con Git

```bash
# Ver cambios pendientes
git status

# Añadir cambios
git add .

# Crear un commit
git commit -m "Descripción del cambio"

# Subir al repositorio remoto
git push
```

## Notas

- La carpeta `Library/` está excluida del repositorio ya que Unity la regenera automáticamente.
- No subir las carpetas `Temp/`, `Logs/` ni archivos `.csproj`/`.sln` generados por el IDE.
