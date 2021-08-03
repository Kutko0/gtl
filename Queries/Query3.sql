Select TOP(10)  DT.AddressLine1,(Count(DT.CampusAddress)) / (Count(DISTINCT DT.CampusAddress)) as AveragePerCampus from 
(
SELECT Loan.CardID, LoanDate, Members.CampusAddress, Addresses.AddressLine1 from Loan
    Inner Join Members
    On Loan.CardID = Members.CardID
    Inner Join Addresses
    On Members.CampusAddress = Addresses.ID
    Where LoanDate >= DATEADD(year, -1, getdate())
) DT
Group by DT.AddressLine1
Order by AveragePerCampus desc