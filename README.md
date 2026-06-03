# Visor LPGC

Visor 3D de Las Palmas de Gran Canaria desarrollado en Unity.

## Requisitos

- **Unity 6000.4.9f1** (o superior de la rama 6000.x)
  - Descargable desde [Unity Hub](https://unity.com/download)
- Git

## Instalación

1. Clona el repositorio:

```bash
git clone https://github.com/Juanelboi/Visor_LP.git
```

2. Abre **Unity Hub** y haz clic en **Add project from disk**.
3. Selecciona la carpeta `Visor-LPGC` que se acaba de clonar.
4. Unity generará automáticamente la carpeta `Library` al abrir el proyecto por primera vez — esto puede tardar varios minutos.

## Abrir el proyecto

Una vez abierto en Unity, ve a `File > Open Scene` y abre la escena principal desde la carpeta `Assets/`.

## Dependencias / Packages

Los paquetes necesarios están declarados en `Packages/manifest.json` y se descargan automáticamente al abrir el proyecto con Unity. No es necesario instalar nada manualmente.

## Notas

- La carpeta `Library/` no está incluida en el repositorio; Unity la regenera sola al abrir el proyecto.
- Si el proyecto no abre correctamente, asegúrate de usar exactamente la versión **6000.4.9f1** de Unity.
