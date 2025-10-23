DELIMITER //

--GET ALL
CREATE PROCEDURE GetAllStocks(
    IN p_searchTerm VARCHAR(100),
    IN p_page INT,
    IN p_pageSize INT
)
BEGIN
    SET @offset = (p_page - 1) * p_pageSize;
    SELECT *
    FROM Stocks
    WHERE (p_searchTerm IS NULL OR Symbol LIKE CONCAT('%', p_searchTerm, '%'))
    ORDER BY Id DESC
    LIMIT p_pageSize OFFSET @offset;
END //

--GET BY ID
CREATE PROCEDURE GetStockById(IN p_id INT)
BEGIN
    SELECT * FROM Stocks WHERE Id = p_id;
END //

--ADD
CREATE PROCEDURE AddStock(
    IN p_symbol VARCHAR(10),
    IN p_companyName VARCHAR(100),
    IN p_purchasePrice DECIMAL(18,2),
    IN p_marketCap BIGINT
)
BEGIN
    INSERT INTO Stocks (Symbol, CompanyName, PurchasePrice, MarketCap, CreatedAt)
    VALUES (p_symbol, p_companyName, p_purchasePrice, p_marketCap, NOW());
    SELECT * FROM Stocks WHERE Id = LAST_INSERT_ID();
END //

--UPDATE
CREATE PROCEDURE UpdateStock(
    IN p_id INT,
    IN p_symbol VARCHAR(10),
    IN p_companyName VARCHAR(100),
    IN p_purchasePrice DECIMAL(18,2),
    IN p_marketCap BIGINT
)
BEGIN
    UPDATE Stocks
    SET Symbol = p_symbol,
        CompanyName = p_companyName,
        PurchasePrice = p_purchasePrice,
        MarketCap = p_marketCap,
        UpdatedAt = NOW()
    WHERE Id = p_id;
    SELECT * FROM Stocks WHERE Id = p_id;
END //

--DELETE
CREATE PROCEDURE DeleteStock(IN p_id INT)
BEGIN
    DELETE FROM Stocks WHERE Id = p_id;
END //

DELIMITER ;
