/* =============================================================================
   SECCIÓN 1: UBICACIÓN GEOGRÁFICA
   ============================================================================= */

/* 
=============================
TABLA: DEPARTAMENTO.
=============================
*/

-- REGISTRAR DEPARTAMENTO
CREATE PROCEDURE usp_Departamento_Registrar
    @nombre NVARCHAR(100),
    @codigo_dane VARCHAR(10)
AS
BEGIN
    INSERT INTO Departamento (nombre, codigo_dane)
    VALUES (@nombre, @codigo_dane);
END
GO

-- ACTUALIZAR DEPARTAMENTO
CREATE PROCEDURE usp_Departamento_Actualizar
    @id_departamento INT,
    @nombre NVARCHAR(100),
    @codigo_dane VARCHAR(10),
    @activo BIT
AS
BEGIN
    UPDATE Departamento
    SET nombre = @nombre,
        codigo_dane = @codigo_dane,
        activo = @activo
    WHERE id_departamento = @id_departamento;
END
GO

-- ELIMINAR DEPARTAMENTO (Físico)
CREATE PROCEDURE usp_Departamento_Eliminar
    @id_departamento INT
AS
BEGIN
    DELETE FROM Departamento WHERE id_departamento = @id_departamento;
END
GO

-- CONSULTAR DEPARTAMENTO POR ID
CREATE PROCEDURE usp_Departamento_ObtenerPorId
    @id_departamento INT
AS
BEGIN
    SELECT id_departamento, nombre, codigo_dane, activo 
    FROM Departamento 
    WHERE id_departamento = @id_departamento;
END
GO

-- LISTAR TODOS LOS DEPARTAMENTOS
CREATE PROCEDURE usp_Departamento_Listar
AS
BEGIN
    SELECT id_departamento, nombre, codigo_dane, activo 
    FROM Departamento;
END
GO


/* 
=============================
TABLA: CIUDAD.
=============================
*/

-- REGISTRAR CIUDAD
CREATE PROCEDURE usp_Ciudad_Registrar
    @nombre NVARCHAR(100),
    @codigo_dane VARCHAR(10),
    @id_departamento INT
AS
BEGIN
    INSERT INTO Ciudad (nombre, codigo_dane, id_departamento)
    VALUES (@nombre, @codigo_dane, @id_departamento);
END
GO

-- ACTUALIZAR CIUDAD
CREATE PROCEDURE usp_Ciudad_Actualizar
    @id_ciudad INT,
    @nombre NVARCHAR(100),
    @codigo_dane VARCHAR(10),
    @activo BIT,
    @id_departamento INT
AS
BEGIN
    UPDATE Ciudad
    SET nombre = @nombre,
        codigo_dane = @codigo_dane,
        activo = @activo,
        id_departamento = @id_departamento
    WHERE id_ciudad = @id_ciudad;
END
GO

-- ELIMINAR CIUDAD
CREATE PROCEDURE usp_Ciudad_Eliminar
    @id_ciudad INT
AS
BEGIN
    DELETE FROM Ciudad WHERE id_ciudad = @id_ciudad;
END
GO

-- LISTAR CIUDADES POR DEPARTAMENTO
CREATE PROCEDURE usp_Ciudad_ListarPorDepartamento
    @id_departamento INT
AS
BEGIN
    SELECT id_ciudad, nombre, codigo_dane, activo 
    FROM Ciudad 
    WHERE id_departamento = @id_departamento AND activo = 1;
END
GO


/* 
=============================
TABLA: CODIGO POSTAL.
=============================
*/

-- REGISTRAR CÓDIGO POSTAL
CREATE PROCEDURE usp_CodigoPostal_Registrar
    @codigo VARCHAR(10),
    @id_ciudad INT
AS
BEGIN
    INSERT INTO CodigoPostal (codigo, id_ciudad)
    VALUES (@codigo, @id_ciudad);
END
GO

-- ACTUALIZAR CÓDIGO POSTAL
CREATE PROCEDURE usp_CodigoPostal_Actualizar
    @id_codigo_postal INT,
    @codigo VARCHAR(10),
    @activo BIT,
    @id_ciudad INT
AS
BEGIN
    UPDATE CodigoPostal
    SET codigo = @codigo,
        activo = @activo,
        id_ciudad = @id_ciudad
    WHERE id_codigo_postal = @id_codigo_postal;
END
GO

-- CONSULTAR CÓDIGO POSTAL POR CIUDAD
CREATE PROCEDURE usp_CodigoPostal_ListarPorCiudad
    @id_ciudad INT
AS
BEGIN
    SELECT id_codigo_postal, codigo, activo 
    FROM CodigoPostal 
    WHERE id_ciudad = @id_ciudad;
END
GO


/* =============================================================================
   SECCIÓN 2: CLIENTES
   ============================================================================= */

/* 
=============================
TABLA: CLIENTE.
=============================
*/

-- REGISTRAR CLIENTE
CREATE PROCEDURE usp_Cliente_Registrar
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @email NVARCHAR(255),
    @telefono NVARCHAR(20),
    @contrasena_hash NVARCHAR(255)
AS
BEGIN
    INSERT INTO Cliente (nombres, apellidos, email, telefono, contrasena_hash)
    VALUES (@nombres, @apellidos, @email, @telefono, @contrasena_hash);
    
    SELECT SCOPE_IDENTITY() AS id_cliente; -- Importante para capturar el ID en .NET
END
GO

-- ACTUALIZAR CLIENTE
-- Nota: updated_at se actualiza aquí para llevar registro de cambios
CREATE PROCEDURE usp_Cliente_Actualizar
    @id_cliente BIGINT,
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @email NVARCHAR(255),
    @telefono NVARCHAR(20),
    @estado NVARCHAR(20)
AS
BEGIN
    UPDATE Cliente
    SET nombres = @nombres,
        apellidos = @apellidos,
        email = @email,
        telefono = @telefono,
        estado = @estado,
        updated_at = SYSDATETIME()
    WHERE id_cliente = @id_cliente;
END
GO

-- CAMBIAR CONTRASEÑA (Separado por seguridad)
CREATE PROCEDURE usp_Cliente_ActualizarPassword
    @id_cliente BIGINT,
    @nueva_contrasena_hash NVARCHAR(255)
AS
BEGIN
    UPDATE Cliente
    SET contrasena_hash = @nueva_contrasena_hash,
        updated_at = SYSDATETIME()
    WHERE id_cliente = @id_cliente;
END
GO

-- CONSULTAR CLIENTE POR EMAIL (Para el Login)
CREATE PROCEDURE usp_Cliente_ObtenerPorEmail
    @email NVARCHAR(255)
AS
BEGIN
    SELECT id_cliente, nombres, apellidos, email, contrasena_hash, estado
    FROM Cliente
    WHERE email = @email;
END
GO

-- LISTAR TODOS LOS CLIENTES
CREATE PROCEDURE usp_Cliente_Listar
AS
BEGIN
    SELECT id_cliente, nombres, apellidos, email, telefono, estado, created_at
    FROM Cliente;
END
GO

/* 
=============================
TABLA: DIRECCIÓN CLIENTE.
=============================
*/

-- REGISTRAR DIRECCIÓN
CREATE PROCEDURE usp_DireccionCliente_Registrar
    @descripcion NVARCHAR(500),
    @es_principal BIT,
    @id_cliente BIGINT,
    @id_codigo_postal INT
AS
BEGIN
    -- Si el cliente marca esta como principal, quitamos la marca a las demás
    IF @es_principal = 1
    BEGIN
        UPDATE DireccionCliente 
        SET es_principal = 0 
        WHERE id_cliente = @id_cliente;
    END

    INSERT INTO DireccionCliente (descripcion, es_principal, id_cliente, id_codigo_postal)
    VALUES (@descripcion, @es_principal, @id_cliente, @id_codigo_postal);
END
GO

-- ACTUALIZAR DIRECCIÓN
CREATE PROCEDURE usp_DireccionCliente_Actualizar
    @id_direccion BIGINT,
    @descripcion NVARCHAR(500),
    @es_principal BIT,
    @activo BIT,
    @id_codigo_postal INT
AS
BEGIN
    -- Obtenemos el id_cliente de esta dirección para manejar la principalidad
    DECLARE @id_cliente BIGINT;
    SELECT @id_cliente = id_cliente FROM DireccionCliente WHERE id_direccion = @id_direccion;

    IF @es_principal = 1
    BEGIN
        UPDATE DireccionCliente 
        SET es_principal = 0 
        WHERE id_cliente = @id_cliente;
    END

    UPDATE DireccionCliente
    SET descripcion = @descripcion,
        es_principal = @es_principal,
        activo = @activo,
        id_codigo_postal = @id_codigo_postal
    WHERE id_direccion = @id_direccion;
END
GO

-- LISTAR DIRECCIONES DE UN CLIENTE
CREATE PROCEDURE usp_DireccionCliente_ListarPorCliente
    @id_cliente BIGINT
AS
BEGIN
    SELECT d.id_direccion, d.descripcion, d.es_principal, d.activo, 
           cp.codigo AS codigo_postal, c.nombre AS ciudad_nombre
    FROM DireccionCliente d
    INNER JOIN CodigoPostal cp ON d.id_codigo_postal = cp.id_codigo_postal
    INNER JOIN Ciudad c ON cp.id_ciudad = c.id_ciudad
    WHERE d.id_cliente = @id_cliente AND d.activo = 1;
END
GO

-- ELIMINACIÓN LÓGICA DE DIRECCIÓN
CREATE PROCEDURE usp_DireccionCliente_Desactivar
    @id_direccion BIGINT
AS
BEGIN
    UPDATE DireccionCliente SET activo = 0 WHERE id_direccion = @id_direccion;
END
GO


/* =============================================================================
   SECCIÓN 3: CATALOGO DE PRODUCTOS
   ============================================================================= */

/* 
=============================
TABLA: CATEGORIA.
=============================
*/

-- REGISTRAR CATEGORÍA
CREATE PROCEDURE usp_Categoria_Registrar
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(500)
AS
BEGIN
    INSERT INTO Categoria (nombre, descripcion)
    VALUES (@nombre, @descripcion);
END
GO

-- ACTUALIZAR CATEGORÍA
CREATE PROCEDURE usp_Categoria_Actualizar
    @id_categoria INT,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(500),
    @activo BIT
AS
BEGIN
    UPDATE Categoria
    SET nombre = @nombre,
        descripcion = @descripcion,
        activo = @activo
    WHERE id_categoria = @id_categoria;
END
GO

-- LISTAR CATEGORÍAS (Solo activas para el cliente final)
CREATE PROCEDURE usp_Categoria_ListarActivas
AS
BEGIN
    SELECT id_categoria, nombre, descripcion 
    FROM Categoria 
    WHERE activo = 1;
END
GO

/* 
=============================
TABLA: PRODUCTO.
=============================
*/

-- REGISTRAR PRODUCTO
CREATE PROCEDURE usp_Producto_Registrar
    @nombre NVARCHAR(150),
    @descripcion NVARCHAR(2000),
    @sku NVARCHAR(100),
    @precio DECIMAL(10,2),
    @stock_cantidad INT,
    @id_categoria INT
AS
BEGIN
    INSERT INTO Producto (nombre, descripcion, sku, precio, stock_cantidad, id_categoria)
    VALUES (@nombre, @descripcion, @sku, @precio, @stock_cantidad, @id_categoria);
    
    SELECT SCOPE_IDENTITY() AS id_producto;
END
GO

-- ACTUALIZAR PRODUCTO
CREATE PROCEDURE usp_Producto_Actualizar
    @id_producto BIGINT,
    @nombre NVARCHAR(150),
    @descripcion NVARCHAR(2000),
    @sku NVARCHAR(100),
    @precio DECIMAL(10,2),
    @stock_cantidad INT,
    @activo BIT,
    @id_categoria INT
AS
BEGIN
    UPDATE Producto
    SET nombre = @nombre,
        descripcion = @descripcion,
        sku = @sku,
        precio = @precio,
        stock_cantidad = @stock_cantidad,
        activo = @activo,
        id_categoria = @id_categoria,
        updated_at = SYSDATETIME()
    WHERE id_producto = @id_producto;
END
GO

-- CONSULTAR PRODUCTO DETALLADO (Incluye nombre de categoría)
CREATE PROCEDURE usp_Producto_ObtenerPorId
    @id_producto BIGINT
AS
BEGIN
    SELECT p.*, c.nombre AS categoria_nombre
    FROM Producto p
    INNER JOIN Categoria c ON p.id_categoria = c.id_categoria
    WHERE p.id_producto = @id_producto;
END
GO

-- ACTUALIZAR STOCK (Uso frecuente en ventas/devoluciones)
CREATE PROCEDURE usp_Producto_ActualizarStock
    @id_producto BIGINT,
    @cantidad_cambio INT -- Positivo para sumar, Negativo para restar
AS
BEGIN
    UPDATE Producto 
    SET stock_cantidad = stock_cantidad + @cantidad_cambio,
        updated_at = SYSDATETIME()
    WHERE id_producto = @id_producto;
END
GO

/* 
=============================
TABLA: IMAGENPRODUCTO.
=============================
*/

-- REGISTRAR IMAGEN
CREATE PROCEDURE usp_ImagenProducto_Registrar
    @url_imagen NVARCHAR(500),
    @es_principal BIT,
    @orden TINYINT,
    @id_producto BIGINT
AS
BEGIN
    -- Si se marca como principal, las otras imágenes de ese producto dejan de serlo
    IF @es_principal = 1
    BEGIN
        UPDATE ImagenProducto SET es_principal = 0 WHERE id_producto = @id_producto;
    END

    INSERT INTO ImagenProducto (url_imagen, es_principal, orden, id_producto)
    VALUES (@url_imagen, @es_principal, @orden, @id_producto);
END
GO

-- ELIMINAR IMAGEN (Físico)
CREATE PROCEDURE usp_ImagenProducto_Eliminar
    @id_imagen INT
AS
BEGIN
    DELETE FROM ImagenProducto WHERE id_imagen = @id_imagen;
END
GO

-- LISTAR IMÁGENES DE UN PRODUCTO
CREATE PROCEDURE usp_ImagenProducto_ListarPorProducto
    @id_producto BIGINT
AS
BEGIN
    SELECT id_imagen, url_imagen, es_principal, orden 
    FROM ImagenProducto 
    WHERE id_producto = @id_producto
    ORDER BY orden ASC;
END
GO


/* =============================================================================
   SECCIÓN 4: RESEÑAS
   ============================================================================= */

/* 
=============================
TABLA: RESENA.
=============================
*/

-- REGISTRAR RESEÑA
-- La base de datos lanzará un error si el cliente ya reseñó este producto
CREATE PROCEDURE usp_Resena_Registrar
    @rating TINYINT,
    @comentario NVARCHAR(1000),
    @id_cliente BIGINT,
    @id_producto BIGINT
AS
BEGIN
    INSERT INTO Resena (rating, comentario, id_cliente, id_producto)
    VALUES (@rating, @comentario, @id_cliente, @id_producto);
END
GO

-- ACTUALIZAR RESEÑA
-- Permite al usuario editar su comentario o calificación inicial
CREATE PROCEDURE usp_Resena_Actualizar
    @id_resena BIGINT,
    @rating TINYINT,
    @comentario NVARCHAR(1000)
AS
BEGIN
    UPDATE Resena
    SET rating = @rating,
        comentario = @comentario
    WHERE id_resena = @id_resena;
END
GO

-- ELIMINAR RESEÑA
CREATE PROCEDURE usp_Resena_Eliminar
    @id_resena BIGINT
AS
BEGIN
    DELETE FROM Resena WHERE id_resena = @id_resena;
END
GO

-- LISTAR RESEÑAS POR PRODUCTO (Incluye nombres del cliente)
-- Aquí aplicamos el JOIN para no tener datos redundantes en la tabla Resena
CREATE PROCEDURE usp_Resena_ListarPorProducto
    @id_producto BIGINT
AS
BEGIN
    SELECT 
        r.id_resena, 
        r.rating, 
        r.comentario, 
        r.created_at, 
        c.nombres + ' ' + c.apellidos AS nombre_cliente
    FROM Resena r
    INNER JOIN Cliente c ON r.id_cliente = c.id_cliente
    WHERE r.id_producto = @id_producto
    ORDER BY r.created_at DESC;
END
GO

-- OBTENER PROMEDIO DE CALIFICACIÓN DE UN PRODUCTO
-- Útil para mostrar las "estrellitas" en el catálogo
CREATE PROCEDURE usp_Resena_ObtenerPromedio
    @id_producto BIGINT
AS
BEGIN
    SELECT 
        COUNT(id_resena) AS total_resenas,
        AVG(CAST(rating AS DECIMAL(3,2))) AS promedio_rating
    FROM Resena
    WHERE id_producto = @id_producto;
END
GO


/* =============================================================================
   SECCIÓN 5: PEDIDOS Y FACTURACIÓN
   ============================================================================= */

/* 
=============================
TABLA: PEDIDO.
=============================
*/

-- CREAR PEDIDO INICIAL
CREATE PROCEDURE usp_Pedido_Registrar
    @id_cliente BIGINT,
    @id_direccion BIGINT
AS
BEGIN
    INSERT INTO Pedido (id_cliente, id_direccion)
    VALUES (@id_cliente, @id_direccion);
    
    SELECT SCOPE_IDENTITY() AS id_pedido;
END
GO

-- ACTUALIZAR ESTADO DEL PEDIDO
-- Para transiciones como 'Pendiente' -> 'Confirmado' -> 'Enviado'
CREATE PROCEDURE usp_Pedido_ActualizarEstado
    @id_pedido BIGINT,
    @nuevo_estado NVARCHAR(30)
AS
BEGIN
    UPDATE Pedido
    SET estado = @nuevo_estado,
        updated_at = SYSDATETIME()
    WHERE id_pedido = @id_pedido;
END
GO

-- CONSULTAR CABECERA DE PEDIDO (Con datos de cliente y dirección)
CREATE PROCEDURE usp_Pedido_ObtenerPorId
    @id_pedido BIGINT
AS
BEGIN
    SELECT p.id_pedido, p.estado, p.created_at, 
           c.nombres + ' ' + c.apellidos AS cliente,
           d.descripcion AS direccion_entrega
    FROM Pedido p
    INNER JOIN Cliente c ON p.id_cliente = c.id_cliente
    INNER JOIN DireccionCliente d ON p.id_direccion = d.id_direccion
    WHERE p.id_pedido = @id_pedido;
END
GO

/* 
=============================
TABLA: DETALLE PEDIDO.
=============================
*/

-- AGREGAR PRODUCTO AL PEDIDO
-- Se captura el precio actual de la tabla Producto para "congelarlo"
CREATE PROCEDURE usp_DetallePedido_AgregarItem
    @id_pedido BIGINT,
    @id_producto BIGINT,
    @cantidad INT
AS
BEGIN
    DECLARE @precio_capturado DECIMAL(10,2);
    
    SELECT @precio_capturado = precio 
    FROM Producto 
    WHERE id_producto = @id_producto;

    INSERT INTO DetallePedido (id_pedido, id_producto, cantidad, precio_unitario)
    VALUES (@id_pedido, @id_producto, @cantidad, @precio_capturado);
END
GO

-- ELIMINAR ITEM DEL PEDIDO
CREATE PROCEDURE usp_DetallePedido_EliminarItem
    @id_detalle BIGINT
AS
BEGIN
    DELETE FROM DetallePedido WHERE id_detalle = @id_detalle;
END
GO

-- LISTAR TODOS LOS ITEMS DE UN PEDIDO (Con nombre de producto)
CREATE PROCEDURE usp_DetallePedido_ListarPorPedido
    @id_pedido BIGINT
AS
BEGIN
    SELECT dp.id_detalle, p.nombre AS producto, dp.cantidad, 
           dp.precio_unitario, dp.subtotal
    FROM DetallePedido dp
    INNER JOIN Producto p ON dp.id_producto = p.id_producto
    WHERE dp.id_pedido = @id_pedido;
END
GO

/* 
=============================
TABLA: FACTURA.
=============================
*/

-- GENERAR FACTURA
CREATE PROCEDURE usp_Factura_Registrar
    @nro_factura NVARCHAR(30),
    @subtotal DECIMAL(10,2),
    @iva DECIMAL(10,2),
    @id_pedido BIGINT
AS
BEGIN
    INSERT INTO Factura (nro_factura, subtotal, iva, id_pedido)
    VALUES (@nro_factura, @subtotal, @iva, @id_pedido);
    
    SELECT SCOPE_IDENTITY() AS id_factura;
END
GO

-- CONSULTAR FACTURA POR NÚMERO
CREATE PROCEDURE usp_Factura_ObtenerPorNumero
    @nro_factura NVARCHAR(30)
AS
BEGIN
    SELECT id_factura, nro_factura, subtotal, iva, total, fecha_emision, id_pedido
    FROM Factura
    WHERE nro_factura = @nro_factura;
END
GO

-- LISTAR FACTURAS DE UN CLIENTE
CREATE PROCEDURE usp_Factura_ListarPorCliente
    @id_cliente BIGINT
AS
BEGIN
    SELECT f.id_factura, f.nro_factura, f.total, f.fecha_emision, p.id_pedido
    FROM Factura f
    INNER JOIN Pedido p ON f.id_pedido = p.id_pedido
    WHERE p.id_cliente = @id_cliente
    ORDER BY f.fecha_emision DESC;
END
GO


/* =============================================================================
   SECCIÓN 6: PAGOS
   ============================================================================= */

/* 
=============================
TABLA: PAGO.
=============================
*/

-- REGISTRAR INTENTO DE PAGO
-- Se usa cuando el cliente inicia la transacción en la pasarela
CREATE PROCEDURE usp_Pago_Registrar
    @metodo_pago NVARCHAR(30),
    @monto DECIMAL(10,2),
    @referencia_externa NVARCHAR(255) = NULL,
    @id_factura BIGINT
AS
BEGIN
    INSERT INTO Pago (metodo_pago, monto, referencia_externa, id_factura)
    VALUES (@metodo_pago, @monto, @referencia_externa, @id_factura);
    
    SELECT SCOPE_IDENTITY() AS id_pago;
END
GO

-- ACTUALIZAR ESTADO DE PAGO
-- Vital para confirmar cuando la pasarela (PayPal/Stripe) responde 'Completado'
CREATE PROCEDURE usp_Pago_ActualizarEstado
    @id_pago BIGINT,
    @nuevo_estado NVARCHAR(20)
AS
BEGIN
    UPDATE Pago
    SET estado = @nuevo_estado,
        fecha_pago = SYSDATETIME()
    WHERE id_pago = @id_pago;
    
    -- Si el pago se completa, se podría disparar aquí la lógica para 
    -- actualizar el estado del pedido a 'Confirmado' desde el backend.
END
GO

-- CONSULTAR PAGOS DE UNA FACTURA
-- Permite ver el historial de intentos (Fallidos, Pendientes, Completados)
CREATE PROCEDURE usp_Pago_ListarPorFactura
    @id_factura BIGINT
AS
BEGIN
    SELECT id_pago, metodo_pago, monto, estado, referencia_externa, fecha_pago
    FROM Pago
    WHERE id_factura = @id_factura
    ORDER BY fecha_pago DESC;
END
GO

-- OBTENER DETALLE DE PAGO POR ID
CREATE PROCEDURE usp_Pago_ObtenerPorId
    @id_pago BIGINT
AS
BEGIN
    SELECT p.*, f.nro_factura 
    FROM Pago p
    INNER JOIN Factura f ON p.id_factura = f.id_factura
    WHERE p.id_pago = @id_pago;
END
GO

-- ELIMINAR REGISTRO DE PAGO (Uso administrativo)
CREATE PROCEDURE usp_Pago_Eliminar
    @id_pago BIGINT
AS
BEGIN
    DELETE FROM Pago WHERE id_pago = @id_pago;
END
GO


/* =============================================================================
   SECCIÓN 7: ENVÍOS
   ============================================================================= */

/* 
=============================
TABLA: ENVIO.
=============================
*/

-- REGISTRAR ENVÍO
-- Se dispara normalmente cuando el pedido cambia a estado 'En preparación' o 'Enviado'
CREATE PROCEDURE usp_Envio_Registrar
    @metodo_envio NVARCHAR(30),
    @costo_envio DECIMAL(10,2),
    @id_pedido BIGINT,
    @empresa_transporte NVARCHAR(100) = NULL,
    @numero_guia NVARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO Envio (metodo_envio, costo_envio, id_pedido, empresa_transporte, numero_guia)
    VALUES (@metodo_envio, @costo_envio, @id_pedido, @empresa_transporte, @numero_guia);
    
    SELECT SCOPE_IDENTITY() AS id_envio;
END
GO

-- ACTUALIZAR INFORMACIÓN DE RASTREO
-- Útil para cuando la transportadora asigna la guía después de creado el registro
CREATE PROCEDURE usp_Envio_ActualizarRastreo
    @id_envio BIGINT,
    @empresa_transporte NVARCHAR(100),
    @numero_guia NVARCHAR(100)
AS
BEGIN
    UPDATE Envio
    SET empresa_transporte = @empresa_transporte,
        numero_guia = @numero_guia
    WHERE id_envio = @id_envio;
END
GO

-- ACTUALIZAR ESTADO Y FECHAS
-- Maneja la transición de 'En tránsito' a 'Entregado'
CREATE PROCEDURE usp_Envio_ActualizarEstado
    @id_envio BIGINT,
    @nuevo_estado NVARCHAR(30)
AS
BEGIN
    DECLARE @fecha_despacho DATETIME2 = NULL;
    DECLARE @fecha_entrega DATETIME2 = NULL;

    -- Si pasa a tránsito, marcamos despacho
    IF @nuevo_estado = 'En transito' 
        SET @fecha_despacho = SYSDATETIME();
    
    -- Si se entrega, marcamos la fecha final
    IF @nuevo_estado = 'Entregado' 
        SET @fecha_entrega = SYSDATETIME();

    UPDATE Envio
    SET estado_envio = @nuevo_estado,
        fecha_despacho = ISNULL(@fecha_despacho, fecha_despacho),
        fecha_entrega = ISNULL(@fecha_entrega, fecha_entrega)
    WHERE id_envio = @id_envio;
END
GO

-- CONSULTAR ENVÍO POR ID DE PEDIDO
-- Muy usado en la vista de "Mis Pedidos" del cliente
CREATE PROCEDURE usp_Envio_ObtenerPorPedido
    @id_pedido BIGINT
AS
BEGIN
    SELECT id_envio, numero_guia, empresa_transporte, metodo_envio, 
           costo_envio, estado_envio, fecha_despacho, fecha_entrega
    FROM Envio
    WHERE id_pedido = @id_pedido;
END
GO

-- ELIMINAR ENVÍO (Solo en caso de cancelación total antes de despacho)
CREATE PROCEDURE usp_Envio_Eliminar
    @id_envio BIGINT
AS
BEGIN
    DELETE FROM Envio WHERE id_envio = @id_envio;
END
GO