# Capacitación RabbitMQ con Simone.Common.RabbitMQ

Ejemplo simple de cómo usar RabbitMQ con Simone.Common.RabbitMQ. Demuestra cómo crear productores y consumidores de mensajes para una aplicación de chat.

## Comenzando

### Inicializar RabbitMQ

Este proyecto usa **Simone.Common.RabbitMQ** que requiere una instancia de RabbitMQ corriendo. 

### Configurar RabbitMQ en el consumidor y productor

En el archivo `appsettings.json` de ambos proyectos (Consumer y Producer), agrega la siguiente configuración para RabbitMQ:

**Productor (ChatProducer/appsettings.json):**
```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "username",
    "Password": "password",
    "DefaultExchange": "ChatExchange",
    "Exchanges": ["ChatExchange"]
  }
}
```

**Consumidor (ChatConsumer/appsettings.json):**
```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "username",
    "Password": "password",
    "Queues": {
      "ChatExchange": "ChatQueue"
    },
    "Errors": {
      "Queue": "ChatErrorsQueue",
      "Exchange": "ChatErrorsExchange"
    }
  }
}
```

Para más detalles sobre la configuración, consulta la documentación de Simone.Common.RabbitMQ.

### Ejecutar el Productor y el Consumidor

1. Inicia el contenedor de RabbitMQ desde Simone.Infrastructure
2. Ejecuta el proyecto consumidor para comenzar a escuchar mensajes
3. Ejecuta el proyecto productor para enviar mensajes a la cola

Abre la consola en cada proyecto y ejecuta `dotnet run` para iniciar las aplicaciones. Puedes enviar mensajes desde el productor y verlos siendo recibidos por el consumidor.

```bash
# Terminal 1 - Consumidor
cd ChatConsumer
dotnet run

# Terminal 2 - Productor
cd ChatProducer
dotnet run
```