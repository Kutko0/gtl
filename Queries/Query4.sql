SELECT Count(Loan.ID) as NumberOfBooksPerLastQuarter, 
    Count(Distinct Loan.CardID) as PeopleLoaned, 
    Count(Distinct Loan.ISBN) DifferentBooks from Loan
    Where LoanDate >= DATEADD(month, -3, getdate())