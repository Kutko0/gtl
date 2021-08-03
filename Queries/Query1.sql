Select ISBN, CardID, RequiredDate from Orders
Where LoanID IS NULL 
AND RequiredDate IS NOT NULL
Order by RequiredDate ASC