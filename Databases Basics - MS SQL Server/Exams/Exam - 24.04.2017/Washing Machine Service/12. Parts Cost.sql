SELECT ISNULL(SUM(p.Price * op.Quantity), 0) AS [Part Total]
FROM Parts AS p
INNER JOIN OrderParts AS op ON op.PartId = p.PartId
INNER JOIN Orders AS o ON o.OrderId = op.OrderId
WHERE DATEDIFF(WEEK, o.IssueDate, '04-24-2017') <= 3