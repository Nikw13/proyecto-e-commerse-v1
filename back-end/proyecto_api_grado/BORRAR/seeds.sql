-- ============================================================================
-- SEEDS PARA TABLA CLIENTE
-- ============================================================================

INSERT INTO Cliente (nombres, apellidos, email, telefono, contrasena_hash, estado)
VALUES 
('Juan', 'Pérez', 'juan.perez@email.com', '3001234567', '$2a$10$N9qo8uLOickgx2ZMRZoMye/U.VfGfLhY5u5M5xH5yY5K5xH5yY5K5', 'Activo'),
('María', 'García', 'maria.garcia@email.com', '3002345678', '$2a$10$N9qo8uLOickgx2ZMRZoMye/U.VfGfLhY5u5M5xH5yY5K5xH5yY5K5', 'Activo'),
('Carlos', 'Rodríguez', 'carlos.rodriguez@email.com', '3003456789', '$2a$10$N9qo8uLOickgx2ZMRZoMye/U.VfGfLhY5u5M5xH5yY5K5xH5yY5K5', 'Activo'),
('Ana', 'López', 'ana.lopez@email.com', '3004567890', '$2a$10$N9qo8uLOickgx2ZMRZoMye/U.VfGfLhY5u5M5xH5yY5K5xH5yY5K5', 'Activo'),
('Pedro', 'Martínez', 'pedro.martinez@email.com', '3005678901', '$2a$10$N9qo8uLOickgx2ZMRZoMye/U.VfGfLhY5u5M5xH5yY5K5xH5yY5K5', 'Inactivo');

-- ============================================================================
-- SEEDS PARA TABLA DEPARTAMENTO
-- ============================================================================

INSERT INTO Departamento (nombre, codigo_dane, activo)
VALUES 
('Cundinamarca', '25', 1),
('Antioquia', '05', 1),
('Valle del Cauca', '76', 1),
('Atlántico', '08', 1),
('Santander', '68', 1);

-- ============================================================================
-- SEEDS PARA TABLA CIUDAD
-- ============================================================================

INSERT INTO Ciudad (nombre, codigo_dane, activo, id_departamento)
VALUES 
('Bogotá', '11001', 1, 1),
('Medellín', '05001', 1, 2),
('Cali', '76001', 1, 3),
('Barranquilla', '08001', 1, 4),
('Bucaramanga', '68001', 1, 5);

-- ============================================================================
-- SEEDS PARA TABLA CODIGO POSTAL
-- ============================================================================

INSERT INTO CodigoPostal (codigo, activo, id_ciudad)
VALUES 
('110111', 1, 1),
('110221', 1, 1),
('050001', 1, 2),
('760001', 1, 3),
('080001', 1, 4),
('680001', 1, 5);

-- ============================================================================
-- SEEDS PARA TABLA DIRECCION CLIENTE
-- ============================================================================

INSERT INTO DireccionCliente (descripcion, es_principal, activo, id_cliente, id_codigo_postal)
VALUES 
('Calle 100 #45-67, Apto 301', 1, 1, 1, 1),
('Carrera 50 #20-30, Casa 5', 1, 1, 2, 3),
('Av. Jiménez #10-25, Oficina 402', 1, 1, 3, 1),
('Calle 80 #15-40, Torre 2, Apto 501', 1, 1, 4, 2),
('Diagonal 30 #5-10', 1, 1, 5, 5);

-- ============================================================================
-- SEEDS PARA TABLA CATEGORIA
-- ============================================================================

INSERT INTO Categoria (nombre, descripcion, activo)
VALUES 
('Electrónica', 'Dispositivos y accesorios electrónicos', 1),
('Ropa', 'Vestimenta y accesorios de moda', 1),
('Hogar', 'Muebles y artículos para el hogar', 1),
('Deportes', 'Equipos y accesorios deportivos', 1),
('Juguetes', 'Juguetes y juegos', 1);

-- ============================================================================
-- SEEDS PARA TABLA PRODUCTO
-- ============================================================================

INSERT INTO Producto (nombre, descripcion, sku, precio, stock_cantidad, activo, id_categoria)
VALUES 
('Laptop HP 15s', 'Laptop 15.6 pulgadas, 8GB RAM, 256GB SSD', 'LAP-HP-001', 2499000, 10, 1, 1),
('Celular Samsung A54', 'Samsung Galaxy A54 5G, 128GB', 'CEL-SAM-002', 1599000, 25, 1, 1),
('Camiseta Algodón', 'Camiseta básica de algodón, color negro', 'CAM-001', 29900, 100, 1, 2),
('Zapatillas Nike', 'Zapatillas running, modelo Air Max', 'ZAP-NIKE-001', 189900, 30, 1, 4),
('Sofa 3 puestos', 'Sofá de 3 puestos, color gris', 'SOF-001', 899000, 5, 1, 3),
('Balón Fútbol', 'Balón de fútbol profesional', 'BAL-FUT-001', 89000, 50, 1, 4),
('Consola PlayStation 5', 'Consola PS5 Digital Edition', 'CON-PS5-001', 2499000, 8, 1, 1),
('Muñeca Barbie', 'Muñeca Barbie clásica', 'MUÑ-BAR-001', 45000, 40, 1, 5);

-- ============================================================================
-- SEEDS PARA TABLA RESEÑA
-- ============================================================================

INSERT INTO Resena (rating, comentario, id_cliente, id_producto)
VALUES 
(5, 'Excelente producto, muy recomendable', 1, 1),
(4, 'Buena calidad, pero el precio es alto', 2, 1),
(5, 'El celular funciona perfectamente', 1, 2),
(3, 'La talla me quedó pequeña', 3, 3),
(5, 'Muy cómodas para correr', 4, 4),
(4, 'Buen sofa, pero took mucho tiempo en llegar', 2, 5);

-- ============================================================================
-- SEEDS PARA TABLA PEDIDO
-- ============================================================================

INSERT INTO Pedido (estado, id_cliente, id_direccion)
VALUES 
('Pendiente', 1, 1),
('Confirmado', 2, 2),
('Entregado', 1, 1),
('Cancelado', 3, 3),
('Enviado', 4, 4);

-- ============================================================================
-- SEEDS PARA TABLA DETALLE PEDIDO
-- ============================================================================

INSERT INTO DetallePedido (cantidad, precio_unitario, id_pedido, id_producto)
VALUES 
(2, 2499000, 1, 1),
(1, 29900, 1, 3),
(1, 1599000, 2, 2),
(3, 89000, 3, 6),
(1, 189900, 4, 4),
(1, 899000, 5, 5);

-- ============================================================================
-- SEEDS PARA TABLA FACTURA
-- ============================================================================

INSERT INTO Factura (nro_factura, subtotal, iva, id_pedido)
VALUES 
('FAC-2025-00001', 5027800, 603336, 1),
('FAC-2025-00002', 1599000, 191880, 2),
('FAC-2025-00003', 26700, 3204, 3),
('FAC-2025-00004', 189900, 22788, 4);

-- ============================================================================
-- SEEDS PARA TABLA PAGO
-- ============================================================================

INSERT INTO Pago (metodo_pago, monto, estado, referencia_externa, id_factura)
VALUES 
('Tarjeta', 5631136, 'Completado', 'TXN-001-ABC123', 1),
('PayPal', 1790880, 'Completado', 'TXN-002-DEF456', 2),
('Efectivo', 29904, 'Pendiente', NULL, 3),
('Transferencia', 212688, 'Fallido', 'TXN-004-GHI789', 4);

-- ============================================================================
-- SEEDS PARA TABLA ENVIO
-- ============================================================================

INSERT INTO Envio (numero_guia, empresa_transporte, metodo_envio, costo_envio, estado_envio, id_pedido)
VALUES 
('SEDEX-123456789', 'Envía', 'Estandar', 15000, 'Entregado', 1),
('SEDEX-987654321', 'Interrapidísimo', 'Express', 25000, 'En transito', 2),
(NULL, 'Coordinadora', 'Estandar', 12000, 'Pendiente', 3);

SELECT 'Se insertaron los seeds correctamente' AS mensaje;