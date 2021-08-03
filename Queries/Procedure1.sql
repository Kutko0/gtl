CREATE PROCEDURE SelectBooksWithInfo
AS
SELECT 
	ISBN, 
	Title,
	concat(FName, ' ' , Lname) As 'Author name',
	AreaType as 'Subject Area',
	Lang as 'Language',
	Cover as 'Cover type',
	Copies as 'Number of copies'
FROM Book
INNER JOIN AuthorName
	ON Book.AuthorID = AuthorName.ID
INNER JOIN CoverType
	ON Book.CoverID = CoverType.ID
INNER JOIN SubjectArea
	ON Book.SubjectID = SubjectArea.ID
INNER JOIN LanguageType
	ON Book.LangID = LanguageType.ID
GO;
