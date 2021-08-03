CREATE PROCEDURE SelectFinishedOrdersInfoForPastYear
AS
SELECT 
	Book.ISBN,
	Title,
	concat(FName, ' ', LName) as 'Loaner name',
	ActualReturnDate as 'Retuned on',
	InStock as 'In stock now'
FROM Orders
Left JOIN Loan
	ON Orders.LoanID = Loan.ID
INNER JOIN Book
	ON Orders.ISBN = Book.ISBN
INNER JOIN SubjectArea
	ON Book.SubjectID = SubjectArea.ID
Inner JOIN Members
	ON Members.CardID = Orders.CardID
Where OrderDate >= DATEADD(YEAR, -1, GETDATE())
AND LoanID IS NOT NULL
AND ActualReturnDate IS NOT NULL