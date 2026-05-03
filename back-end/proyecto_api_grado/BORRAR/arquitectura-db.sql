/* =============================================================================
   E-COMMERCE DATABASE - v2.0
   Motor:      Microsoft SQL Server
   Backend:    .NET / C# con ADO.NET
   Autor:      Nicolás Vacca - ADSO: 3070124
   Fecha:      2025

   CONVENCIONES USADAS EN ESTE SCRIPT:
   - Nombres de tablas:   PascalCase  (ej: Cliente, Producto)
   - Nombres de columnas: snake_case  (ej: id_cliente, fecha_registro)
   - PKs:                 id_{tabla}  siempre BIGINT IDENTITY
   - FKs:                 id_{tabla_referenciada}
   - Estados/Enums:       NVARCHAR con CHECK IN (...)
   - Fechas de auditoría: created_at / updated_at en tablas críticas
   ============================================================================= */

-- Crear y seleccionar la base de datos
CREATE DATABASE EcommerceDB;
GO

USE EcommerceDB;
GO


/* =============================================================================
   SECCIÓN 1: UBICACIÓN GEOGRÁFICA
   Razón: Otras tablas dependen de esta sección, debe crearse primero.
   Flujo: Departamento → Ciudad → CodigoPostal
   ============================================================================= */

CREATE TABLE Departamento (
    id_departamento     INT             IDENTITY(1,1)   PRIMARY KEY,
    nombre              NVARCHAR(100)   NOT NULL        UNIQUE,
    codigo_dane         VARCHAR(10)     NOT NULL        UNIQUE,     -- Código oficial DANE Colombia
    activo              BIT             NOT NULL        DEFAULT 1   -- 1 = Activo, 0 = Inactivo (más eficiente que NVARCHAR)
);
GO

CREATE TABLE Ciudad (
    id_ciudad           INT             IDENTITY(1,1)   PRIMARY KEY,
    nombre              NVARCHAR(100)   NOT NULL,
    codigo_dane         VARCHAR(10)     NOT NULL        UNIQUE,
    activo              BIT             NOT NULL        DEFAULT 1,
    id_departamento     INT             NOT NULL,

    CONSTRAINT FK_Ciudad_Departamento FOREIGN KEY (id_departamento)
        REFERENCES Departamento(id_departamento)
);
GO

CREATE TABLE CodigoPostal (
    id_codigo_postal    INT             IDENTITY(1,1)   PRIMARY KEY,
    codigo              VARCHAR(10)     NOT NULL,
    activo              BIT             NOT NULL        DEFAULT 1,
    id_ciudad           INT             NOT NULL,

    CONSTRAINT FK_CodigoPostal_Ciudad FOREIGN KEY (id_ciudad)
        REFERENCES Ciudad(id_ciudad),

    -- Un código postal es único dentro de una ciudad
    CONSTRAINT UQ_CodigoPostal_Ciudad UNIQUE (codigo, id_ciudad)
);
GO


/* =============================================================================
   SECCIÓN 2: CLIENTES
   Razón: El cliente es el actor principal del e-commerce.
   Nota:  La dirección se separó en su propia tabla para permitir
          múltiples direcciones por cliente (patrón estándar de e-commerce).
   ============================================================================= */

CREATE TABLE Cliente (
    id_cliente          BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    nombres             NVARCHAR(100)   NOT NULL,
    apellidos           NVARCHAR(100)   NOT NULL,
    email               NVARCHAR(255)   NOT NULL        UNIQUE,
    telefono            NVARCHAR(20)    NULL,
    contrasena_hash     NVARCHAR(255)   NOT NULL,       -- Almacenar SOLO el hash (bcrypt recomendado en .NET)
    estado              NVARCHAR(20)    NOT NULL        DEFAULT 'Activo',
    created_at          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    updated_at          DATETIME2       NULL,           -- Se actualiza desde el backend al modificar el registro

    CONSTRAINT CHK_Cliente_Estado CHECK (estado IN ('Activo', 'Inactivo', 'Suspendido', 'Baneado'))
);
GO

-- Tabla de direcciones separada: un cliente puede tener múltiples direcciones.
-- Esto es estándar en cualquier e-commerce real (casa, trabajo, etc.)
CREATE TABLE DireccionCliente (
    id_direccion        BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    descripcion         NVARCHAR(500)   NOT NULL,       -- Ej: "Calle 10 #23-45, Apto 301"
    es_principal        BIT             NOT NULL        DEFAULT 0, -- Solo una puede ser principal por cliente
    activo              BIT             NOT NULL        DEFAULT 1,
    id_cliente          BIGINT          NOT NULL,
    id_codigo_postal    INT             NOT NULL,

    CONSTRAINT FK_DireccionCliente_Cliente      FOREIGN KEY (id_cliente)
        REFERENCES Cliente(id_cliente),
    CONSTRAINT FK_DireccionCliente_CodigoPostal FOREIGN KEY (id_codigo_postal)
        REFERENCES CodigoPostal(id_codigo_postal)
);
GO


/* =============================================================================
   SECCIÓN 3: CATÁLOGO DE PRODUCTOS
   Flujo: Categoria → Producto → ImagenProducto (opcional, para el frontend)
   ============================================================================= */

CREATE TABLE Categoria (
    id_categoria        INT             IDENTITY(1,1)   PRIMARY KEY,
    nombre              NVARCHAR(100)   NOT NULL        UNIQUE,
    descripcion         NVARCHAR(500)   NULL,
    activo              BIT             NOT NULL        DEFAULT 1
);
GO

CREATE TABLE Producto (
    id_producto         BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    nombre              NVARCHAR(150)   NOT NULL,
    descripcion         NVARCHAR(2000)  NULL,
    sku                 NVARCHAR(100)   NULL            UNIQUE,     -- Stock Keeping Unit: identificador logístico interno
    precio              DECIMAL(10,2)   NOT NULL,
    stock_cantidad      INT             NOT NULL        DEFAULT 0,
    activo              BIT             NOT NULL        DEFAULT 1,
    created_at          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    updated_at          DATETIME2       NULL,
    id_categoria        INT             NOT NULL,

    CONSTRAINT CHK_Producto_Precio  CHECK (precio >= 0),
    CONSTRAINT CHK_Producto_Stock   CHECK (stock_cantidad >= 0),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (id_categoria)
        REFERENCES Categoria(id_categoria)
);
GO

-- Tabla auxiliar para imágenes del producto.
-- Separada de Producto para permitir múltiples imágenes por producto.
CREATE TABLE ImagenProducto (
    id_imagen           INT             IDENTITY(1,1)   PRIMARY KEY,
    url_imagen          NVARCHAR(500)   NOT NULL,       -- URL del CDN o servidor de archivos
    es_principal        BIT             NOT NULL        DEFAULT 0,
    orden               TINYINT         NOT NULL        DEFAULT 0, -- Para ordenar el carrusel de imágenes
    id_producto         BIGINT          NOT NULL,

    CONSTRAINT FK_ImagenProducto_Producto FOREIGN KEY (id_producto)
        REFERENCES Producto(id_producto)
);
GO


/* =============================================================================
   SECCIÓN 4: RESEÑAS
   Nota: Solo se almacena la FK al cliente y al producto.
         El nombre del cliente se obtiene con un JOIN al momento de consultar.
         Esto garantiza que los datos siempre estén actualizados y normalizados.
   ============================================================================= */

CREATE TABLE Resena (
    id_resena           BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    rating              TINYINT         NOT NULL,       -- 1 a 5 estrellas
    comentario          NVARCHAR(1000)  NULL,
    created_at          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    id_cliente          BIGINT          NOT NULL,
    id_producto         BIGINT          NOT NULL,

    CONSTRAINT CHK_Resena_Rating    CHECK (rating BETWEEN 1 AND 5),
    CONSTRAINT FK_Resena_Cliente    FOREIGN KEY (id_cliente)    REFERENCES Cliente(id_cliente),
    CONSTRAINT FK_Resena_Producto   FOREIGN KEY (id_producto)   REFERENCES Producto(id_producto),

    -- Un cliente solo puede reseñar un producto una vez
    CONSTRAINT UQ_Resena_ClienteProducto UNIQUE (id_cliente, id_producto)
);
GO


/* =============================================================================
   SECCIÓN 5: PEDIDOS Y FACTURACIÓN
   Flujo de negocio: Pedido → DetallePedido → Factura → Pago → Envio

   ¿Por qué Pedido y Factura son tablas separadas?
   - El Pedido representa la INTENCIÓN de compra (puede cancelarse, modificarse).
   - La Factura es el documento LEGAL que se genera al confirmar el pago.
   - Esta separación es estándar en contabilidad y sistemas ERP.
   ============================================================================= */

CREATE TABLE Pedido (
    id_pedido           BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    estado              NVARCHAR(30)    NOT NULL        DEFAULT 'Pendiente',
    created_at          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    updated_at          DATETIME2       NULL,
    id_cliente          BIGINT          NOT NULL,
    id_direccion        BIGINT          NOT NULL,       -- Dirección de entrega elegida por el cliente

    CONSTRAINT CHK_Pedido_Estado CHECK (estado IN ('Pendiente', 'Confirmado', 'En preparacion', 'Enviado', 'Entregado', 'Cancelado')),
    CONSTRAINT FK_Pedido_Cliente    FOREIGN KEY (id_cliente)    REFERENCES Cliente(id_cliente),
    CONSTRAINT FK_Pedido_Direccion  FOREIGN KEY (id_direccion)  REFERENCES DireccionCliente(id_direccion)
);
GO

-- Detalle del pedido: qué productos y en qué cantidad pidió el cliente.
-- El precio_unitario se congela al momento del pedido (aunque el precio del
-- producto cambie después, el pedido conserva el precio original).
CREATE TABLE DetallePedido (
    id_detalle          BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    cantidad            INT             NOT NULL,
    precio_unitario     DECIMAL(10,2)   NOT NULL,       -- Precio al momento del pedido (congelado)
    subtotal            AS (cantidad * precio_unitario) PERSISTED, -- Columna calculada y persistida en disco
    id_pedido           BIGINT          NOT NULL,
    id_producto         BIGINT          NOT NULL,

    CONSTRAINT CHK_DetallePedido_Cantidad   CHECK (cantidad > 0),
    CONSTRAINT CHK_DetallePedido_Precio     CHECK (precio_unitario >= 0),
    CONSTRAINT FK_DetallePedido_Pedido      FOREIGN KEY (id_pedido)     REFERENCES Pedido(id_pedido),
    CONSTRAINT FK_DetallePedido_Producto    FOREIGN KEY (id_producto)   REFERENCES Producto(id_producto),

    -- No puede haber dos líneas del mismo producto en el mismo pedido
    CONSTRAINT UQ_DetallePedido_PedidoProducto UNIQUE (id_pedido, id_producto)
);
GO

-- La factura se genera cuando el pedido es confirmado.
-- nro_factura lo genera el backend con el formato que definas (ej: FAC-2025-00001)
CREATE TABLE Factura (
    id_factura          BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    nro_factura         NVARCHAR(30)    NOT NULL        UNIQUE,
    subtotal            DECIMAL(10,2)   NOT NULL,
    iva                 DECIMAL(10,2)   NOT NULL        DEFAULT 0.00,
    total               AS (subtotal + iva)             PERSISTED,  -- Calculado automáticamente, sin riesgo de error
    fecha_emision       DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    id_pedido           BIGINT          NOT NULL        UNIQUE,      -- Una factura por pedido

    CONSTRAINT CHK_Factura_Subtotal CHECK (subtotal >= 0),
    CONSTRAINT CHK_Factura_IVA      CHECK (iva >= 0),
    CONSTRAINT FK_Factura_Pedido    FOREIGN KEY (id_pedido) REFERENCES Pedido(id_pedido)
);
GO


/* =============================================================================
   SECCIÓN 6: PAGOS
   Un pedido puede tener múltiples intentos de pago (ej: el primero falla,
   el segundo se completa). Solo uno puede tener estado 'Completado'.
   ============================================================================= */

CREATE TABLE Pago (
    id_pago             BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    metodo_pago         NVARCHAR(30)    NOT NULL,
    monto               DECIMAL(10,2)   NOT NULL,
    estado              NVARCHAR(20)    NOT NULL        DEFAULT 'Pendiente',
    referencia_externa  NVARCHAR(255)   NULL,           -- ID de transacción de PayPal, pasarela, etc.
    fecha_pago          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    id_factura          BIGINT          NOT NULL,

    CONSTRAINT CHK_Pago_Metodo  CHECK (metodo_pago IN ('Tarjeta', 'PayPal', 'Efectivo', 'Transferencia')),
    CONSTRAINT CHK_Pago_Estado  CHECK (estado IN ('Pendiente', 'Completado', 'Fallido', 'Reembolsado')),
    CONSTRAINT CHK_Pago_Monto   CHECK (monto > 0),
    CONSTRAINT FK_Pago_Factura  FOREIGN KEY (id_factura) REFERENCES Factura(id_factura)
);
GO


/* =============================================================================
   SECCIÓN 7: ENVÍOS
   El envío se relaciona con el Pedido (no con el Pago).
   Razón: El envío es una consecuencia del pedido, no del medio de pago.
          El backend debe validar que el pago esté completado antes de crear el envío.
   ============================================================================= */

CREATE TABLE Envio (
    id_envio            BIGINT          IDENTITY(1,1)   PRIMARY KEY,
    numero_guia         NVARCHAR(100)   NULL,           -- Número de rastreo de la transportadora
    empresa_transporte  NVARCHAR(100)   NULL,
    metodo_envio        NVARCHAR(30)    NOT NULL,
    costo_envio         DECIMAL(10,2)   NOT NULL        DEFAULT 0.00,
    estado_envio        NVARCHAR(30)    NOT NULL        DEFAULT 'Pendiente',
    fecha_despacho      DATETIME2       NULL,           -- Cuándo salió del almacén
    fecha_entrega       DATETIME2       NULL,           -- Cuándo llegó al cliente
    created_at          DATETIME2       NOT NULL        DEFAULT SYSDATETIME(),
    id_pedido           BIGINT          NOT NULL        UNIQUE,      -- Un envío por pedido

    CONSTRAINT CHK_Envio_Metodo CHECK (metodo_envio IN ('Estandar', 'Express', 'Recogida en tienda')),
    CONSTRAINT CHK_Envio_Estado CHECK (estado_envio IN ('Pendiente', 'En preparacion', 'En transito', 'Entregado', 'Cancelado')),
    CONSTRAINT CHK_Envio_Costo  CHECK (costo_envio >= 0),
    CONSTRAINT FK_Envio_Pedido  FOREIGN KEY (id_pedido) REFERENCES Pedido(id_pedido)
);
GO


/* =============================================================================
   ÍNDICES ADICIONALES
   Razón: Las FKs no crean índices automáticamente en SQL Server.
          Sin estos índices, los JOINs sobre columnas FK hacen table scans
          completos, lo que degrada el rendimiento con miles de registros.
   ============================================================================= */

-- Consultas frecuentes: "todos los pedidos de un cliente"
CREATE INDEX IX_Pedido_Cliente        ON Pedido(id_cliente);

-- Consultas frecuentes: "todos los detalles de un pedido"
CREATE INDEX IX_DetallePedido_Pedido  ON DetallePedido(id_pedido);

-- Consultas frecuentes: "todos los productos de una categoría"
CREATE INDEX IX_Producto_Categoria    ON Producto(id_categoria);

-- Consultas frecuentes: "todas las reseñas de un producto"
CREATE INDEX IX_Resena_Producto       ON Resena(id_producto);

-- Consultas frecuentes: "todos los pagos de una factura"
CREATE INDEX IX_Pago_Factura          ON Pago(id_factura);

-- Consultas frecuentes: "ciudades por departamento"
CREATE INDEX IX_Ciudad_Departamento   ON Ciudad(id_departamento);
GO


/* =============================================================================
   FIN DEL SCRIPT
   
   FLUJO COMPLETO DE UN PEDIDO (para referencia del backend):
   
   1. Cliente se registra             → INSERT Cliente
   2. Cliente agrega dirección        → INSERT DireccionCliente
   3. Cliente agrega productos        → (lógica en memoria / carrito)
   4. Cliente confirma pedido         → INSERT Pedido + INSERT DetallePedido (x N productos)
                                         UPDATE Producto SET stock_cantidad -= cantidad (por cada ítem)
   5. Se genera la factura            → INSERT Factura (nro_factura generado en backend)
   6. Cliente paga                    → INSERT Pago
   7. Se confirma el pago             → UPDATE Pago SET estado = 'Completado'
                                         UPDATE Pedido SET estado = 'Confirmado'
   8. Se despacha el pedido           → INSERT Envio
                                         UPDATE Pedido SET estado = 'Enviado'
   9. Pedido entregado                → UPDATE Envio SET estado_envio = 'Entregado', fecha_entrega = SYSDATETIME()
                                         UPDATE Pedido SET estado = 'Entregado'
   10. Cliente deja reseña            → INSERT Resena
   ============================================================================= */