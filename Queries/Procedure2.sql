CREATE PROCEDURE SelectMembersWithInfo
AS
SELECT 
	SSN as 'Social security number',
	concat(FName, ' ', LName) as 'Full name',
	Phone,
	concat(AddressLine1 , City, ZIPID, ZipName) as 'Full Campus Address',
	CardID, 
	ExpiryDate as 'Card expiry',
	EmpType as 'Employee Type'
FROM Members
Inner Join
(
	SELECT Addresses.ID, AddressLine1, City, ZipName, Zip.ID as 'ZIPID' from Addresses
	inner join Zip
	On Addresses.ZipID = Zip.ID
) DT
On DT.ID = Members.CampusAddress
INNER JOIN Card
	ON Card.ID = Members.CardID
LEFT JOIN EmployeeType
	ON EmployeeType.ID = Members.EmployeeTypeID
	
