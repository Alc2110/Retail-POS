CREATE VIEW outOfStockProducts AS
SELECT * FROM Products
WHERE Quantity = 0;
