CREATE TRIGGER t_DeleteProduct ON Products INSTEAD OF DELETE
AS
BEGIN
	DECLARE @ProductId INT = (SELECT Id FROM deleted)

	DELETE FROM Feedbacks
	WHERE ProductId = @ProductId

	DELETE FROM ProductsIngredients
	WHERE ProductId = @ProductId

	DELETE FROM Products
	WHERE Id = @ProductId
END