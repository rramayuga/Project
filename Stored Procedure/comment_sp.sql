DELIMITER //

--GET ALL
CREATE PROCEDURE GetAllComments(
    IN p_stockId INT,
    IN p_page INT,
    IN p_pageSize INT
)
BEGIN
    SET @offset = (p_page - 1) * p_pageSize;
    SELECT c.*, u.UserName, s.Symbol
    FROM Comments c
    JOIN AppUsers u ON c.AppUserId = u.Id
    JOIN Stocks s ON c.StockId = s.Id
    WHERE (p_stockId IS NULL OR c.StockId = p_stockId)
    ORDER BY c.CreatedAt DESC
    LIMIT p_pageSize OFFSET @offset;
END //

--GET BY ID
CREATE PROCEDURE GetCommentById(IN p_id INT)
BEGIN
    SELECT c.*, u.UserName, s.Symbol
    FROM Comments c
    JOIN AppUsers u ON c.AppUserId = u.Id
    JOIN Stocks s ON c.StockId = s.Id
    WHERE c.Id = p_id;
END //

--ADD
CREATE PROCEDURE AddComment(
    IN p_content TEXT,
    IN p_stockId INT,
    IN p_appUserId VARCHAR(255)
)
BEGIN
    INSERT INTO Comments (Content, StockId, AppUserId, CreatedAt)
    VALUES (p_content, p_stockId, p_appUserId, NOW());
    SELECT * FROM Comments WHERE Id = LAST_INSERT_ID();
END //

--UPDATE
CREATE PROCEDURE UpdateComment(
    IN p_id INT,
    IN p_content TEXT
)
BEGIN
    UPDATE Comments
    SET Content = p_content,
        UpdatedAt = NOW()
    WHERE Id = p_id;
    SELECT * FROM Comments WHERE Id = p_id;
END //

--DELETE
CREATE PROCEDURE DeleteComment(IN p_id INT)
BEGIN
    DELETE FROM Comments WHERE Id = p_id;
END //

DELIMITER ;
