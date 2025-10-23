DELIMITER //

--GET
CREATE PROCEDURE GetUserPortfolio(IN p_appUserId VARCHAR(255))
BEGIN
    SELECT p.Id AS PortfolioId, s.*
    FROM Portfolios p
    JOIN Stocks s ON p.StockId = s.Id
    WHERE p.AppUserId = p_appUserId
    ORDER BY s.Symbol ASC;
END //

--ADD
CREATE PROCEDURE AddPortfolio(
    IN p_stockId INT,
    IN p_appUserId VARCHAR(255)
)
BEGIN
    INSERT INTO Portfolios (StockId, AppUserId, CreatedAt)
    VALUES (p_stockId, p_appUserId, NOW());
    SELECT * FROM Portfolios WHERE Id = LAST_INSERT_ID();
END //

--DELETE
CREATE PROCEDURE DeletePortfolio(
    IN p_appUserId VARCHAR(255),
    IN p_stockSymbol VARCHAR(10)
)
BEGIN
    DELETE p FROM Portfolios p
    JOIN Stocks s ON p.StockId = s.Id
    WHERE p.AppUserId = p_appUserId AND s.Symbol = p_stockSymbol;
END //

DELIMITER ;
