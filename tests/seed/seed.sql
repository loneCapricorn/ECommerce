SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRANSACTION;
 
    -- Clean slate (optional)
    DELETE FROM dbo.ProductCategories;
    DELETE FROM dbo.Products;
    DELETE FROM dbo.Categories;
 
    -- Insert Categories
    INSERT INTO dbo.Categories (Name)
    VALUES
        (N'Electronics'),
        (N'Books'),
        (N'Home & Kitchen'),
        (N'Clothing'),
        (N'Sports');
 
    -- Capture Category IDs
    DECLARE @CategoryIds TABLE (Name NVARCHAR(100), CategoryId INT);
    INSERT INTO @CategoryIds (Name, CategoryId)
    SELECT Name, CategoryId
    FROM dbo.Categories
    WHERE Name IN (N'Electronics', N'Books', N'Home & Kitchen', N'Clothing', N'Sports');
 
    -- Insert Products (matching columns: Name, Description, Price, Stock)
    INSERT INTO dbo.Products (Name, Description, Price, Stock)
    VALUES
        (N'Wireless Headphones', N'Bluetooth over-ear headphones with noise cancelation', 99.99, 50),
        (N'Smartphone Case', N'Shock-absorbing case for 6.5-inch phones', 14.99, 200),
        (N'Non-stick Frying Pan', N'28cm aluminum pan with non-stick coating', 24.50, 120),
        (N'Yoga Mat', N'6mm thick non-slip mat for yoga and pilates', 19.95, 80),
        (N'Fantasy Novel', N'Hardcover edition of popular fantasy series', 29.00, 60),
        (N'Graphic T-Shirt', N'100% cotton t-shirt with minimalist print', 15.00, 150),
        (N'Stainless Steel Water Bottle', N'750ml insulated bottle keeps drinks cold for 24h', 22.00, 100);
 
    -- Capture Product IDs
    DECLARE @ProductIds TABLE (Name NVARCHAR(200), ProductId INT);
    INSERT INTO @ProductIds (Name, ProductId)
    SELECT Name, ProductId
    FROM dbo.Products
    WHERE Name IN (
        N'Wireless Headphones',
        N'Smartphone Case',
        N'Non-stick Frying Pan',
        N'Yoga Mat',
        N'Fantasy Novel',
        N'Graphic T-Shirt',
        N'Stainless Steel Water Bottle'
    );
 
    -- Map Products to Categories (ProductCategories has columns: ProductId, CategoryId)
    INSERT INTO dbo.ProductCategories (ProductId, CategoryId)
    SELECT p.ProductId, c.CategoryId
    FROM @ProductIds p
    JOIN @CategoryIds c
      ON (p.Name = N'Wireless Headphones'      AND c.Name = N'Electronics')
      OR (p.Name = N'Smartphone Case'          AND c.Name = N'Electronics')
      OR (p.Name = N'Non-stick Frying Pan'     AND c.Name = N'Home & Kitchen')
      OR (p.Name = N'Yoga Mat'                 AND c.Name = N'Sports')
      OR (p.Name = N'Fantasy Novel'            AND c.Name = N'Books')
      OR (p.Name = N'Graphic T-Shirt'          AND c.Name = N'Clothing')
      OR (p.Name = N'Stainless Steel Water Bottle' AND c.Name = N'Sports')
      OR (p.Name = N'Stainless Steel Water Bottle' AND c.Name = N'Home & Kitchen');
 
    COMMIT TRANSACTION;
    PRINT 'Seed data inserted successfully.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrState INT = ERROR_STATE();
    RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
END CATCH;
