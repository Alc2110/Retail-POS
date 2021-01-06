
-- Transaction example
BEGIN
BEGIN Transaction

    BEGIN Try
        --variables
        DECLARE @varCustomerID int;
        DECLARE @varStaffID int;
        DECLARE @varProductID int;
        SET @varCustomerID = 1;
        SET @varStaffID = 1;
        SET @varProductID = 1;

        --New record in Transactions table
        INSERT INTO  Transactions (Timestamp_, CustomerID, StaffID, ProductID)
        VALUES (SYSDATETIME(),@varCustomerID,@varStaffID,@varProductID)

        --Update inventory (Products table)
        --First retrieve the current quantity of this product
        DECLARE @varCurrQuantity int;
        SET @varCurrQuantity = (SELECT Quantity FROM Products WHERE ProductID=@varProductID)
        --And update it
        UPDATE Products
        SET Quantity = @varCurrQuantity-1
        WHERE ProductID = @varProductID
        
    END Try

    BEGIN Catch
        ROLLBACK Transaction
    END Catch

COMMIT Transaction
END