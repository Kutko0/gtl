SELECT Datename(QUARTER, DATEADD(month, -3, getdate())) as Quarter,
    Count(Loan.ID) as NumberOfBooksPerMonth, 
    Count(Distinct Loan.CardID) as PeopleLoaned, 
    Count(Distinct Loan.ISBN) DifferentBooks from Loan
    Where LoanDate >= DATEADD(QUARTER, -1, getdate())