Drop trigger if exists take_stock;
Go
create trigger take_stock
ON Loan
After Insert
AS BEGIN
    Declare @ISBN VARCHAR(16);
    Select @ISBN = ISBN From INSERTED;
    Update Book SET Book.InStock = Book.InStock -1 Where Book.ISBN= @ISBN
    END;

Drop trigger if exists return_stock;
Go
create trigger return_stock
ON Loan
After Update
AS BEGIN
    Declare @ISBN VARCHAR(16);
    Declare @ActualReturnDate Datetime;
    Select @ISBN = ISBN, @ActualReturnDate = ActualReturnDate From Inserted;
    IF @ActualReturnDate IS NOT NULL Update Book SET Book.InStock = Book.InStock +1 Where Book.ISBN= @ISBN
    END;

Drop trigger if exists order_loanid_update;
Go
create trigger order_loanid_update
ON Loan
After Insert
AS BEGIN
    Declare @CardID int;
    Declare @ISBN varchar(16);
    Declare @ID int;
    Select @ISBN = ISBN, @CardID = CardID, @ID = ID From INSERTED;
    Update Orders SET Orders.LoanID = @ID Where Orders.CardID = @CardID AND Orders.ISBN = @ISBN AND Orders.LoanID IS NULL
    END;