Select TOP (3) DTB.FullName, count(DTB.ISBN) as BooksLoaned from
(
Select ID, concat(Fname,' ', Lname) as FullName, DT.ISBN From AuthorName
inner Join 
(
SELECT Loan.ISBN, LoanDate, Book.AuthorID from Loan
inner join Book
On Loan.ISBN = Book.ISBN
Where LoanDate >= DATEADD(year, -1, getdate())
) DT
On DT.AuthorID = AuthorName.id
 ) DTB
 Group by DTB.FullName
 Order by BooksLoaned desc